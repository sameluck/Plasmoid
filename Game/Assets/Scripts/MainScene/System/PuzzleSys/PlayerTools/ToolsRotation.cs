using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsRotation : MonoBehaviour
{
    private Transform _playerBody;

    private void Start()
    {
        _playerBody = UnificControl.Player.transform.GetChild(1);
    }
    
    void Update()
    {
        transform.position = _playerBody.position + _playerBody.up;
        transform.forward = _playerBody.forward;
    }
}
