using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class BossBT : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject gunObjectPos;
    [SerializeField]private GameObject currentGun;
    [SerializeField] private float fireRateCooldown;
    [SerializeField]private GameObject playerList;
    [SerializeField]private GameObject agentObject;
    [SerializeField]private float attackDamage;

    private GameObject target;
    private NavMeshAgent agent;
    [SerializeField] private HealthScript health;
    private Sequence rootNode;  
    private ActionNode an_isDead;
    private ActionNode an_randPlayer;
    private ActionNode an_attack;
    private Coroutine FIRE;
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
    }

    private void Start(){
        an_isDead = new ActionNode(HealthChecker);
        an_randPlayer = new ActionNode(TargetClose
);
        an_attack = new ActionNode(Attack);

        List<Node> childNodes = new();
        childNodes.Add(an_isDead);
        childNodes.Add(an_randPlayer);
        childNodes.Add(an_attack);

        rootNode = new Sequence(childNodes);
    }

    private void Update(){
        rootNode.Evaluate();
    }
    private NodeState HealthChecker(){
        if(health.isAlive()){
            return NodeState.SUCCESS;
        }
        else{
            GiveScoreToLastHit();
            DestroyOverNetwork();
            GameManagerScript.Instance.ModifyEnemyCount(-1);
            return NodeState.FAILURE;
        }
    }
    private NodeState TargetClose(){
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
        target = nearest;
        return NodeState.SUCCESS;    
    }
    private NodeState Attack(){
        agent.SetDestination(target.transform.position);

        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y,0f);
            Vector3 agentPos = new Vector3(transform.position.x,transform.position.y,0f);
            Vector3 gunAimPos = targetPos - agentPos;
            float aimAngle = Mathf.Atan2(gunAimPos.y, gunAimPos.x) * Mathf.Rad2Deg;        
            gunObjectPos.transform.rotation = Quaternion.AngleAxis(aimAngle,Vector3.forward);

            if(FIRE != null){
               return NodeState.FAILURE; 
            }
            FIRE = StartCoroutine(Shoot());

        return NodeState.SUCCESS;
    }

    private IEnumerator Shoot(){
        currentGun.GetComponent<GenericGunScript>().Shoot();
        yield return new WaitForSeconds(fireRateCooldown);
        FIRE = null;
    }

    private void FlipSpriteOnDirection(){
        if(agentObject.transform.position.x > target.transform.position.x){
            agentObject.transform.localScale = new Vector3(-1,1,0);
        }
        else{
            agentObject.transform.localScale = new Vector3(1,1,0);
        }
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
        GameManagerScript.Instance.AddScore(1000,LastPlayerHit);
    }
}