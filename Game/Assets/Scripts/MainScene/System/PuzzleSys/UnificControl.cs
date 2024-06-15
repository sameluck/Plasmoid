using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UnificControl : MonoBehaviour
{
    
    public static event EventHandler<RayShootEventArgs> LeftMouse;
    public static event EventHandler<EventArgs> RightMouse;
    public static event EventHandler<EventArgs> MidlMouse;
    public static event EventHandler<RayShootEventArgs> EButton;
    public static event EventHandler<RayShootEventArgs> FButton;
    public static event EventHandler<EventArgs> SpacebarButton;
    public static event EventHandler<MovementEventArgs> WasdButtons;
    public static event EventHandler<EventArgs> CButton;
    public static event EventHandler<EventArgs> EscButton;

    [SerializeField] private Transform uiControl;
    public static Transform UIControl;
    
    public static GameObject Cam;
    public static GameObject Player;
    
    private Transform _transformPlayer;
    
    private static int _classicHologramIgnoreMask = -5;
    private static int _classicReachIgnoreMask = -5;

    [SerializeField] private float setTimeToTeleport  = 1f;
    public static float TimeToTeleport;
    [SerializeField] private uint setMaxNumOfHolograms = 3;
    public static uint MaxNumOfHolograms;
    
    [SerializeField] private bool allowShield = false;
    public static bool AllowShield;

    [SerializeField] private TextMeshProUGUI holNumText;
    public static TextMeshProUGUI HolNumText;
    
    [SerializeField] private TextMeshProUGUI maxHolNum;
    public static TextMeshProUGUI MaxHolNum;
    
    [SerializeField] private TextMeshProUGUI holNumCur;
    public static TextMeshProUGUI HolNumCur;

    [SerializeField] private float maxDistanceToUse = 2f;
    
    public static int GetClassicHologramIgnoreMask()
    {
        return _classicHologramIgnoreMask;
    }
    
    public static int GetClassicReachIgnoreMask()
    {
        return _classicReachIgnoreMask;
    }
    
    private void Awake()
    {
        SpacebarButton = null;
        WasdButtons = null;
        CButton = null;
        
        UIControl = uiControl;

        Cam = GameObject.FindWithTag("MainCamera");
        Player = GameObject.FindWithTag("Player");
        _transformPlayer = Player.transform;

        TimeToTeleport = setTimeToTeleport;
        MaxNumOfHolograms = setMaxNumOfHolograms;
        AllowShield = allowShield;
        
        //
        _classicHologramIgnoreMask &= ~(1 << 6);
        _classicHologramIgnoreMask &= ~(1 << 5);
        _classicHologramIgnoreMask &= ~(1 << 7);
        _classicHologramIgnoreMask &= ~(1 << 8);
        //
        _classicReachIgnoreMask &= ~(1 << 5);
        _classicReachIgnoreMask &= ~(1 << 8);
        //

        HolNumText = holNumText;
        MaxHolNum = maxHolNum;
        HolNumCur = holNumCur;
        
        MaxHolNum.SetText(MaxNumOfHolograms.ToString());
        hologramControl.HologramDestroyed += UpdateCurrentNumOfHologram;
        EButton += UseSomething;
    }

    public static void UpdateCurrentNumOfHologram(object sender = null, EventArgs e = null)
    {
        HolNumCur.SetText(teleportGun.HologramsList.Count.ToString());
    }

    public class MovementEventArgs : EventArgs
    {
        public float HorizontalInput;
        public float VerticalInput;

        public MovementEventArgs(float horizontalInput, float verticalInput)
        {
            this.HorizontalInput = horizontalInput;
            this.VerticalInput = verticalInput;
        }
    }
    
    public class RayShootEventArgs : EventArgs
    {
        public RayShootEventArgs(Vector3 startPos, Vector3 hitPos)
        {
            this.StartPos = startPos;
            this.HitPos = hitPos;
            this.IsHit = true;
        }
        
        public RayShootEventArgs(Vector3 startPos, bool isHit)
        {
            this.StartPos = startPos;
            this.IsHit = isHit;
        }

        public Vector3 StartPos;
        public bool IsHit;
        public Vector3 HitPos;
    }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    private void UseSomething(object sender, UnificControl.RayShootEventArgs e)
    {
        Vector3 dirToUse = e.HitPos - _transformPlayer.position;
        RaycastHit hit;
        if (Physics.Raycast(_transformPlayer.position, dirToUse, out hit ,maxDistanceToUse, _classicReachIgnoreMask))
        {
            E_Usable raycastObj = hit.collider.gameObject.GetComponent<E_Usable>();
            //if (hit.collider.CompareTag("Button"))
            if(raycastObj != null)
            {
                raycastObj.use();
                Debug.Log("Tried to use button");
            }
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    
    void Update()
    {
        //Invoke directional input event
        if (WasdButtons != null)
        { 
            WasdButtons.Invoke(this, new MovementEventArgs(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }
        
        //invoke shooting event
        if (Input.GetKeyDown("mouse 0"))
        {
            if (LeftMouse != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, float.PositiveInfinity, _classicHologramIgnoreMask))
                {
                    LeftMouse.Invoke(this, new RayShootEventArgs(Cam.transform.position, hit.point));
                }
            }
        }
        
        if (Input.GetKeyDown("mouse 1"))
        {
            if (RightMouse != null)
            {
                RightMouse.Invoke(this, EventArgs.Empty);
            }
        }

        if (Input.GetKeyDown("mouse 2"))
        {
            if (MidlMouse != null)
            {
                MidlMouse.Invoke(this, EventArgs.Empty);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (EButton != null)
            {
                RaycastHit hitE;
                if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hitE, float.PositiveInfinity, _classicHologramIgnoreMask))
                {
                    EButton.Invoke(this, new RayShootEventArgs(Cam.transform.position, hitE.point));
                }
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FButton != null)
            {
                RaycastHit hitF;
                if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hitF, float.PositiveInfinity, _classicHologramIgnoreMask))
                {
                    FButton.Invoke(this, new RayShootEventArgs(Cam.transform.position, hitF.point));
                }
                else
                {
                    FButton.Invoke(this, new RayShootEventArgs(Cam.transform.position, false));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SpacebarButton != null)
            {
                SpacebarButton.Invoke(this, EventArgs.Empty);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscButton != null)
            {
                
                EscButton.Invoke(this, EventArgs.Empty);
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (WasdButtons != null)
            {
                WasdButtons.Invoke(this, new MovementEventArgs(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
        }
        
        if (Input.GetKey(KeyCode.C))
        {
            if (CButton != null)
            {
                CButton.Invoke(this, EventArgs.Empty);
            }
        }
        
    }

    private void OnDestroy()
    {
        hologramControl.HologramDestroyed -= UpdateCurrentNumOfHologram;
        EButton -= UseSomething;
    }
}
