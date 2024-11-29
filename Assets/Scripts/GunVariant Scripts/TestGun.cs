using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : GenericGunScript
{
    private void Update(){
        if(fireRateCountdown > 0){
            fireRateCountdown -= Time.deltaTime;
        }
    }
    public override void Shoot()
    {
        if(fireRateCountdown <= 0){
            Instantiate(Bullet,BulletSpawnPosition.transform.position, transform.parent.rotation);            
            fireRateCountdown = FireRate; 
        }
        
               
    }
    public override void AttachToParent()
    {
        throw new System.NotImplementedException();
    }
}
