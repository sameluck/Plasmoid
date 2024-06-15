using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotComponent : MonoBehaviour, Usable
{
    [SerializeField] private GameObject[] whatToUse;
    private Usable[] _scriptToUse;
    private void Awake()
    {
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

    private void Start()
    {
        this.use(false);
    }

    public int use(bool state = true)
    {
        if (_scriptToUse != null)
        {
            UseAll(!state);
        }

        return 0;
    }
}
