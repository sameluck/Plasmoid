using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, E_Usable
{
    [SerializeField] private bool useState;
    [SerializeField] private bool sticky;
    [SerializeField] private GameObject[] whatToUse;
    
    [SerializeField] private Material baseMat;
    [SerializeField] private Material actMat;
    
    [SerializeField] private float secondsToChange = 0.1f;

    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource nuh_uh_Sound;

    private bool _usedThis;
    private bool _localState;
    private Usable[] _scriptToUse;
    private bool _blocker;
    private Renderer _myRenderer;
    
    private void Awake()
    {
        _blocker = false;
        _usedThis = false;
        _localState = false;
        if (whatToUse != null)
        {
            _scriptToUse = new Usable[whatToUse.Length];
            for (var i = 0; i < whatToUse.Length; i++)
            {
                _scriptToUse[i] = whatToUse[i].GetComponent<Usable>();
            }
        }

        _myRenderer = GetComponent<Renderer>();
    }

    private void ChangeMatToDefault()
    {
        //sound of deactivation
        clickSound.Play();
        _myRenderer.material = baseMat;
    }
    private void ChangeMatToActive()
    {
        //sound of activation
        clickSound.Play();
        _myRenderer.material = actMat;
    }

    private void UseAll(bool state = true)
    {
        foreach (var usable in _scriptToUse)
        {
            usable.use(state);
        }
    }

    private IEnumerator ClickChange()
    {
        ChangeMatToActive();
        UseAll(true);
        yield return new WaitForSeconds(secondsToChange);
        ChangeMatToDefault();
        UseAll(false);
    }

    public int use(bool state)
    {
        if (_blocker)
        {
            nuh_uh_Sound.Play();
            return 0;
            //sound of not use
        }
        if (state && _scriptToUse != null)
        {
            if (sticky)
            {
                if (_usedThis)
                {
                    //sound of not use
                    nuh_uh_Sound.Play();
                    return -2;
                }
                _usedThis = true;
                UseAll(true);
                ChangeMatToActive();
                return 2;
            }
            else
            {
                if (useState)
                {
                    _localState = !_localState;
                    UseAll(_localState);
                    if (_localState)
                    {
                        ChangeMatToActive();
                    }
                    else
                    {
                        ChangeMatToDefault();
                    }
                }
                else
                {
                    StartCoroutine(ClickChange());
                }
                return 1;
            }
            
        }
        nuh_uh_Sound.Play();
        //sound of not use
        return -1;
    }
}
