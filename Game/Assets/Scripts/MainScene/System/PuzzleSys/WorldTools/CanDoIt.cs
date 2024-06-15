using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDoIt : MonoBehaviour, Usable
{
    [SerializeField] private GameObject[] whatToUse;
    [SerializeField] private Material baseMat;
    [SerializeField] private Material actMat;
    private Usable[] _scriptToUse;
    private Renderer _myRenderer;
    private void Awake()
    {
        _myRenderer = transform.GetChild(0).GetComponent<Renderer>();
        if (whatToUse != null)
        {
            _scriptToUse = new Usable[whatToUse.Length];
            for (var i = 0; i < whatToUse.Length; i++)
            {
                _scriptToUse[i] = whatToUse[i].GetComponent<Usable>();
            }
        }
    }
    
    private void UseAll(bool state = true)
    {
        foreach (var usable in _scriptToUse)
        {
            usable.use(state);
        }
    }

    public int use(bool state = true)
    {
        if (state)
        {
            //TODO sound of activation
            _myRenderer.material = actMat;
        }
        else
        {
            //TODO sound of deactivation
            _myRenderer.material = baseMat;
        }

        UseAll(state);

        return 0;
    }
}
