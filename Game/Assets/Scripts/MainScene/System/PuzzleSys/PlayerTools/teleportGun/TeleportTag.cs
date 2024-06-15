using System;
using System.Collections;
using UnityEngine;

public class TeleportTag : MonoBehaviour
{

    [SerializeField] private float hh;
    [SerializeField] private float rr;
    [SerializeField] private float timeToOpen = 1;
    [SerializeField] private Transform pfPlayerHologram;
    [SerializeField] private float velocity = 1;
    
    private Rigidbody _myRigidbody;
    private bool _timerStarted;
    private int _ignorMask = -5;
    private Vector3 _moveVect;
    
    public void Setup(Vector3 shootDir)
    {
        Destroy(gameObject, 60);
        _moveVect = transform.forward * velocity;
        _myRigidbody = GetComponent<Rigidbody>();
        _myRigidbody.velocity = _moveVect;
    }
    
    
    private bool CheckAllSides()
    {
        var position = transform.position;

        float xDelta = 0;
        float yDelta = 0;
        float zDelta = 0;
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit;
        bool wasHit1 = false;
        bool wasHit2 = false;
        
        if (Physics.Raycast(position, Vector3.up, out hit1, hh, _ignorMask))
        {
            wasHit1 = true;
        }
        if (Physics.Raycast(position, Vector3.down, out hit2, hh, _ignorMask))
        {
            wasHit2 = true;
        }

        if (wasHit1 && wasHit2)
        {
            return false;
        }
        if(wasHit1 || wasHit2)
        {
            if (wasHit1)
            {
                hit = hit1;
            }
            else
            {
                hit = hit2;
            }
            float yy = (position - hit.point).y;
            if (Math.Sign(yy) < 0)
            {
                yDelta = -(hh + yy);
            }
            else
            {
                yDelta = hh - yy;
            }
        }

        wasHit1 = false;
        wasHit2 = false;
        
        if (Physics.Raycast(position, Vector3.right, out hit1, rr, _ignorMask))
        {
            wasHit1 = true;
        }
        if (Physics.Raycast(position, Vector3.left, out hit2, rr, _ignorMask))
        {
            wasHit2 = true;
        }
        
        
        if (wasHit1 && wasHit2)
        {
            return false;
        }
        if(wasHit1 || wasHit2)
        {
            if (wasHit1)
            {
                hit = hit1;
            }
            else
            {
                hit = hit2;
            }
            float xx = (position - hit.point).x;
            if (Math.Sign(xx) < 0)
            {
                xDelta = -(rr + xx);
            }
            else
            {
                xDelta = rr - xx;
            }
        }
        
        
        wasHit1 = false;
        wasHit2 = false;
        
        if (Physics.Raycast(position, Vector3.forward, out hit1, rr, _ignorMask))
        {
            wasHit1 = true;
        }
        if (Physics.Raycast(position, Vector3.back, out hit2, rr, _ignorMask))
        {
            wasHit2 = true;
        }
        
        
        if (wasHit1 && wasHit2)
        {
            return false;
        }
        if(wasHit1 || wasHit2)
        {
            if (wasHit1)
            {
                hit = hit1;
            }
            else
            {
                hit = hit2;
            }
            float zz = (position - hit.point).z;
            if (Math.Sign(zz) < 0)
            {
                zDelta = -(rr + zz);
            }
            else
            {
                zDelta = rr - zz;
            }
        }
        
        _myRigidbody.velocity = new Vector3(xDelta, yDelta, zDelta) / timeToOpen;
        if (!_timerStarted)
        {
            StartCoroutine(timerToOpen());
        }

        _timerStarted = true;
        return true;
    }

    private void Awake()
    {
        _ignorMask &= ~(1 << 6);
        _ignorMask &= ~(1 << 5);
        _ignorMask &= ~(1 << 8);
        _myRigidbody = transform.GetComponent<Rigidbody>();
    }

    private void Open()
    {
        //TODO sound of electric explosion
        Transform playerHologram = Instantiate(pfPlayerHologram, transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.up));
        teleportGun.PutHologram(playerHologram.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator timerToOpen()
    {
        yield return new WaitForSeconds(timeToOpen);
        if (CheckAllSides())
        {
            Open();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //TODO sound of idle hologram
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (!collision.collider.CompareTag("LogicTrigger"))
        {
            if (collision.transform.CompareTag("Bullet"))
            {
                Destroy(gameObject);
            }
            else if (!CheckAllSides())
            {
                Destroy(gameObject);
            }
        }
    }
    
}
