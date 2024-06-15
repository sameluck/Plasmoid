using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : MonoBehaviour
{
    private float accumulatedTime;

    [SerializeField] private float delay;
    [SerializeField] private GameObject whatToUse;
    private Usable useScript;
    private void Awake()
    {
        if (whatToUse != null)
        {
            useScript = whatToUse.GetComponent<Usable>();
        }
        
        accumulatedTime = 0;
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime;
        if (accumulatedTime > delay)
        {
            useScript.use();
            accumulatedTime %= delay;
        }
    }
}
