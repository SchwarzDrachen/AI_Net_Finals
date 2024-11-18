using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic base script meant to be inherited by all guns as base script
public abstract class GenericGunScript : MonoBehaviour
{
    [SerializeField]protected GameObject Bullet;
    [SerializeField]protected GameObject BulletSpawnPosition;
    [SerializeField]protected float Damage;
    [SerializeField]protected float FireRate;
    [SerializeField]protected float Spread;

    public abstract void Shoot();    
}
