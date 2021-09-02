using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(AudioSource))]
public class AudioProcessing : MonoBehaviour
{
    AudioSource _audioSource;
    int[] hertzPerSample = {-1, 0};
    public static float[] _audioSamples = new float[512];
    public static float[] _freqBands = new float[8];
    public static float[] _freqBandBuffers = new float[8];
    float[] _buffDecrease = new float[8];
    public static float[] _freqBandHighs = new float[8];
    public static float[] _audioBands = new float[8];
    public static float[] _audioBandBuffs = new float[8];


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
    }

    void GetSpectrum()
    {
        _audioSource.GetSpectrumData(_audioSamples, 0, FFTWindow.Blackman);
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
                bandAverage += _audioSamples[count] * (count + 1);
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
}
