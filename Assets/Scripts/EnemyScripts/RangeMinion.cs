using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeMinion : MonoBehaviour
{
    [SerializeField]
    private GameObject targetMarker; 
    [SerializeField]
    private float fireRateCountdown;
    [SerializeField]
    private GameObject agentObject;
    [SerializeField]
    private GameObject gunObjectPos;
    [SerializeField]
    private GameObject gun;
    private NavMeshAgent agent;
    private SpriteRenderer agentSprite;
    private Rigidbody2D agentBody;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agentSprite = GetComponent<SpriteRenderer>();
        agentBody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        targetMarker = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(fireRateCountdown > 0){
            fireRateCountdown -= Time.deltaTime;
        }

        if(targetMarker.transform.position == null){
            Debug.Log("NO TARGET");
        }
        else{
            agent.SetDestination(targetMarker.transform.position);
            FlipSpriteOnDirection();
            AimGun();
            //Debug.Log(agent.velocity);
            if(agent.velocity.magnitude <= 0.0f && fireRateCountdown <= 0){
            fireRateCountdown = 5;
            gun.GetComponent<GenericGunScript>().Shoot();
            }
        }
    }

    private void FlipSpriteOnDirection(){
        if(agentObject.transform.position.x > targetMarker.transform.position.x){
            agentObject.transform.localScale = new Vector2(-1,1);
            gunObjectPos.transform.localScale = new Vector2(-1,1);
        }
        else{
            agentObject.transform.localScale = new Vector2(1,1);
            gunObjectPos.transform.localScale = new Vector2(1,1);
        }
    }

    private void AimGun(){
        Vector3 targetPos = new Vector3(targetMarker.transform.position.x, targetMarker.transform.position.y,0f);
        Vector3 agentPos = new Vector3(agentObject.transform.position.x,agentObject.transform.position.y,0f);
        Vector3 gunAimPos = targetPos - agentPos;
        float aimAngle = Mathf.Atan2(gunAimPos.y, gunAimPos.x) * Mathf.Rad2Deg;        
        gunObjectPos.transform.rotation = Quaternion.AngleAxis(aimAngle,Vector3.forward);
    }
}
