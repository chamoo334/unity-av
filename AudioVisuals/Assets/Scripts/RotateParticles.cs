using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParticles : MonoBehaviour
{
    ParticleSystem rotatingPS;
    Vector3 rDirection = new Vector3(0, 0, 10);
    int baseRate = 100;
    float smoothTime, lastSmoothTime;
    float convertTime = 200f;
    float smooth;
    
    void Awake()
    {
        rotatingPS = this.GetComponent<ParticleSystem>();
        var psEmission = rotatingPS.emission;
        psEmission.rateOverTime = baseRate;
    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        var psEmission = rotatingPS.emission;
        var lights = rotatingPS.lights;
        smoothTime = AudioProcessing._amplitudeBuff;

        if (smoothTime < lastSmoothTime){
            rDirection *= -1;
        } 

        if (smoothTime >= (lastSmoothTime*2) || smoothTime < (lastSmoothTime/2)){
            lights.ratio = smoothTime;
        }

        psEmission.rateOverTime = baseRate * smoothTime;


        smooth = Time.deltaTime * smoothTime * convertTime;
        transform.Rotate(rDirection * smooth);
        lastSmoothTime = smoothTime;

    }
}
