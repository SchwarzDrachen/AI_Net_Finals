using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossBT : MonoBehaviour
{
    [SerializeField] private GameObject[] player;
    [SerializeField] private GameObject gunObjectPos;
    [SerializeField] private GameObject gun;
    //private Coroutine fireRateCooldown;
    private GameObject target;
    private NavMeshAgent agent;
    private HealthScript health;
    private Sequence rootNode;  
    private ActionNode an_isDead;
    private ActionNode an_randPlayer;
    private ActionNode an_rangeChoice;
    private ActionNode an_attack;

    private void Awake(){
        health = GetComponent<HealthScript>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start(){
        an_isDead = new ActionNode(HealthChecker);
        an_randPlayer = new ActionNode(RandomPlayer);
        an_rangeChoice = new ActionNode(RangeChoice);
        an_attack = new ActionNode(Attack);

        List<Node> childNodes = new();
        childNodes.Add(an_isDead);
        childNodes.Add(an_randPlayer);
        childNodes.Add(an_rangeChoice);
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
            return NodeState.FAILURE;
        }
    }
    private NodeState RandomPlayer(){
        int i = Random.Range(0,player.Length);
        player[i] = target;
        if(target == null){
            return NodeState.FAILURE;
        }
        else{
            agent.SetDestination(player[i].transform.position);
            return NodeState.SUCCESS;
        }
    }
    private NodeState RangeChoice(){
        int i = Random.Range(0,1);
        if(i == 1){
            agent.stoppingDistance = 15;
            return NodeState.SUCCESS;
        }
        else{
            agent.stoppingDistance = 0;
            return NodeState.SUCCESS;
        }
    }
    private NodeState Attack(){
        if(agent.stoppingDistance == 15){
            agent.speed = 3.5f;
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y,0f);
            Vector3 agentPos = new Vector3(transform.position.x,transform.position.y,0f);
            Vector3 gunAimPos = targetPos - agentPos;
            float aimAngle = Mathf.Atan2(gunAimPos.y, gunAimPos.x) * Mathf.Rad2Deg;        
            gunObjectPos.transform.rotation = Quaternion.AngleAxis(aimAngle,Vector3.forward);

            gun.GetComponent<GenericGunScript>().Shoot();
            RangeFireRate();
            gun.GetComponent<GenericGunScript>().Shoot();
            RangeFireRate();
            gun.GetComponent<GenericGunScript>().Shoot();

            return NodeState.SUCCESS;
        }
        else{
            agent.speed = 7f;

            return NodeState.SUCCESS;
        }
        
    }

    private IEnumerator RangeFireRate(){
        yield return new WaitForSeconds(1.0f);
    }
}
