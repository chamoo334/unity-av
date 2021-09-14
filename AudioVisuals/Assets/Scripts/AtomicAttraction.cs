using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicAttraction : MonoBehaviour
{
    public GameObject _atom, _attractor;
    GameObject[] _atomArray, _attractorArray;
    public Gradient _gradient;
    public Material _material;
	Material[] _sharedMaterials;
	Color[] _sharedColors;

    public int[] _attractPoints;
    [Range(0,20)]
    public float _spacingBetweenAttractors;
    public Vector3 _spacingDirection;
    [Range(0,20)]
    public float _scaleAttractPoints;

    public int _numAtomsPerAttractor;
    public Vector2 _atomScaleMinMax;
    float[] _atomScaleSet;
    public float _strengthofAttraction, _maxMagnitude, _positionDistance;
    public bool _useGravity;

    public float _audioScaleMult, _audioEmissionMult;
    [Range(0.0f,1.0f)]
    public float _emissionThreshold;
    public enum _BufferEmission {Buffer, NoBuffer};
    public _BufferEmission useBufferEmission = new _BufferEmission();
    public enum _BufferEmittedColors {Buffer, NoBuffer}
    public _BufferEmittedColors  useBufferColorEmission = new _BufferEmittedColors();
    public enum _BufferAtomScaling {Buffer, NoBuffer};
    public _BufferAtomScaling useBufferAtomScale = new _BufferAtomScaling();
    float[] _audioBandEmissionThreshold;
    float[] _audioBandEmissionColor;
    float[] _audioBandScale;

    public bool _animatePosition;
    Vector3 _startPoint, _destinationPoint;
    public AnimationCurve _animationCurve;
    float _animationTimer;
    public float _animationSpeed;
    public bool _animationPosBuffered;

    float gradientStepSize; 
    // float gradientStepSize2 = 1.0f / _gradient.colorKeys.Length;

    /*Provides view of attractors while editing scene*/
    private void OnDrawGizmos() 
    {
        gradientStepSize = 1.0f / _attractPoints.Length; 
        for (int i=0; i<_attractPoints.Length; i++)
        {
            // calculate color
            Color color = _gradient.Evaluate(gradientStepSize * i);
            Gizmos.color = color;

            // calculate position
            Vector3 pos = new Vector3(transform.position.x + (_spacingBetweenAttractors * i * _spacingDirection.x),
                                    transform.position.y + (_spacingBetweenAttractors * i * _spacingDirection.y),
                                    transform.position.z + (_spacingBetweenAttractors * i * _spacingDirection.z));


            Gizmos.DrawSphere(pos, _scaleAttractPoints * 0.5f);
        }

        Gizmos.color = new Color(1,1,1);
        Vector3 startPoint = transform.position;
        Vector3 endpoint = transform.position + _destinationPoint;
        Gizmos.DrawLine(startPoint, endpoint);
    }

    /**/
    void Start()
    {
        _startPoint = transform.position;
        gradientStepSize = 1.0f / _attractPoints.Length; 
        _attractorArray = new GameObject[_attractPoints.Length];
        _atomArray = new GameObject[_attractPoints.Length * _numAtomsPerAttractor];
        _atomScaleSet = new float[_attractPoints.Length * _numAtomsPerAttractor];
        _sharedMaterials = new Material[8];
        _sharedColors = new Color[8];
        _audioBandEmissionThreshold = new float[8];
        _audioBandEmissionColor = new float[8];
        _audioBandScale = new float[8];
        
        int atomCount = 0;

        //instantiate attract points
        for (int i=0; i < _attractPoints.Length; i++){
            GameObject _attractInst = (GameObject)Instantiate(_attractor);
            _attractInst.transform.position = new Vector3(transform.position.x + (_spacingBetweenAttractors * i * _spacingDirection.x),
                                    transform.position.y + (_spacingBetweenAttractors * i * _spacingDirection.y),
                                    transform.position.z + (_spacingBetweenAttractors * i * _spacingDirection.z));
            _attractInst.transform.localScale = new Vector3(_scaleAttractPoints, _scaleAttractPoints, _scaleAttractPoints);
            _attractInst.transform.parent = this.transform;
            _attractorArray[i] = _attractInst;

            //set colors to material
            Material _matInstance = new Material(_material);
            _matInstance.EnableKeyword("_EMISSION");
            _sharedMaterials[i] = _matInstance;
            _sharedColors[i] = _gradient.Evaluate(gradientStepSize * i);
        

            //instantiate atoms
            for (int j=0; j < _numAtomsPerAttractor; j++){
                GameObject _atomInstance = (GameObject)Instantiate(_atom);
                _atomInstance.GetComponent<Atom2Attractor>()._attractedTo = _attractorArray[i].transform;
                _atomInstance.GetComponent<Atom2Attractor>()._strengthofAttraction = _strengthofAttraction;
                _atomInstance.GetComponent<Atom2Attractor>()._maxMag = _maxMagnitude;

                if (_useGravity){ _atomInstance.GetComponent<Rigidbody>().useGravity = true;}
                else {_atomInstance.GetComponent<Rigidbody>().useGravity = false;}

                // randomize atom position in reference to attractor and positionDistance
                _atomInstance.transform.position = new Vector3(_attractorArray[i].transform.position.x + Random.Range(-_positionDistance, _positionDistance),
                                                            _attractorArray[i].transform.position.y + Random.Range(-_positionDistance, _positionDistance),
                                                            _attractorArray[i].transform.position.z + Random.Range(-_positionDistance, _positionDistance));

                // randomize atom's scale based on min and max values set
                _atomScaleSet[atomCount] = Random.Range(_atomScaleMinMax.x, _atomScaleMinMax.y);
                _atomInstance.transform.localScale = new Vector3(_atomScaleSet[atomCount], _atomScaleSet[atomCount], _atomScaleSet[atomCount]);

                //apply material
                _atomInstance.GetComponent<MeshRenderer>().material = _sharedMaterials[i];

                _atomInstance.transform.parent = _attractInst.transform;
                _atomArray[atomCount] = _atomInstance;
                atomCount++;
            }
        }
    }

    /**/
    void Update()
    {
        SelectAudioValues();
        AtomBehavior();
        AnimatePosition();
    }

    /* Updates threshold, emittedColors, and scaling for each band*/
    void SelectAudioValues()
    {
        //threshold
        if(useBufferEmission == _BufferEmission.Buffer){
            for (int i=0; i<8; i++){
                _audioBandEmissionThreshold[i] = AudioProcessing._audioBandBuffs[i];
            }
        } else {
            for (int i=0; i<8; i++){
                _audioBandEmissionThreshold[i] = AudioProcessing._audioBands[i];
            }
        }

        //emittedColors
        if(useBufferColorEmission == _BufferEmittedColors.Buffer){
            for (int i=0; i<8; i++){
                _audioBandEmissionColor[i] = AudioProcessing._audioBandBuffs[i];
            }
        } else {
            for (int i=0; i<8; i++){
                _audioBandEmissionColor[i] = AudioProcessing._audioBands[i];
            }
        }

        //scaling
        if(useBufferAtomScale == _BufferAtomScaling.Buffer){
            for (int i=0; i<8; i++){
                _audioBandScale[i] = AudioProcessing._audioBandBuffs[i];
            }
        } else {
            for (int i=0; i<8; i++){
                _audioBandScale[i] = AudioProcessing._audioBands[i];
            }
        }

    }

    /*Sets audibandEmission relative to threshold. If below threshold, black. */
    void AtomBehavior()
    {
        int atomCount=0;

        for (int i=0; i<_attractPoints.Length; i++){
            Color _audioColor;
    
            if (_audioBandEmissionThreshold[_attractPoints[i]] >= _emissionThreshold){
                _audioColor = new Color(_sharedColors[i].r * _audioBandEmissionColor[_attractPoints[i]] * _audioEmissionMult,
                                    _sharedColors[i].g * _audioBandEmissionColor[_attractPoints[i]] * _audioEmissionMult,
                                    _sharedColors[i].b * _audioBandEmissionColor[_attractPoints[i]] * _audioEmissionMult,
                                    1);
                _sharedMaterials[i].SetColor("_EmissionColor", _audioColor);
            } else {
                _audioColor = new Color(0,0,0,1);
                _sharedMaterials[i].SetColor("_EmissionColor", _audioColor);
            }

            for(int j=0; j<_numAtomsPerAttractor; j++){
                _atomArray[atomCount].GetComponent<MeshRenderer>().material = _sharedMaterials[i];
                _atomArray[atomCount].transform.localScale = new Vector3(_atomScaleSet[atomCount] + _audioBandScale[_attractPoints[i]] * _audioScaleMult,
                                                            _atomScaleSet[atomCount] + _audioBandScale[_attractPoints[i]] * _audioScaleMult,
                                                            _atomScaleSet[atomCount] + _audioBandScale[_attractPoints[i]] * _audioScaleMult);

                atomCount++;
            }
        }
    }

    /*Attractors movement timing based on audioband values.
    TODO: adjust newX to ensure object remains in view*/
    void AnimatePosition()
    {
        if (_animatePosition){

            float newX = Mathf.Cos(AudioProcessing._amplitudeBuff)*100;
            // if (newX > (transform.position.x + 10) && newX < (transform.position.x-10)){
            //     newX *= -1;
            // }
            float newY = Mathf.Sin(AudioProcessing._amplitudeBuff)*100;
            if (newY > 50){
                newY -= 50;
            } else if (newY < 0){
                newY = 10;
            }

            float newZ = Mathf.Sqrt(Mathf.Pow((newX - transform.position.x), 2) + Mathf.Pow((newY - transform.position.y), 2));
            if (newZ > 100){
                newZ -= 100;
            } else if (newZ < -100){
                newZ += 100;
            }

            _destinationPoint = new Vector3(transform.position.x, newY, newZ);
            // _destinationPoint = new Vector3(newX, newY, newZ);

            
            if (_animationPosBuffered){
                _animationTimer = Time.deltaTime * AudioProcessing._amplitudeBuff* _animationSpeed;
            } else {
                _animationTimer = Time.deltaTime * AudioProcessing._amplitude * _animationSpeed;
            }

            if (_animationTimer >= 1){ _animationTimer -= 1f;}

            float _alphaTime2 = _animationCurve.Evaluate (_animationTimer);

            transform.position = Vector3.Lerp(_startPoint,_destinationPoint, _alphaTime2);
        }
    }
}
