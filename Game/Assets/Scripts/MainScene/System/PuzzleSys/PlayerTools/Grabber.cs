using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private float grabForce;
    [SerializeField] private AudioSource sound;
    
    private GameObject _heldObject;
    private Rigidbody _heldObjectRigidbody;
    
    private Transform _playerBody;
    
    private float _dragMem;
    private bool _canGrab;
    
    private void GrabSomething(object sender, UnificControl.RayShootEventArgs e)
    {
        if (_canGrab)
        {
            if (_heldObject == null)
            {
                if (e.IsHit)
                {
                    Vector3 onObject = e.HitPos - transform.position;
                    if (Utilits.CheckForward(_playerBody.forward, onObject))
                    {
                        return;
                    }
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, onObject, out hit, grabDistance, UnificControl.GetClassicReachIgnoreMask()))
                    {
                        GrabObject(Utilits.FindParentWithRigedbody(hit.transform).gameObject);
                    } 
                }
            }
            else
            {
                DropObject();
            }
        }
        
    }

    private void Start()
    {
        _canGrab = true;
        _playerBody = UnificControl.Player.transform.GetChild(1);
        UnificControl.FButton += GrabSomething;
        //TeleportPlatform.DoingPlayerTeleportStationar += CheckHeldObjectOnPlayerTeleport;
        teleportGun.DoingPlayerTeleport += CheckHeldObjectOnPlayerTeleport;
    }

    private void Update()
    {
        if (_heldObject != null)
        {
            HoldObject();
        }
    }

    void HoldObject()
    {
        float dist = Vector3.Distance(_heldObject.transform.position, holdPoint.position);
        if (dist > 2f)
        {
            DropObject();
        }
        else if (dist > 0.1f)
        {
            //TODO sound of hold
            Vector3 moveDir = holdPoint.position - _heldObject.transform.position;
            _heldObjectRigidbody.AddForce(moveDir * grabForce);
            _heldObject.transform.forward = _playerBody.forward;
        }

        
    }

    
    
    void GrabObject(GameObject grabbedObject)
    {
        if (grabbedObject.CompareTag("Grabbable") || grabbedObject.CompareTag("GrabbableNotTeleportable"))
        {
            if (grabbedObject.GetComponent<Rigidbody>())
            {
                //TODO sound of grab
                sound.Play();
                _heldObjectRigidbody = grabbedObject.GetComponent<Rigidbody>();
                _heldObjectRigidbody.useGravity = false;
                _dragMem = _heldObjectRigidbody.drag;
                _heldObjectRigidbody.drag = 10;
                _heldObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                _heldObject = grabbedObject;
            }
        }
    }

    void DropObject()
    {
        //TODO sound of drop
        sound.Stop();
        _heldObjectRigidbody.useGravity = true;
        _heldObjectRigidbody.drag = _dragMem;
        _heldObjectRigidbody.constraints = RigidbodyConstraints.None;
        _heldObject = null;
    }

    private IEnumerator DissolveObject(Transform obj, teleportGun.TeleportEventArgs e)
    {
        Dissolvable disScript = obj.GetComponent<Dissolvable>();
        _canGrab = false;
        if (disScript != null)
        {
            disScript.Dissolve();
        }
        yield return new WaitForSeconds(UnificControl.TimeToTeleport);
        if (_heldObject != null && _heldObject.transform == obj)
        {
            obj.position = e.PlayerPos + e.DVector;
        }

        if (disScript != null)
        {
            disScript.Undissolve();
        }

        _canGrab = true;

    }
    
    void CheckHeldObjectOnPlayerTeleport(object sender, EventArgs e)
    {
        if (_heldObject != null)
        {
            if (_heldObject.CompareTag("GrabbableNotTeleportable"))
            {
                DropObject();
            }
            else if (_heldObject.CompareTag("Grabbable"))
            {
                StartCoroutine(DissolveObject(_heldObject.transform, (teleportGun.TeleportEventArgs)e));
            }
        }
    }

    private void OnDestroy()
    {
        UnificControl.FButton -= GrabSomething;
        //TeleportPlatform.DoingPlayerTeleportStationar -= CheckHeldObjectOnPlayerTeleport;
        teleportGun.DoingPlayerTeleport -= CheckHeldObjectOnPlayerTeleport;
    }
}
