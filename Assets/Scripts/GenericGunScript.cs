using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;

//generic base script meant to be inherited by all guns as base script
public abstract class GenericGunScript : MonoBehaviourPunCallbacks
{
    [SerializeField]protected GameObject Bullet;
    [SerializeField]protected GameObject BulletSpawnPosition;
    [SerializeField]protected string GunName;
    [SerializeField]protected float Damage;
    [SerializeField]protected float FireRate;
    [SerializeField]protected float Spread;
    [SerializeField]protected string GunDescription;
    public GameObject GunParent;
    public Player GunOwner;

    protected float fireRateCountdown;
    protected bool isDestroyed = false;

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

    public virtual void DestroyOverNetwork(){
        photonView.RPC("RPCDestroyOverNetwork", RpcTarget.AllBuffered);
    }
    [PunRPC]
    protected virtual void RPCDestroyOverNetwork()
    {
        // Only the player that spawned the object can destroy it
        // Because the bullet is spawned by the player
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            isDestroyed = true;
        }
    }
    public abstract void AttachToParent();
    
}
