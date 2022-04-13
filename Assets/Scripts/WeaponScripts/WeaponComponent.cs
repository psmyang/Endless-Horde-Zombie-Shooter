using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None, Pistol, MachineGun
}

public enum WeaponFiringPattern
{
    SemiAuto, FullAuto, ThreeShotBurst, FiveShotBurst, PumpAction
}

[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public WeaponFiringPattern firingPattern;
    public string weaponName;
    public float damage;
    public int bulletsInClip;
    public int clipSize;
    public float fireStartDelay;
    public float fireRate;
    public WeaponFiringPattern weaponFiringPattern;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayers;
    public int totalBullets;

}

public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public Transform firingEffectLocation;

    protected WeaponHolder weaponHolder;
    [SerializeField]
    protected ParticleSystem firingEffect;

    [SerializeField]
    public WeaponStats weaponStats;

    public bool isFiring = false;
    public bool isReloading = false;
    protected Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        //firingEffect.gameObject.transform.parent = firingEffectLocation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;

    }

    //decide whether it is automatic or semi-automatic here
    public void Initialize(WeaponHolder _weaponHolder, WeaponScriptable weaponScriptable)
    {
        weaponHolder = _weaponHolder;

        if (weaponScriptable)
        {
            weaponStats = weaponScriptable.weaponStats;
        }
    }

    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if (weaponStats.repeating)
        {
            //fire weapon
            CancelInvoke(nameof(FireWeapon));
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));
        if (firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
    }

    protected virtual void FireWeapon()
    {
        //print("Firing weapon!");
        weaponStats.bulletsInClip--;
    }
    //deal with ammo counts and maybe particle effects
    public virtual void StartReloading()
    {
        isReloading = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        isReloading = false;
    }

    protected virtual void ReloadWeapon()
    {
        //if theres a firing effect hide it here
        if (firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
        // if there's a firing effect, hide it here

        int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;

        if (bulletsToReload < 0)
        {
            weaponStats.totalBullets -= (weaponStats.clipSize - weaponStats.bulletsInClip);
            weaponStats.bulletsInClip = weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }


    }
}
