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
    public static string _channelSetting = "";

    int[] hertzPerSample = {-1, 0};
    public static float[] _audioSamplesLeft = new float[512];
    public static float[] _audioSamplesRight = new float[512];

    public static float[] _freqBands = new float[8];
    public static float[] _freqBandBuffers = new float[8];
    float[] _buffDecrease = new float[8];

    public static float[] _freqBandsLeft = new float[8];
    public static float[] _freqBandBuffersLeft = new float[8];
    float[] _buffDecreaseLeft = new float[8];

    public static float[] _freqBandsRight = new float[8];
    public static float[] _freqBandBuffersRight = new float[8];
    float[] _buffDecreaseRight = new float[8];

    public static float[] _freqBandHighs = new float[8];
    public static float[] _audioBands = new float[8];
    public static float[] _audioBandBuffs = new float[8];

    public static float _amplitude, _amplitudeBuff; 
    float _amplitudeHigh;
    float _audioProfile = 0;

    /*  obtain audio source component and initialize _freqBandHighest values */
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /* Set initial audioProfile and continue to process audio on each update */
    void Update()
    {
        if (hertzPerSample[0] < 0){
            AudioProfile(_audioProfile);
        }
        GetSpectrum();
        MakeFreqBands();
        HandleBandBuffers();
        CreateAudioBands();
        GetAmplitude();
    }


    /* Provide initial values for _freqBandHighest for smoothe graphics and obtain channelSettings. 
    Calculate hertz per sample based on audio settings output sample rate and 
    multiplier for calculating sample size for first frequency band.
    */
    void AudioProfile(float audioPro)
    {
        _channelSetting = channel.ToString();

        for (int i=0; i<8; i++){
            _freqBandHighs[i] = audioPro;
        }

        hertzPerSample[0] = AudioSettings.outputSampleRate / 512; // GetSpectrumData uses outputSampleRate
        int finalMultiplier = AudioSettings.outputSampleRate / 20000; // adjust band size based on outputSamplerate to audible freqeuncy of 20k
        int subBassMax = 60;

        while (hertzPerSample[0] * hertzPerSample[1] < subBassMax)
        {
            hertzPerSample[1]+=1;
        }

        hertzPerSample[1] *= finalMultiplier;
    }

    /* obtain left and right channel samples (512 each) using Blackman.
    Divide into 3 for use by other objects. */
    void GetSpectrum()
    {
        _audioSource.GetSpectrumData(_audioSamplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_audioSamplesRight, 1, FFTWindow.Blackman);

    }

    /* Condense samples into 8 bands*/
    void MakeFreqBands()
    {
        int numSamples = hertzPerSample[1];
        int count = 0;

        for (int i=0; i<8; i++)
        {
            float bandAverage = 0, bandAverageLeft = 0, bandAverageRight = 0;

            for (int j=0; j<numSamples; j++)
            {
                if (channel == _channel.Stereo){
                    bandAverage += (_audioSamplesLeft[count] + _audioSamplesRight[count]) * (count + 1);
                    bandAverageLeft += (_audioSamplesLeft[count]) * (count + 1);
                    bandAverageRight += (_audioSamplesRight[count]) * (count + 1);
                }
                if (channel == _channel.Left){
                    bandAverage += (_audioSamplesLeft[count]) * (count + 1);
                }
                if(channel == _channel.Right){
                    bandAverage += (_audioSamplesRight[count]) * (count + 1);
                }
                count++;
            }

            _freqBands[i] = bandAverage / count;
            _freqBandsLeft[i] = bandAverageLeft / count;
            _freqBandsRight[i] = bandAverageRight / count;

            numSamples *= 2;
            if (i == 6) { numSamples += 2; }
        }
        
    }

    /* Band buffers to reduce speed at which item reduces when value is lower than previous value.
    TODO: Condense the if else statements - possibly by multiD array*/
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
            if (_channelSetting == "Stereo"){
                if(_freqBandsLeft[i] < _freqBandBuffersLeft[i])
                {
                    _freqBandBuffersLeft[i] -= _buffDecreaseLeft[i];
                    _buffDecreaseLeft[i] *= 1.2f;
                }

                else if(_freqBandsLeft[i] > _freqBandBuffersLeft[i])
                {
                    _freqBandBuffersLeft[i] = _freqBandsLeft[i];
                    _buffDecreaseLeft[i] = 0.005f;
                }

                if(_freqBandsRight[i] < _freqBandBuffersRight[i])
                {
                    _freqBandBuffersRight[i] -= _buffDecreaseRight[i];
                    _buffDecreaseRight[i] *= 1.2f;
                }

                else if(_freqBandsRight[i] > _freqBandBuffersRight[i])
                {
                    _freqBandBuffersRight[i] = _freqBandsRight[i];
                    _buffDecreaseRight[i] = 0.005f;
                }
            }
        }
    }

    /* Create ranged values from 0-1 for each band by dividing current value by highest played*/
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

    /* Calculates average amplitude and amplitudeBuffer of all bands */
    void GetAmplitude() 
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
