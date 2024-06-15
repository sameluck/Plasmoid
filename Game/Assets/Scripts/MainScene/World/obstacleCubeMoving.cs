using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleCubeMoving : MonoBehaviour
{
    //the object
    //private GameObject Cube;

    private float Xpos;
    private float Ypos;
    private float Zpos;

    private float _movementStrength;

    //variable to help adjust the tickrate to the trig functions and lessen the workload on the pc
    
    private double _trigHelper;
    
    // movement attributes
    [SerializeField]private float smoothness;
    [SerializeField]public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _trigHelper = 6*Math.PI / smoothness;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(
            (float) (speed * Math.Sin(_movementStrength*_trigHelper)),
            0f,
            (float) (speed * Math.Cos(_movementStrength*_trigHelper))
        );
        _movementStrength++;
        _movementStrength %=6*smoothness;
    }
}
