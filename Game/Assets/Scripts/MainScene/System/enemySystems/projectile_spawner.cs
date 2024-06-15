using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class projectile_spawner : MonoBehaviour, Usable
{
    [SerializeField] private Transform pfProjectile;

    [SerializeField] private float delay;
    
    public bool stopAutomatic = false;

    private Transform target;
    private float ticker;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        ticker += Time.deltaTime;

        if (!stopAutomatic && ticker > delay)
        {
            Shoot();
            ticker = 0;
        }
    }

    void Shoot()
    {
        if (GameManager.gameManager._playerHealth.alive())
        {
            var emitPosition = transform.position;
            Instantiate(pfProjectile, emitPosition, Quaternion.LookRotation(transform.forward));
        }
    }

    public int use(bool state = true)
    {
        Shoot();
        return 1;
    }
}