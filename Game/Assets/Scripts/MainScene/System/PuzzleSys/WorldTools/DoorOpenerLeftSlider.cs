using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenerLeftSlider : MonoBehaviour, Usable
{
    private bool state;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private float doorWidth;
    private float halfOfWidth;
    private void Awake()
    {
        halfOfWidth = doorWidth / 2;
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
            if (rightDoor.parent != leftDoor)
            {
                //TODO sound of move
                if (rightDoor.localPosition.z > -halfOfWidth)
                {
                    rightDoor.localPosition += Vector3.back * (doorOpeningSpeed * Time.deltaTime);
                }
                else if (rightDoor.localPosition.z <= -halfOfWidth && rightDoor.parent != leftDoor)
                {
                    rightDoor.parent = leftDoor;
                }
            }
            else
            {
                if (leftDoor.localPosition.z > -(doorWidth + halfOfWidth))
                {
                    //TODO sound of move
                    leftDoor.localPosition += Vector3.back * (doorOpeningSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (rightDoor.parent == leftDoor)
            {
                //TODO sound of move
                if (leftDoor.localPosition.z < -halfOfWidth)
                {
                    leftDoor.localPosition += Vector3.forward * (doorOpeningSpeed * Time.deltaTime);
                }
                else if (rightDoor.parent == leftDoor)
                {
                    rightDoor.parent = transform;
                }
            }
            else
            {
                if (rightDoor.localPosition.z < halfOfWidth)
                {
                    //TODO sound of move
                    rightDoor.localPosition += Vector3.forward * (doorOpeningSpeed * Time.deltaTime);
                }
            }
        }
    }
}
