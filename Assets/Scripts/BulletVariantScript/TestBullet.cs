using System.Collections;
using System.Collections.Generic;
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
            rb.velocity = transform.up * bulletSpeed;

            this.lifetime -= Time.deltaTime;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Update(){
        moveBullet();
        Debug.Log(lifetime);
    }
}
