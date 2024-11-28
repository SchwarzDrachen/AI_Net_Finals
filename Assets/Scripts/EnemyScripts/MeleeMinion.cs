using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : MonoBehaviour
{
    [SerializeField]private GameObject targetMarker; 
    [SerializeField]
    private GameObject agentObject;
    [SerializeField]
    private HealthScript health;
    private NavMeshAgent agent;
    
    private Rigidbody2D agentBody;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        //must polish function
        targetMarker = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void Update()
    {
        if(!health.isAlive()){
            Debug.Log("DEATH");
            Destroy(this.gameObject);
            return;
        }

        if(agentObject.transform.position.z != 0){            
           agentObject.transform.position = new Vector3(agentObject.transform.position.x,agentObject.transform.position.y,0); 
        }

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
            agentObject.transform.localScale = new Vector3(-1,1,0);
        }
        else{
            agentObject.transform.localScale = new Vector3(1,1,0);
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Wall"){
            Physics2D.IgnoreCollision(col.collider,GetComponent<Collider2D>());
        }
    }
}
