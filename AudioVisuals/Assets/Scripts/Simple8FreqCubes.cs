using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple8FreqCubes : MonoBehaviour
{
    public bool _useBuffers;
    float _startScale = 1, _scaleMultiplier = 10000;
    public GameObject _sampleCubeFreq;
    GameObject[] _cubeFreqs = new GameObject[8];
    // Start is called before the first frame update
    void Start()
    {
        int[] centeringValues = {-7, -5, -3, -1, 1, 3, 5, 7};
        for(int i=0; i<8; i++)
        {
            GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubeFreq);
            _instanceSampleCube.transform.position = new Vector3(centeringValues[i], 0, 0);
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube" + i;
            _cubeFreqs[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<8; i++)
        {
            if (_useBuffers)
            {
                _cubeFreqs[i].transform.localScale = new Vector3(transform.localScale.x, (AudioProcessing._freqBandBuffers[i] * _scaleMultiplier) + _startScale, transform.localScale.z);
            }
            else
            {
                _cubeFreqs[i].transform.localScale = new Vector3(transform.localScale.x, (AudioProcessing._freqBands[i] * _scaleMultiplier) + _startScale, transform.localScale.z);
            }
        }
    }
}
