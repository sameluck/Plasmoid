using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform orientation;
    public Transform cameraTarget;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager._playerHealth.alive())
        {
            Vector3 direction =
                cameraTarget.position -
                new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
            orientation.forward = direction.normalized;
        }
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
