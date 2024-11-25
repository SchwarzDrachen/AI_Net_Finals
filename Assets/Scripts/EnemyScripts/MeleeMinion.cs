using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : MonoBehaviour
{
    [SerializeField]
    private Transform targetMarker; 
    [SerializeField]
    private float fireRateCountdown;
    [SerializeField]
    private GameObject agentObject;
    private NavMeshAgent agent;
    private SpriteRenderer agentSprite;
    private Rigidbody2D agentBody;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    private void Update()
    {
        if(targetMarker.position == null){
            Debug.Log("NO TARGET");
        }
        else{
            FlipSpriteOnDirection();
            agent.SetDestination(targetMarker.position);
        }
        
    }
    private void FlipSpriteOnDirection(){
        if(agentObject.transform.position.x > targetMarker.transform.position.x){
            agentObject.transform.localScale = new Vector2(-1,1);
        }
        else{
            agentObject.transform.localScale = new Vector2(1,1);
        }
    }
}
