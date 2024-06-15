using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    /* 
    Create a variable called 'rb' that will represent the 
    rigid body of this object.
    */
    private Rigidbody rb;

    //Checking if grounded
    public float playerHeight;
    public LayerMask groundMask;
    public LayerMask additionalGroundMask;
    private bool onGround;

    [SerializeField] private GameObject _replacement;

    private bool shieldActivationBlocker;
    [SerializeField] private float shieldRechargeTimer;
    [SerializeField] private float shieldTimer;
    [SerializeField] private Transform shieldTransform;
    
    [SerializeField] private AudioSource idleShieldSound;
    [SerializeField] private AudioSource nuh_uh_Sound;
    
    //jumping
    public bool readyToJump;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    public Transform playerBody;
    public Transform orientation;

    public float speed;
    public float rotationSpeed;
    public float groundDrag;

    private float horizontalMovement;
    private float verticalMovement;

    public bool shield = false;
    private Renderer _shieldRenderer;
    
    private int finalGroundMask;
    
    void Start()
    {
        // make our rb variable equal the rigid body component
        rb = GetComponent<Rigidbody>();
        readyToJump = true;

        rb.sleepThreshold = 0;
        UnificControl.WasdButtons += MovePlayer;
        UnificControl.SpacebarButton += jumpPlayer;
        UnificControl.CButton += ActivateShield;
    }
    
    private void Awake()
    {
        finalGroundMask = groundMask;
        
        finalGroundMask |= additionalGroundMask;
        _shieldRenderer = shieldTransform.GetComponent<Renderer>();
    }

    void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, finalGroundMask);

        if (onGround)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void MovePlayer(object sender, UnificControl.MovementEventArgs e)
    {
        horizontalMovement = e.HorizontalInput;
        verticalMovement = e.VerticalInput;
    }

	void FixedUpdate()
	{
		Vector3 inputDir = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        playerBody.forward = Vector3.Slerp(playerBody.forward, inputDir.normalized, Time.fixedDeltaTime * rotationSpeed);
		if(verticalMovement != 0 || horizontalMovement != 0)
		{
			if (onGround)
     		   rb.AddForce(playerBody.forward * speed * 10, ForceMode.Force);
     		else
     		   rb.AddForce(playerBody.forward * (speed * airMultiplier) * 10, ForceMode.Force);		
		}
	}

    void jumpPlayer(object sender, EventArgs e)
    {
        if (onGround && readyToJump)
        {
            readyToJump = false;
            //reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void TakeDamage(int dmg)
    {
        GameManager.gameManager._playerHealth.TakeDamage(dmg);

        if (!GameManager.gameManager._playerHealth.alive())
        {
            Debug.Log("Game over");
            DiesViolently();
        }
    }

    public void HealDamage(int heal)
    {
        GameManager.gameManager._playerHealth.HealDamage(heal);
    }

    private void DiesViolently()
    {
        var replacement = Instantiate(_replacement, transform.position, transform.rotation);

        var rbs = replacement.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(10f, transform.position, 20);
        }
        Destroy(gameObject);
    }

    
    private void DisableShield()
    {
        //TODO sound of disabled shield
        idleShieldSound.Stop();
        _shieldRenderer.enabled = false;
        shield = false;
        Invoke(nameof(DisableShieldActivationBlocker), shieldRechargeTimer);
    }
    
    private void EnableShield()
    {
        //TODO sound of enable shield
        idleShieldSound.Play();
        _shieldRenderer.enabled = true;
        shield = true;
        shieldActivationBlocker = true;
        Invoke(nameof(DisableShield), shieldTimer);
    }

    private void DisableShieldActivationBlocker()
    {
        //TODO sound that shield can be activated
        shieldActivationBlocker = false;
    }

    private void ActivateShield(object sender, EventArgs e)
    {
        if (!shieldActivationBlocker && UnificControl.AllowShield)
        {
            EnableShield();
        }
        else
        {
            nuh_uh_Sound.Play();
            //TODO sound of not use
        }
    }

}