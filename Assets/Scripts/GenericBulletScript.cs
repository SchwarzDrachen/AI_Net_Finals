using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Generic script meant to be inherited by all bullets
[RequireComponent(typeof(Rigidbody2D))]
public abstract class GenericBulletScript : MonoBehaviour
{
    [SerializeField]protected float bulletDamage;
    [SerializeField]protected float bulletSpeed;    
    [SerializeField]protected float bulletLifetime; 
    //[SerializeField]protected Player owner;

    protected abstract void moveBullet(); 
    public virtual void SetBulletDamage(float value){
        bulletDamage = value;
    }
}
