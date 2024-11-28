using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossBT : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gunObjectPos;
    [SerializeField] private GameObject gun;
    [SerializeField] private float fireRateCooldown;
    [SerializeField] private float fireRateBreak;

    private GameObject target;
    private NavMeshAgent agent;
    private HealthScript health;
    private Sequence rootNode;  
    private ActionNode an_isDead;
    private ActionNode an_randPlayer;
    //private ActionNode an_rangeChoice;
    private ActionNode an_attack;
    private Coroutine FIRE;

    private void Awake(){
        health = GetComponent<HealthScript>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
		agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start(){
        an_isDead = new ActionNode(HealthChecker);
        an_randPlayer = new ActionNode(RandomPlayer);
        //an_rangeChoice = new ActionNode(RangeChoice);
        an_attack = new ActionNode(Attack);

        List<Node> childNodes = new();
        childNodes.Add(an_isDead);
        childNodes.Add(an_randPlayer);
        //childNodes.Add(an_rangeChoice);
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
        int i = Random.Range(0,0);
        //player[i] = target;
        target = player;
        if(target == null){
            return NodeState.FAILURE;
        }
        else{
            agent.SetDestination(target.transform.position);
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
            agent.speed = 5f;
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
        gun.GetComponent<GenericGunScript>().Shoot();
        yield return new WaitForSeconds(fireRateCooldown);
        FIRE = null;
    }
}
