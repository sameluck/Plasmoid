using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hologramControl : MonoBehaviour
{
    private Rigidbody _rb;
    private GameObject _originalPlayer;
    private Rigidbody _opRb;
    private Transform _opForward;
    
    public static event EventHandler<EventArgs> HologramDestroyed;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalPlayer = UnificControl.Player;
        _opRb = _originalPlayer.GetComponent<Rigidbody>();
        _opForward = _originalPlayer.transform.GetChild(1);
    }

    private void FixedUpdate()
    {
        //TODO sound of idle hologram
        float upVel = _rb.velocity.y;
        
        if (_opRb.velocity.y > 0)
        {
            //upVel = _opRb.velocity.y*1.1f;
        }

        _rb.velocity = new Vector3(_opRb.velocity.x * 1.1f, upVel, _opRb.velocity.z*1.1f);
        if (_rb.velocity.y != 0)
        {
            _rb.drag = 0;
        }
        else
        {
            _rb.drag = 5;
        }

        _rb.angularVelocity = _opRb.angularVelocity;
        transform.forward = _opForward.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        teleportGun.HologramsList.Remove(gameObject);
        _rb.mass = 0;
        if (HologramDestroyed != null)
        {
            HologramDestroyed.Invoke(this, EventArgs.Empty);
        }
    }
}
