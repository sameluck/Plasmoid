using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatform : MonoBehaviour, Usable
{
    private LinkedList<Transform> _objectsInZone;
    public static event EventHandler<EventArgs> DoingPlayerTeleportStationar;
    [SerializeField] Transform endPoint;
    [SerializeField] private AudioSource sound;
    private void Awake()
    {
        _objectsInZone = new LinkedList<Transform>();
    }

    public int use(bool state)
    {
        if (state)
        {
            if (endPoint == null)
            {
                return 0;
            }
            sound.Play();
            Vector3 dVector = endPoint.position - transform.position;
            while (_objectsInZone.Count > 0)
            {
                Transform item = _objectsInZone.First.Value;
                Debug.Log(item);
                if (item != null)
                {
                    StartCoroutine(TransferObject(item, dVector, item.position));
                }
                _objectsInZone.RemoveFirst();
            }
            return 1;
        }
        
        return -1;
    }

    private IEnumerator TransferObject(Transform obj, Vector3 dVector, Vector3 objPos)
    {
        if (obj.CompareTag("Player"))
        {
            if (DoingPlayerTeleportStationar != null)
            {
                DoingPlayerTeleportStationar.Invoke(this, EventArgs.Empty);
            }
        }

        Dissolvable disScript = obj.GetComponent<Dissolvable>();
        Debug.Log(disScript);
        if (disScript != null)
        {
            disScript.Dissolve();
        }
        yield return new WaitForSeconds(UnificControl.TimeToTeleport);
        if (obj != null)
        {
            //obj.position += dVector; //to use teleport feature
            obj.position = objPos + dVector;
        }

        if (disScript != null)
        {
            disScript.Undissolve();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Transform otherParent = Utilits.FindParentWithRigedbody(other.transform);
        if (otherParent.CompareTag("GrabbableNotTeleportable"))
        {
            return;
        }

        if (!_objectsInZone.Contains(otherParent))
        {
            _objectsInZone.AddFirst(otherParent);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Transform otherParent = Utilits.FindParentWithRigedbody(other.transform);
        if (_objectsInZone.Contains(otherParent))
        {
            _objectsInZone.Remove(otherParent);
        }
        
    }
}
