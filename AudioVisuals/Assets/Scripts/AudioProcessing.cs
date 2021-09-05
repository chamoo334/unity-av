using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(AudioSource))]
public class AudioProcessing : MonoBehaviour
{
    AudioSource _audioSource;
    /* creates dropdown in gui for stereo, left, or right*/
    public enum _channel {Stereo, Left, Right};
    public _channel channel = new _channel();

    int[] hertzPerSample = {-1, 0};
    public static float[] _audioSamplesLeft = new float[512]; //left channel only
    public static float[] _audioSamplesRight = new float[512]; //right channel only

    public static float[] _freqBands = new float[8];
    public static float[] _freqBandBuffers = new float[8];

    float[] _buffDecrease = new float[8];
    public static float[] _freqBandHighs = new float[8];
    public static float[] _audioBands = new float[8];
    public static float[] _audioBandBuffs = new float[8];

    public static float _amplitude, _amplitudeBuff; 
    float _amplitudeHigh;
    float _audioProfile = 0;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        if (hertzPerSample[0] < 0){
            GetHertzPerSample();
        }
        GetSpectrum();
        MakeFreqBands();
        HandleBandBuffers();
        CreateAudioBands();
        GetTotalAmplitude();
    }

    void AudioProfile(float audioPro) //provide initial values for _freqhighest for smother graphics
    {
        for (int i=0; i<8; i++){
            _freqBandHighs[i] = audioPro;
        }
    }

    void GetSpectrum()
    {
        _audioSource.GetSpectrumData(_audioSamplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_audioSamplesRight, 1, FFTWindow.Blackman);

    }

    void GetHertzPerSample()
    {
        hertzPerSample[0] = GameAudio._audioFrequency / 512;
        int subBassMax = 60;

        // determining multiplier for first frequency band
        while (hertzPerSample[0] * hertzPerSample[1] < subBassMax)
        {
            hertzPerSample[1]+=1;
        }
    }

    void MakeFreqBands()
    {
        int numSamples = hertzPerSample[1];
        int count = 0;

        for (int i=0; i<8; i++)
        {
            float bandAverage = 0;

            for (int j=0; j<numSamples; j++)
            {
                if (channel == _channel.Stereo){
                    bandAverage += (_audioSamplesLeft[count] + _audioSamplesRight[count]) * (count + 1);
                }
                if (channel == _channel.Left){
                    bandAverage += (_audioSamplesLeft[count]) * (count + 1);
                }
                if(channel == _channel.Right){
                    bandAverage += (_audioSamplesRight[count]) * (count + 1);
                }
                count++;
            }

            bandAverage /= count;
            _freqBands[i] = bandAverage;

            numSamples *= 2;
        }
    }

    void HandleBandBuffers()
    {
        for (int i=0; i<8; i++)
        {
            if(_freqBands[i] < _freqBandBuffers[i])
            {
                _freqBandBuffers[i] -= _buffDecrease[i];
                _buffDecrease[i] *= 1.2f;
            }

            else if(_freqBands[i] > _freqBandBuffers[i])
            {
                _freqBandBuffers[i] = _freqBands[i];
                _buffDecrease[i] = 0.005f;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int i=0; i<8; i++)
        {
            if (_freqBands[i] > _freqBandHighs[i])
            {
                _freqBandHighs[i] = _freqBands[i];
            }
            _audioBands[i] = (_freqBands[i]/_freqBandHighs[i]);
            _audioBandBuffs[i] = (_freqBandBuffers[i]/_freqBandHighs[i]);

        }
    }

    void GetTotalAmplitude() //get average amplitude single value
    {
        float tempAmp=0, tempAmpBuff=0;
        
        for(int i=0; i<8;i++){
            tempAmp += _audioBands[i];
            tempAmpBuff += _audioBandBuffs[i];
        }

        if(tempAmp > _amplitudeHigh){
            _amplitudeHigh = tempAmp;
        }

        _amplitude = tempAmp / _amplitudeHigh;
        _amplitudeBuff = tempAmpBuff / _amplitudeHigh;
    }
}
