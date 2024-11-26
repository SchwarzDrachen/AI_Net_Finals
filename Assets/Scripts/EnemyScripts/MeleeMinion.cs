using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : MonoBehaviour
{
    private GameObject targetMarker; 
    [SerializeField]
    private GameObject agentObject;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float curHealth;
    private NavMeshAgent agent;
    private SpriteRenderer agentSprite;
    private Rigidbody2D agentBody;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        targetMarker = GameObject.FindGameObjectWithTag("Player");
        curHealth = maxHealth;
    }

    private void Update()
    {
        if(targetMarker.transform.position == null){
            Debug.Log("NO TARGET!");
        }
        else{
            FlipSpriteOnDirection();
            agent.SetDestination(targetMarker.transform.position);
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

    private void OnCollisionEnter2D(Collision collider){
        if(collider.tag == "bullet"){
            curHealth -= 50;
        }
    }
}
