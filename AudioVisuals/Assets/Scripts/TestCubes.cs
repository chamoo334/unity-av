using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _cubeSamples = new GameObject[512];
    public float _maxScale;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<512; i++){
            GameObject _instanceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 100;
            _cubeSamples[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<512; i++){
            if (_cubeSamples != null){
                _cubeSamples[i].transform.localScale = new Vector3(10,(AudioProcessing._audioSamplesLeft[i] * _maxScale + 2),10);
            }
        }
    }
}
