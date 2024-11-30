using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class MeleeMinion : MonoBehaviourPunCallbacks
{
    [SerializeField]private GameObject targetMarker; 
    
    [SerializeField]private GameObject agentObject;
    
    [SerializeField]private HealthScript health;
    [SerializeField]private GameObject playerList;
    [SerializeField]private float attackDamage;
    [SerializeField]private float attackCooldown;
    private float attackCurrentCooldown = 0;
    private NavMeshAgent agent;
    
    private Rigidbody2D agentBody;
    private Player LastPlayerHit;
    private bool isDestroyed = false;
    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;

        
    }
    public override void OnEnable(){
        isDestroyed = false;
        health.ResetHealth();
        //must polish function
    }

    private void Update()
    {
        if(!health.isAlive() && isDestroyed == false){
            GiveScoreToLastHit();
            DestroyOverNetwork();
            GameManagerScript.Instance.ModifyEnemyCount(-1);            
            return;
        }
        if(playerList.transform.childCount > 0){
            GetNearestPlayer();
        }

        if(targetMarker == null){
            //Debug.Log("NO TARGET!");
        }
        else{
            FlipSpriteOnDirection();
            agent.SetDestination(targetMarker.transform.position);
        }
        attackCurrentCooldown -= Time.deltaTime;
    }

    private void FlipSpriteOnDirection(){
        if(agentObject.transform.position.x > targetMarker.transform.position.x){
            agentObject.transform.localScale = new Vector3(-1,1,0);
        }
        else{
            agentObject.transform.localScale = new Vector3(1,1,0);
        }
    }

    private void GetNearestPlayer(){
        GameObject nearest = null;
        foreach(Transform player in playerList.transform){            
            if(player.gameObject.activeSelf == false){
                continue;
            }
            if(nearest == null){
                nearest = player.gameObject; 
                continue;
            }
            float nearestcompdist = Vector2.Distance(transform.position, nearest.transform.position);
            float currentcompdist = Vector2.Distance(transform.position, player.transform.position);            
            if(currentcompdist < nearestcompdist){
                nearest = player.gameObject;
            }
        }
        targetMarker = nearest;        

    }
    public void SetPlayerList(GameObject value){
        playerList = value;

    }

    public void TakeDamage(float damage){        
        health.takeDamage(damage);        
    }
    public void SetLastPlayerHit(Player playerVal){
        LastPlayerHit = playerVal;
    }

    public float GetDamage(){
        return attackDamage;
    }
    public virtual void DestroyOverNetwork(){
        photonView.RPC("RPCDestroyOverNetwork", RpcTarget.AllBuffered);
    }
    [PunRPC]
    protected virtual void RPCDestroyOverNetwork()
    {
        // Only the player that spawned the object can destroy it
        // Because the bullet is spawned by the player
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            isDestroyed = true;
        }
    }

    public void GiveScoreToLastHit(){
        GameManagerScript.Instance.AddScore(100,LastPlayerHit);
    }
    public void ResetAttackCooldown(){
        attackCurrentCooldown = attackCooldown;
    }
    public float GetAttackCooldown(){
        return attackCurrentCooldown;
    }
}
