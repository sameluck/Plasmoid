using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDoorOpener : MonoBehaviour, Usable
{
    private bool _state;
    [SerializeField] private Transform door;
    [SerializeField] private float doorWidth;
    [SerializeField] private AudioSource sound;
    private void Awake()
    {
        _state = false;
    }

    public int use(bool state = true)
    {
        sound.Play();
        this._state = state;
        if (state)
        {
            return 1;
        }
        return -1;
    }

    [SerializeField] private float doorOpeningSpeed;
    private void Update()
    {
        if (_state)
        {
            if (door.localPosition.z > -doorWidth)
            {
                //TODO sound of move
                door.localPosition += Vector3.back * (doorOpeningSpeed * Time.deltaTime);
            }
            else
            {
                sound.Stop();
            }
        }
        else
        {
            if (door.localPosition.z < 0)
            {
                //TODO sound of move
                door.localPosition += Vector3.forward * (doorOpeningSpeed * Time.deltaTime);
            }
            else
            {
                sound.Stop();
            }
        }
    }
}
