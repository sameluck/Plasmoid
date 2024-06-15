using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalAnd : MonoBehaviour, Usable
{

    [SerializeField] private GameObject[] whatToUse;
    [SerializeField] private bool sticky;
    private LogicalIn[] _childrenScripts;
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
        _state = false;
        _childrenScripts = new LogicalIn[transform.childCount - 1];
        for (int i = 1; i < transform.childCount; i++)
        {
            _childrenScripts[i - 1] = transform.GetChild(i).GetComponent<LogicalIn>();
        }
    }

    public bool _state;
    
    
    private void UseAll(bool state = true)
    {
        foreach (var usable in _scriptToUse)
        {
            usable.use(state);
        }
    }
    
    public int use(bool state = true)
    {
        if (!sticky || !this._state)
        {
            this._state = true;
            foreach (var childrenScript in _childrenScripts)
            {
                if (!childrenScript.state)
                {
                    this._state = false;
                    UseAll(_state);
                    return -1;
                }
            }
            UseAll(_state);
        }

        return 1;
    }
}
