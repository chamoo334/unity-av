using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom2Attractor : MonoBehaviour
{
    Rigidbody _rigidbody;
    public Transform _attractedTo;
    public float _strengthofAttraction, _maxMag;

    /**/
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    /*Updates position relative to _attractredTo. Note: _attractedTo is set in AtomicAttractions' start()*/
    void Update()
    {
        if (_strengthofAttraction >= 0)
        {
            // determine direction of force
            Vector3 _forceDirection = _attractedTo.position - transform.position;
            // apply force
            _rigidbody.AddForce(_strengthofAttraction * _forceDirection);

            // ensure magnitude does not exceed maxMAg
            if (_rigidbody.velocity.magnitude > _maxMag)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxMag;
            }
        }
    }
}
