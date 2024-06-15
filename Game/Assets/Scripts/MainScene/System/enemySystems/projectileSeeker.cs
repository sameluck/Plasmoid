using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class projectileSeeker : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] public ParticleSystem particle;

    [SerializeField] public float force;
    [SerializeField] public float kaboomForce;
    [SerializeField] public float kaboomRadius;

    private Rigidbody _icbm;


    private Transform _transform1;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        _icbm = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target);

        _icbm.AddForce(transform.forward * (force));
        force *= 1 + 0.1f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {

        _transform1 = transform;

        if (!collision.gameObject.CompareTag("Hologram"))
        {

            Collider[] colliders = Physics.OverlapSphere(_transform1.position, kaboomRadius);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    var position = _transform1.position;
                    rb.AddExplosionForce(kaboomForce * 2, position, kaboomRadius / 2);
                    rb.AddExplosionForce(kaboomForce, position, kaboomRadius );
                }
            }

            colliders = Physics.OverlapSphere(transform.position, kaboomRadius / 2);

            foreach (Collider nearbyObject in colliders)
            {
                if (nearbyObject.gameObject.CompareTag("Hologram"))
                {
                    Destroy(Utilits.FindParent(nearbyObject.transform).gameObject);
                    continue;
                }

                playerController pc = Utilits.FindParentWithRigedbody(nearbyObject.transform).GetComponent<playerController>();
                if (pc != null && !pc.shield)
                {
                    Debug.Log(nearbyObject.name);
                    pc.TakeDamage(3);
                    Debug.Log(GameManager.gameManager._playerHealth.Health);
                }
            }

            Debug.Log("Kaboom");
            
            Instantiate(particle, _transform1.position, _transform1.rotation);

            Destroy(gameObject);
        }
        
    }
}