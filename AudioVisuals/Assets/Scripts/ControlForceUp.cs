using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlForceUp : MonoBehaviour
{
    ParticleSystemForceField ffUp;
    float lastAmp = 0;

    void Awake() 
    {
        ffUp = this.GetComponent<ParticleSystemForceField>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currAmp = AudioProcessing._amplitudeBuff;

        if (currAmp >= (lastAmp*2) || currAmp < (lastAmp/2)){
            ffUp.startRange = currAmp;
        }
        
    }
}
