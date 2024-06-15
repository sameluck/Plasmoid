using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class teleportGun : MonoBehaviour
{
    public static event EventHandler<TeleportEventArgs> DoingPlayerTeleport;
    
    public class TeleportEventArgs : EventArgs
    {
        public Vector3 DVector;
        public Vector3 PlayerPos;
        public TeleportEventArgs(Vector3 dVector, Vector3 playerPos)
        {
            this.PlayerPos = playerPos;
            this.DVector = dVector;
        }
    }
    
    [SerializeField] private Transform pfProjectile;
    [SerializeField] private Transform localDissolver;
    [SerializeField] private float timeToCooldown = 0.1f;

    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private AudioSource teleportSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource nuh_uh_Sound;
    [SerializeField] private AudioSource electricExplosion;
    
    private Dissolvable _disScript;
    private Transform _playerBody;
    public static LinkedList<GameObject> HologramsList = new LinkedList<GameObject>();
    private int _currentHologramNum = -1;
    private bool _blockPersonalTeleport;
    private bool _blockShoot;
    
    public int GetCurrentHologramNum()
    {
        return _currentHologramNum;
    }

    public static void PutHologram(GameObject hologram)
    {
        if (HologramsList.Count >= UnificControl.MaxNumOfHolograms)
        {
            GameObject go = HologramsList.First.Value;
            HologramsList.RemoveFirst();
            Destroy(go);
        }
        HologramsList.AddLast(hologram);
        UnificControl.UpdateCurrentNumOfHologram();
    }
    
    private void Start()
    {
        HologramsList.Clear();
        _playerBody = UnificControl.Player.transform.GetChild(1);
        _blockPersonalTeleport = false;
        _blockShoot = false;
        
        UnificControl.LeftMouse += PlayerShootTagProjectile;
        UnificControl.MidlMouse += PlayerChangeHologramNum;
        UnificControl.RightMouse += PlayerTeleportToHologram;
        
        UnificControl.HolNumText.SetText((GetCurrentHologramNum()+1).ToString());
        
        UnificControl.HolNumCur.SetText(HologramsList.Count.ToString());
        if (localDissolver != null)
        {
            _disScript = localDissolver.GetComponent<Dissolvable>();
        }

    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        _blockShoot = false;
    }

    private void PlayerShootTagProjectile(object sender, UnificControl.RayShootEventArgs e)
    {
        if (!_blockShoot)
        {
            _blockShoot = true;
            StartCoroutine(Cooldown(timeToCooldown));
            
            var emitPosition = transform.position;
        
            if (Utilits.CheckForward(transform.parent.parent.forward, e.HitPos - emitPosition))
            {
                return;
            }
            shootingSound.Play();
            //TODO sound of shoot
            Vector3 shootDir = (e.HitPos - emitPosition).normalized;
            Transform projTransf = Instantiate(pfProjectile, emitPosition, Quaternion.LookRotation(shootDir, Vector3.up));
            projTransf.GetComponent<TeleportTag>().Setup(shootDir);
        }
    }

    private void PlayerChangeHologramNum(object sender, EventArgs e)
    {
        if (HologramsList.Count != 0)
        {
            _currentHologramNum = (_currentHologramNum + 1) % HologramsList.Count;
            _playerBody.LookAt(HologramsList.ElementAt(_currentHologramNum).transform.GetChild(0).position);
        }
        else
        {
            _currentHologramNum = -1;
        }
        clickSound.Play();
        UnificControl.HolNumText.SetText((GetCurrentHologramNum()+1).ToString());
    }

    
    private void PlayerTeleportToHologram(object sender, EventArgs e)
    {
        
        if (HologramsList.Count > 0)
        {
            if (_currentHologramNum >= HologramsList.Count)
            {
                _currentHologramNum = HologramsList.Count - 1;
            }

            if (_currentHologramNum > -1)
            {
                if (!_blockPersonalTeleport)
                {
                    StartCoroutine(TransferPlayer());
                }
                else
                {
                    nuh_uh_Sound.Play();
                }
            }
            else
            {
                nuh_uh_Sound.Play();
            }

        }
        else
        {
            nuh_uh_Sound.Play();
            _currentHologramNum = -1;
        }

        UnificControl.HolNumText.SetText((GetCurrentHologramNum()+1).ToString());
    }

    private IEnumerator TransferPlayer()
    {
        teleportSound.Play();
        GameObject hologram = HologramsList.ElementAt(_currentHologramNum);
        Vector3 dVector = hologram.transform.position - _playerBody.parent.transform.position;
        Vector3 playerPos = _playerBody.parent.transform.position;
        
        if (DoingPlayerTeleport != null)
        {
            DoingPlayerTeleport.Invoke(this, new TeleportEventArgs(dVector, playerPos));
        }
        
        _blockPersonalTeleport = true;
        
        if (_disScript != null)
        {
            _disScript.Dissolve();
        }
        
        yield return new WaitForSeconds(UnificControl.TimeToTeleport);
        
        TeleportPlayerToHologram(hologram, dVector, playerPos);
        electricExplosion.Play();
        if (_disScript != null)
        {
            _disScript.Undissolve();
        }
        
        yield return new WaitForSeconds(UnificControl.TimeToTeleport);
        
        _blockPersonalTeleport = false;
    }
    
    private void TeleportPlayerToHologram(GameObject hologram, Vector3 dVector, Vector3 playerPos)
    {
        if (_playerBody != null)
        {
            _playerBody.parent.transform.position = playerPos + dVector;
            HologramsList.Remove(hologram);
            Destroy(hologram);
            UnificControl.HolNumText.SetText(GetCurrentHologramNum().ToString());
            _currentHologramNum--;            
        }
    }

    private void OnDestroy()
    {
        UnificControl.LeftMouse -= PlayerShootTagProjectile;
        UnificControl.MidlMouse -= PlayerChangeHologramNum;
        UnificControl.RightMouse -= PlayerTeleportToHologram;
    }
}
