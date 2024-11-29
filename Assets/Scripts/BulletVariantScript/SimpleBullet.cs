using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleBullet : GenericBulletScript
{
    private void Awake(){
        //initialize values
        this.bulletLifetime = 1;
    }
    protected override void moveBullet()
    {
        if (this.bulletLifetime > 0){            
            gameObject.transform.position += transform.right * (bulletSpeed/10);

            this.bulletLifetime -= Time.deltaTime;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Update(){
        moveBullet();
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Enemy")){
            collider.gameObject.GetComponent<HealthScript>().takeDamage((int)bulletDamage);
        }
        Destroy(this.gameObject);
    }
}
