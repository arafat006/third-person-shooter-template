using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]float rateOfFire;
    [SerializeField]Projectile projectile;


    [HideInInspector]
    public Transform muzzle;

    private WeaponReloader reloader;

    float nextFireAllowed;
    public bool canFire;

    void Awake()
    {
        muzzle = transform.Find("Muzzle");
        reloader = GetComponent<WeaponReloader>();
    }

    public void Reload(){
        if (reloader == null)
            return;

        reloader.Reload();
    }

    public virtual void Fire()
    {

        canFire = false;

        if(Time.time < nextFireAllowed)
        {
            return;
        }

        if (reloader != null)
        {
            if (reloader.IsReloading)
            {
                return;
            }
            if (reloader.RoundsRemainingInClip == 0)
            {
                return;
            }

            reloader.TakeFromClip(1);
        }

        nextFireAllowed = Time.time + rateOfFire;

        //Instantiate the projectile
        Instantiate(projectile, muzzle.position, muzzle.rotation);
        
        //print("Firing : " + Time.time);
        canFire = true;
    }



}
