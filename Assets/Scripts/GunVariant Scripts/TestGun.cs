using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : GenericGunScript
{
    public override void Shoot()
    {
        Instantiate(Bullet,BulletSpawnPosition.transform.position, transform.parent.rotation);
        Debug.Log("Firing");        
    }
}
