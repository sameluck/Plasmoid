using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoorOpener : MonoBehaviour, Usable
{
    private bool state;
    [SerializeField] private Transform door;
    [SerializeField] private float doorDeltaHeight;
    private Vector3 basePos;
    private void Awake()
    {
        basePos = door.position;
        state = false;
    }

    public int use(bool state = true)
    {
        this.state = state;
        if (state)
        {
            return 1;
        }
        return -1;
    }

    [SerializeField] private float doorOpeningSpeed;
    private void Update()
    {
        if (state)
        {
            if (door.localPosition.y < basePos.y + doorDeltaHeight)
            {
                //TODO sound of move
                door.localPosition += Vector3.up * (Time.deltaTime * doorOpeningSpeed);
            }
        }
        else
        {
            if (door.localPosition.y > basePos.y)
            {
                //TODO sound of move
                door.localPosition += Vector3.down * (Time.deltaTime * doorOpeningSpeed);
            }
        }
    }
}
