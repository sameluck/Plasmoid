using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boolet : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float speed;

    private int _lifetime = 20000;

    private Rigidbody _boolet;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        _boolet = GetComponent<Rigidbody>();
        if (GameManager.gameManager._playerHealth.alive())
        {
            transform.LookAt(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _boolet.velocity = (transform.forward * speed);
        if (--_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Hologram"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);

            foreach (Collider nearbyObject in colliders)
            {
                playerController pc = nearbyObject.GetComponent<playerController>();
                if (pc != null && !pc.shield)
                {
                    pc.TakeDamage(1);
                    Debug.Log(GameManager.gameManager._playerHealth.Health);
                }
            }

            Debug.Log("Kaboom");

            Destroy(gameObject);
        }

        
    }
}