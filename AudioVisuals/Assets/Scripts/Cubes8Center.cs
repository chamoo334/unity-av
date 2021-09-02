using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes8Center : MonoBehaviour
{
    public bool _useBuffers;
    float _startScale = 1, _scaleMultiplier = 10000;
    public GameObject _sampleCubeFreq;
    GameObject[] _cubeFreqs = new GameObject[8];
    // Start is called before the first frame update
    void Start()
    {
        int[] centeringValues = {-50, -35, -20, -5, 5, 20, 35, 50};
        for(int i=0; i<8; i++)
        {
            GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubeFreq);
            _instanceSampleCube.transform.position = new Vector3(centeringValues[i], 0, 0);
            _instanceSampleCube.transform.localScale = new Vector3(25, 0, 25);
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube" + i;
            _cubeFreqs[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVec;
        for(int i=0; i<8; i++)
        {   
            newVec = _cubeFreqs[i].transform.localScale;
            
            if (_useBuffers)
            {
                newVec.y = (AudioProcessing._freqBandBuffers[i] * _scaleMultiplier) + _startScale;
            }
            else
            {
                newVec.y = (AudioProcessing._freqBands[i] * _scaleMultiplier) + _startScale;
            }

            _cubeFreqs[i].transform.localScale = newVec;
        }
    }
}
