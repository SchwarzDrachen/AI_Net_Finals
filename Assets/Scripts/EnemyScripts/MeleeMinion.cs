using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : MonoBehaviour
{
    [SerializeField]private GameObject targetMarker; 
    
    [SerializeField]private GameObject agentObject;
    
    [SerializeField]private HealthScript health;
    [SerializeField]private GameObject playerList;
    private NavMeshAgent agent;
    
    private Rigidbody2D agentBody;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        health.ResetHealth();
        //must polish function
        
    }

    private void Update()
    {
        if(!health.isAlive()){
            Debug.Log("DEATH");
            Destroy(this.gameObject);
            return;
        }
        if(playerList.transform.childCount > 0){
            GetNearestPlayer();
        }
        

        if(agentObject.transform.position.z != 0){            
           agentObject.transform.position = new Vector3(agentObject.transform.position.x,agentObject.transform.position.y,0); 
        }

        if(targetMarker == null){
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

    private void GetNearestPlayer(){
        GameObject nearest = null;
        foreach(Transform player in playerList.transform){
            if(nearest == null){
                nearest = player.gameObject;
                return;
            }
            float nearestcompdist = Vector2.Distance(transform.position, nearest.transform.position);
            float currentcompdist = Vector2.Distance(transform.position, player.transform.position);
            if(currentcompdist< nearestcompdist){
                nearest = player.gameObject;
            }
        }
        targetMarker = nearest;

    }
    public void SetPlayerList(GameObject value){
        playerList = value;

    }
}
