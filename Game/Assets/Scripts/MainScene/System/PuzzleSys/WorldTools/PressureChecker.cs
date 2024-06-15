using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureChecker : MonoBehaviour
{
    [SerializeField] private GameObject[] whatToUse;
    [SerializeField] private float minMassToUse = 1;
    [SerializeField] private float maxMassToUse = float.MaxValue;
    [SerializeField] private float pressureSpeed = 1f;
    [SerializeField] private bool sticky = false;
    [SerializeField] private AudioSource sound;
    
    private float _massOnPlate;
    private Transform _button;
    
    private Usable[] _scriptToUse;
    private bool _prevState;

    private void Awake()
    {
        _prevState = false;
        _massOnPlate = 0;
        _button = transform.parent;
        if (whatToUse != null)
        {
            _scriptToUse = new Usable[whatToUse.Length];
            for (var i = 0; i < whatToUse.Length; i++)
            {
                _scriptToUse[i] = whatToUse[i].GetComponent<Usable>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = Utilits.FindParentWithRigedbody(other.transform);
        Rigidbody RB = otherTransform.GetComponent<Rigidbody>();
        if (RB)
        {
            _massOnPlate += RB.mass;
        }
        
        if (_prevState && sticky)
        {
            _massOnPlate = minMassToUse + 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform otherTransform = Utilits.FindParentWithRigedbody(other.transform);
        Rigidbody RB = otherTransform.GetComponent<Rigidbody>();
        if (RB)
        {
            _massOnPlate -= RB.mass;
        }

        if (_prevState && sticky)
        {
            _massOnPlate = minMassToUse + 1f;
        }
    }
    
    private void UseAll(bool state = true)
    {
        foreach (var usable in _scriptToUse)
        {
            usable.use(state);
        }
    }

    
    private void Update()
    {
        if (minMassToUse <= _massOnPlate && _massOnPlate <= maxMassToUse)
        {
            if (!_prevState)
            {
                _prevState = true;
                if (_scriptToUse != null)
                {
                    sound.Play();
                    UseAll(true);
                }
            }
            if (_button.localPosition.y > 0.05)
            {
                //TODO sound of push
                _button.localPosition += Vector3.down * (pressureSpeed * Time.deltaTime);
            }
            else
            {
                sound.Stop();
            }
        }
        else
        {
            if (_prevState)
            {
                _prevState = false;
                if (_scriptToUse != null)
                {
                    sound.Play();
                    UseAll(false);
                }
            }
            if (_button.localPosition.y < 0.1f)
            {
                //TODO sound of unpush
                _button.localPosition += Vector3.up * (pressureSpeed * Time.deltaTime);
            }
            else
            {
                sound.Stop();
            }
        }
    }
}
