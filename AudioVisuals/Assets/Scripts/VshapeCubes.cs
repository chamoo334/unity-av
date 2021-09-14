using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VshapeCubes : MonoBehaviour
{
    float _startScale = 1, _scaleMultiplier = 10000;
    public GameObject _sampleCubeFreq1;
    public GameObject _sampleCubeFreq2;
    GameObject[,] _cubeFreqsV = new GameObject[2, 8];
    public bool _useBuffers1;
    public bool _useBuffers2;
    
    void Start()
    {
        createCubes();
    }

    void Update()
    {

        if (AudioProcessing._channelSetting == "Stereo"){
            StereoChannel();
        }
        else{
            SingleChannel();
        }
    }

    /*Instantiates 16 cubes in the shape of a V that become children of attached object*/
    void createCubes()
    {
        int [] starts ={5, -55};

        for (int i=0; i<8; i++)
        {
            GameObject _instanceSampleCube1 = (GameObject)Instantiate(_sampleCubeFreq1);
            GameObject _instanceSampleCube2 = (GameObject)Instantiate(_sampleCubeFreq2);
            _instanceSampleCube1.transform.position = new Vector3(starts[0]*-1, 0, starts[1]);
            _instanceSampleCube2.transform.position = new Vector3(starts[0], 0, starts[1]);
            _instanceSampleCube1.transform.localScale = new Vector3(25, 0, 25);
            _instanceSampleCube2.transform.localScale = new Vector3(25, 0, 25);
            _instanceSampleCube1.transform.parent = this.transform;
            _instanceSampleCube2.transform.parent = this.transform;
            _instanceSampleCube1.name = "SampleCubeL" + i;
            _instanceSampleCube2.name = "SampleCubeR" + i;
            _cubeFreqsV[0,i] = _instanceSampleCube1;
            _cubeFreqsV[1,i] = _instanceSampleCube2;
            starts[0] += 10;
            starts[1] += 10;


        }
    }

    /* Updates Y position of cubes based on frequency band values if single channel is used*/
    void SingleChannel()
    {
        Vector3 cubeLVec, cubeRVec;
        Material _materialL, _materialR;
        Color _colorEmission;
        
        for(int i=0; i<8; i++)
        {
            cubeLVec = _cubeFreqsV[0,i].transform.localScale;
            cubeRVec = _cubeFreqsV[1,i].transform.localScale;
            _colorEmission = new Color(AudioProcessing._audioBandBuffs[i],AudioProcessing._audioBandBuffs[i],AudioProcessing._audioBandBuffs[i], 1);

            _materialL = _cubeFreqsV[0,i].GetComponent<MeshRenderer>().material;
            _materialL.EnableKeyword("_EMISSION");
            _materialL.SetColor("_EmissionColor", _colorEmission);

            _materialR = _cubeFreqsV[1,i].GetComponent<MeshRenderer>().material;
            _materialR.EnableKeyword("_EMISSION");
            _materialR.SetColor("_EmissionColor", _colorEmission);

            if (_useBuffers1)
            {
                cubeLVec.y = (AudioProcessing._freqBandBuffers[i] * _scaleMultiplier) + _startScale;
            }
            else
            {
                cubeLVec.y = (AudioProcessing._freqBands[i] * _scaleMultiplier) + _startScale;
            }

            if (_useBuffers2)
            {
                cubeRVec.y = (AudioProcessing._freqBandBuffers[i] * _scaleMultiplier) + _startScale;
            }
            else
            {
                cubeRVec.y = (AudioProcessing._freqBands[i] * _scaleMultiplier) + _startScale;
            }

            _cubeFreqsV[0,i].transform.localScale = cubeLVec;
            _cubeFreqsV[1,i].transform.localScale = cubeRVec;

        }
    }

    /* Updates left and right side Y positions based on individual channel data*/
    void StereoChannel()
    {
        Vector3 cubeLVec, cubeRVec;
        Material _materialL, _materialR;
        Color _colorEmission;
        
        for(int i=0; i<8; i++)
        {
            cubeLVec = _cubeFreqsV[0,i].transform.localScale;
            cubeRVec = _cubeFreqsV[1,i].transform.localScale;
            _colorEmission = new Color(AudioProcessing._audioBandBuffs[i],AudioProcessing._audioBandBuffs[i],AudioProcessing._audioBandBuffs[i], 1);

            _materialL = _cubeFreqsV[0,i].GetComponent<MeshRenderer>().material;
            _materialL.EnableKeyword("_EMISSION");
            _materialL.SetColor("_EmissionColor", _colorEmission);

            _materialR = _cubeFreqsV[1,i].GetComponent<MeshRenderer>().material;
            _materialR.EnableKeyword("_EMISSION");
            _materialR.SetColor("_EmissionColor", _colorEmission);

            if (_useBuffers1)
            {
                cubeLVec.y = (AudioProcessing._freqBandBuffersLeft[i] * _scaleMultiplier) + _startScale;
            }
            else
            {
                cubeLVec.y = (AudioProcessing._freqBandsLeft[i] * _scaleMultiplier) + _startScale;
            }

            if (_useBuffers2)
            {
                cubeRVec.y = (AudioProcessing._freqBandBuffersRight[i] * _scaleMultiplier) + _startScale;
            }
            else
            {
                cubeRVec.y = (AudioProcessing._freqBandsRight[i] * _scaleMultiplier) + _startScale;
            }

            _cubeFreqsV[0,i].transform.localScale = cubeLVec;
            _cubeFreqsV[1,i].transform.localScale = cubeRVec;

        }
    }
}
