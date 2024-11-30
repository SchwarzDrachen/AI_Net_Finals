using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class SimpleBullet : GenericBulletScript
{
    private bool isDestroyed = false;
    public override void OnEnable(){
        currentBulletLifetime = bulletLifetime;
        isDestroyed = false;
    }
    public override void OnDisable(){
        owner = null;
    }
    protected override void moveBullet()
    {
        if (this.currentBulletLifetime > 0){            
            gameObject.transform.position += transform.right * (bulletSpeed/10);

            this.currentBulletLifetime -= Time.deltaTime;
        }
        else{
            DestroyOverNetwork();
        }
    }

    private void Update(){
        moveBullet();
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Enemy")){
            MeleeMinion enemyScr = collider.gameObject.GetComponent<MeleeMinion>();
            enemyScr.TakeDamage(bulletDamage);
            enemyScr.SetLastPlayerHit(owner);

        }
        else if(collider.gameObject.CompareTag("Boss")){
            BossBT enemyScr = collider.gameObject.GetComponent<BossBT>();
            enemyScr.TakeDamage(bulletDamage);
            enemyScr.SetLastPlayerHit(owner);

        }
        else if(collider.gameObject.CompareTag("Player")){
            PlayerControllerScript playerScr = collider.gameObject.GetComponent<PlayerControllerScript>();
            Debug.Log("taking damage");
            playerScr.TakeDamage(bulletDamage);            
            Debug.Log("DamageApplioed");
        }
        DestroyOverNetwork();
    }

    public void DestroyOverNetwork(){
        photonView.RPC("RPCDestroyOverNetwork", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void RPCDestroyOverNetwork()
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
}
