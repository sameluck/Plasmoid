using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalIn : MonoBehaviour, Usable
{
    public bool state;
    private LogicalAnd parentScript;
    private void Awake()
    {
        parentScript = transform.parent.GetComponent<LogicalAnd>();
        state = false;
    }


    public int use(bool state = true)
    {
        this.state = state;
        parentScript.use();
        return 0;
    }
}
