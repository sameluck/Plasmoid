using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_turret_rotating : MonoBehaviour
{
    [SerializeField] public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager._playerHealth.alive())
        {
            transform.LookAt(target);
        }
    }
}