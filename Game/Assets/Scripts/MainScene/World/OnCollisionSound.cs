using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private float speedToSound = 2f;
    [SerializeField] private Rigidbody rb;
private void OnCollisionEnter(Collision collision)
    {
        if (rb != null)
        {
            if (rb.velocity.magnitude > speedToSound)
            {
                sound.Play();
            }
        }
        else
        {
            sound.Play();
        }

    }
}
