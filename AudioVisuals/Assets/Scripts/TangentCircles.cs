using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherits from CircleTangent.cs
public class TangentCircles : CircleTangent
{
    public GameObject _circleObject;
    private GameObject _innerCircle, _outerCircle;
    Vector4 _innerCircleVec, _outerCircleVec;
    public float _innerRadius, _outerRadius;
    [Range(1,4)]
    public int _circlesPerBand;
    int _numberBands = 8;
    int _numberCircles;
    GameObject[] _allTangentCircles;
    Vector4[] _allTangentsData;
    float [] _mainPosManip = new float[2];
    float _currAmp, _lastAmp = 0.1f, _minOutTanDist = 0.1f;
    Vector2 _asympSmoother = new Vector2(1, 1);
    float _smoothness = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _numberCircles = _circlesPerBand * _numberBands;

        _allTangentsData = new Vector4[_numberCircles];
        _allTangentCircles = new GameObject[_numberCircles];
        for (int i=0; i<_numberBands; i++){
            for (int j=0; j<_circlesPerBand; j++){
                GameObject _tanInst = (GameObject)Instantiate(_circleObject);
                _tanInst.transform.parent = this.transform;
                _allTangentCircles[i*_circlesPerBand+j] = _tanInst;
            }
        }

        // _innerCircle = (GameObject)Instantiate(_circleObject);
        // _outerCircle = (GameObject)Instantiate(_circleObject);

        _innerCircleVec = new Vector4(transform.position.x, transform.position.y, transform.position.z, _innerRadius);
        _outerCircleVec = new Vector4(transform.position.x, transform.position.y, transform.position.z, _outerRadius);

        // _innerCircle.transform.parent = this.transform;
        // _outerCircle.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _currAmp = AudioProcessing._amplitudeBuff;
        if (_currAmp == 0) {_currAmp = 0.1f;}

        AlterMainPosition();
        UpdateInnerOuterVecs();
        UpdateAllOtherPositions();
        
        _lastAmp = _currAmp;
    }

    void AlterMainPosition()
    {
        float _rot;

        if (_currAmp >= _lastAmp){
            _rot = Random.Range(0, 360) *_currAmp * Time.deltaTime;
        } else {
            _rot = Random.Range(-360, 0) *_currAmp * Time.deltaTime;
        }
        

        transform.Rotate(0f, 0f, _rot, Space.Self);
    }


    void UpdateInnerOuterVecs()
    {
        float __speed = 200f * Time.deltaTime;
        float _ampRatio = _lastAmp/_currAmp;
        Vector4 oldPosLims = new Vector4(_innerCircleVec.x - 2f, _innerCircleVec.x + 2f, _innerCircleVec.z - 2f, _innerCircleVec.z + 2f);

        _asympSmoother = new Vector2(( _asympSmoother.x * (1 - _smoothness) + Mathf.Cos(_ampRatio)  * _smoothness), 
                                    ( _asympSmoother.y * (1 - _smoothness) + Mathf.Sin(_ampRatio) * _smoothness));
        
        if (oldPosLims.x <= _asympSmoother.x && _asympSmoother.x <= oldPosLims.y){ _asympSmoother.x *= -1; }
        if (oldPosLims.z <= _asympSmoother.y && _asympSmoother.y <= oldPosLims.w){ _asympSmoother.y *= -1; }


       _innerCircleVec.x = (_asympSmoother.x * (_outerCircleVec.w - _innerCircleVec.w) * (1 - _minOutTanDist)) + _outerCircleVec.x;
       _innerCircleVec.z = (_asympSmoother.y * (_outerCircleVec.w - _innerCircleVec.w) * (1 - _minOutTanDist)) + _outerCircleVec.z;
        // _innerCircleVec.w *= ampRatio; //TODO: determine appropriate innerCirc ratio change
        
        // outer circle position remains centered around main position
        _outerCircleVec.x = transform.position.x;
        _innerCircleVec.y =_outerCircleVec.y = transform.position.y;
        _outerCircleVec.z = transform.position.z;
        
    }

    void UpdateAllOtherPositions()
    {

        // _innerCircle.transform.position = new Vector3(_innerCircleVec.x, _innerCircleVec.y, _innerCircleVec.z);
        // _innerCircle.transform.localScale = new Vector3(_innerCircleVec.w, _innerCircleVec.w, _innerCircleVec.w) * 2;

        // _outerCircle.transform.position = new Vector3(_outerCircleVec.x, _outerCircleVec.y, _outerCircleVec.z);
        // _outerCircle.transform.localScale = new Vector3(_outerCircleVec.w, _outerCircleVec.w, _outerCircleVec.w) * 2;

        for (int i=0; i<_numberBands; i++){
            for (int j=0; j<_circlesPerBand; j++){
                int pos = i*_circlesPerBand+j;
                _allTangentsData[pos] = GetTangentCircle(_outerCircleVec, _innerCircleVec, (360f/_numberCircles * (pos)));
                _allTangentCircles[pos].transform.position = new Vector3(_allTangentsData[pos].x + _outerCircleVec.x,
                                                                        _allTangentsData[pos].y + _outerCircleVec.y,
                                                                        _allTangentsData[pos].z + _outerCircleVec.z);
                _allTangentCircles[pos].transform.localScale = new Vector3(_allTangentsData[pos].w, _allTangentsData[pos].w, _allTangentsData[i*_circlesPerBand+j].w) * 2;
            }
        }
    }
}
