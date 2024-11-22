using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic base script meant to be inherited by all guns as base script
public abstract class GenericGunScript : MonoBehaviour
{
    [SerializeField]protected GameObject Bullet;
    [SerializeField]protected GameObject BulletSpawnPosition;
    [SerializeField]protected string GunName;
    [SerializeField]protected float Damage;
    [SerializeField]protected float FireRate;
    [SerializeField]protected float Spread;
    [SerializeField]protected string GunDescription;

    protected float fireRateCountdown;

    public abstract void Shoot(); 

    public virtual string Get_GunName(){
        return GunName;
    } 
    public virtual float Get_GunDamage(){
        return Damage;
    }   
    public virtual float Get_GunFireRate(){
        return FireRate;
    }
    public virtual float Get_GunSpread(){
        return Spread;
    }  
    public virtual string Get_GunDesc(){
        return GunDescription;
    } 
    
}
