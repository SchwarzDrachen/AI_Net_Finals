using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : GenericBulletScript
{
    private float lifetime;
    private Rigidbody2D rb;
    private void Awake(){
        //initialize values
        rb = GetComponent<Rigidbody2D>();
        this.lifetime = 1;
    }
    protected override void moveBullet()
    {
        if (this.lifetime > 0){            
            rb.velocity = transform.right * bulletSpeed;

            this.lifetime -= Time.deltaTime;
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
            collider.gameObject.GetComponent<HealthScript>().takeDamage(10);
        }
        Destroy(this.gameObject);
    }
}
