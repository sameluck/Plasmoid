using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorPlate : MonoBehaviour
{
    [SerializeField] private GameObject[] whatToUse;
    [SerializeField] private float minMassToUse = 1;
    [SerializeField] private float maxMassToUse = float.MaxValue;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material actMaterial;
    [SerializeField] private AudioSource sound;
    [SerializeField] private bool sticky;
    
    private float _massOnPlate;
    private Renderer _buttonRenderer;
    
    private Usable[] _scriptToUse;
    private bool _prevState;
    private Collider _collider;

    private LinkedList<Rigidbody> _onMe;

    private void UseAll(bool state = true)
    {
        foreach (var usable in _scriptToUse)
        {
            usable.use(state);
        }
    }
    
    private void Awake()
    {
        _onMe = new LinkedList<Rigidbody>();
        _prevState = false;
        _massOnPlate = 0;
        _buttonRenderer = transform.parent.GetComponent<Renderer>();
        if (whatToUse != null)
        {
            _scriptToUse = new Usable[whatToUse.Length];
            for (var i = 0; i < whatToUse.Length; i++)
            {
                _scriptToUse[i] = whatToUse[i].GetComponent<Usable>();
            }
        }
        _collider = transform.GetComponent<Collider>();
        hologramControl.HologramDestroyed += RecalculateMass;
    }

    private void RecalculateMass(object sender, EventArgs e)
    {
        if (_collider != null)
        {
            LinkedList<Rigidbody> tmpList = new LinkedList<Rigidbody>();
            foreach (var rb in _onMe)
            {
                if (rb != null)
                {
                    tmpList.AddLast(rb);
                }
            }
            
            _onMe.Clear();
            //_onMe = tmpList;
            _massOnPlate = 0;
            
            foreach (var rb in tmpList)
            {
                _onMe.AddLast(rb);
                _massOnPlate += rb.mass;
            }
            tmpList.Clear();
            checkMass();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = Utilits.FindParentWithRigedbody(other.transform);
        Rigidbody rb = otherTransform.GetComponent<Rigidbody>();
        if (rb)
        {
            _onMe.AddLast(rb);
            _massOnPlate += rb.mass;
        }
        checkMass();
    }

    private void OnTriggerExit(Collider other)
    {
        Transform otherTransform = Utilits.FindParentWithRigedbody(other.transform);
        Rigidbody rb = otherTransform.GetComponent<Rigidbody>();
        if (rb)
        {
            _onMe.Remove(rb);
            _massOnPlate -= rb.mass;
        }
        checkMass();
    }
    
    private void checkMass(){
        
        if (_prevState && sticky)
        {
            return;
        }
        
        if (minMassToUse <= _massOnPlate && _massOnPlate <= maxMassToUse)
        {
            if (!_prevState)
            {
                _prevState = true;
                _buttonRenderer.material = actMaterial;
                //TODO sound of activation
                sound.Play();
                if (_scriptToUse != null)
                {
                    UseAll(true);
                }
            }
        }
        else
        {
            if (_prevState)
            {
                _prevState = false;
                _buttonRenderer.material = baseMaterial;
                //TODO sound of deactivation
                sound.Play();
                if (_scriptToUse != null)
                {
                    foreach (var usable in _scriptToUse)
                    {
                        UseAll(false);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        hologramControl.HologramDestroyed -= RecalculateMass;
    }
}
