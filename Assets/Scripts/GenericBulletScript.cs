using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;

//Generic script meant to be inherited by all bullets
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GenericBulletScript : MonoBehaviourPunCallbacks
{
    [SerializeField]protected float bulletDamage;
    [SerializeField]protected float bulletSpeed;    
    [SerializeField]protected float bulletLifetime; 
    protected float currentBulletLifetime;
    protected Player owner;

    protected abstract void moveBullet(); 
    public virtual void SetBulletDamage(float value){
        bulletDamage = value;
    }

    public virtual void SetOwner(Player ownerValue){
        owner = ownerValue;

    }
}
