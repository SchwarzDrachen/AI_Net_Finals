using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
public class SimpleGunVariant : GenericGunScript
{
     private const string BULLET_PREFAB_NAME = "Bullet"; 
    private void Update(){
        if(GunParent != null){
            AttachToParent();
        }
        if(fireRateCountdown > 0){
            fireRateCountdown -= Time.deltaTime;
        }
    }
    public override void Shoot()
    {
        if (!photonView.IsMine) return;
        if(fireRateCountdown <= 0){
            var bullet = PhotonNetwork.Instantiate(BULLET_PREFAB_NAME,BulletSpawnPosition.transform.position, gameObject.transform.rotation);  
            GenericBulletScript bulScr = bullet.GetComponent<GenericBulletScript>();
            bulScr.SetBulletDamage(Get_GunDamage());       
            bulScr.SetOwner(GunOwner);
            fireRateCountdown = FireRate; 
        }                       
    }
    public override void AttachToParent()
    {
        gameObject.transform.position = GunParent.transform.position;
        gameObject.transform.rotation = GunParent.transform.rotation;
    }
}
