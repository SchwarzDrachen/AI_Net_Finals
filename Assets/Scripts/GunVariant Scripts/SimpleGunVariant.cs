using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGunVariant : GenericGunScript
{
    private void Update(){
        if(fireRateCountdown > 0){
            fireRateCountdown -= Time.deltaTime;
        }
    }
    public override void Shoot()
    {
        if(fireRateCountdown <= 0){
            GameObject bullet = Instantiate(Bullet,BulletSpawnPosition.transform.position, transform.parent.rotation);  
            Debug.Log(Get_GunDamage());
            bullet.GetComponent<GenericBulletScript>().SetBulletDamage(Get_GunDamage());            
            fireRateCountdown = FireRate; 
        }
        
               
    }
}
