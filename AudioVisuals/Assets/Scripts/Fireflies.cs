using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    public GameObject _flies, _forceFields;
    GameObject[] _fliesArray = new GameObject[4];
    GameObject[] _forceFieldsArray = new GameObject[4];
    GameObject singleFlies;
    GameObject singleField;

    float[] bands4 = new float[4];
    float lastAmp = 0;


    // Start is called before the first frame update
    void Start()
    {
        CreateForceFields();
        CreateFlies();
    }

    // Update is called once per frame
    void Update()
    {
        ReduceBands();
        AnimateFlies();
    }

    void CreateForceFields(){
        Vector3[] pos = {new Vector3(50, 25, 0), new Vector3(-50, 25, 0), new Vector3(0, 25, 50), new Vector3(0, 25, -50)};
        for (int i=0; i < 4; i++){
            GameObject _forceInst = (GameObject)Instantiate(_forceFields);
            _forceInst.transform.position = pos[i];
            _forceInst.transform.parent = this.transform;
            _forceFieldsArray[i] = _forceInst;

            ParticleSystemForceField psff = _forceInst.GetComponent<ParticleSystemForceField>();
            psff.directionX = pos[i].x;
            psff.directionY = pos[i].y;
            psff.directionZ = pos[i].z;
            
        }
    }


    void CreateFlies()
    {
        Vector3[] pos = {new Vector3(50, 25, 50), new Vector3(50, 25, -50), new Vector3(-50, 25, -50), new Vector3(-50, 25, 50)};
        for (int i=0; i < 4; i++){
            GameObject _fliesInst = (GameObject)Instantiate(_flies);
            _fliesInst.transform.position = pos[i];
            _fliesInst.transform.parent = this.transform;
            _fliesArray[i] = _fliesInst;
        }
    }

    /**/
    void ReduceBands()
    {
        for (int i=0; i<4; i++){
            bands4[i] = (AudioProcessing._audioBands[i] + AudioProcessing._audioBands[i+1]) / 2.0f;
        }
    }

    void AnimateFlies()
    {
        if (AudioProcessing._amplitude > lastAmp){
            Vector3 zeroPos = _fliesArray[0].GetComponent<ParticleSystem>().transform.position;
            Vector3 newPos;

            for (int i=0; i<4; i++){
                if (i == 3){
                    newPos = zeroPos; }
                else {
                    newPos = _fliesArray[i+1].GetComponent<ParticleSystem>().transform.position; 
                }
                ParticleSystem ps = _fliesArray[i].GetComponent<ParticleSystem>();
                Vector3 oldPos = ps.transform.position;
                ps.transform.position = newPos;
                // ps.main.
                newPos = oldPos;
            }
        } else {
            Vector3 newPos = _fliesArray[3].GetComponent<ParticleSystem>().transform.position;
            for (int i=0; i<4; i++){
                ParticleSystem ps = _fliesArray[i].GetComponent<ParticleSystem>();
                Vector3 oldPos = ps.transform.position;
                ps.transform.position = newPos;
                newPos = oldPos;
            }
        }

        lastAmp = AudioProcessing._amplitude;
    }

}
