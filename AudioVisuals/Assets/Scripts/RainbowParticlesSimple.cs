using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowParticlesSimple : MonoBehaviour
{
    float timeCount, _speed, _width, _height;
    Vector3 _center;
    // Start is called before the first frame update
    void Start()
    {
        _center = new Vector3(0, 0, 0);
        timeCount = 0;
        _speed = 3;
        _width = 50;
        _height = 50;
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime*_speed;

        float x=_center.x + Mathf.Cos(timeCount)*_width;
        float y=_center.y;
        float z=_center.z + Mathf.Sin(timeCount)*_height;

        transform.position = new Vector3(x, y, z);
    }
}
