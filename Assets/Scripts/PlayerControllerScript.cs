using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerControllerScript : MonoBehaviourPunCallbacks
{
    [SerializeField]private float maxMovementSpeed;
    [SerializeField]private bool canMove = false;    
    [SerializeField]private GameObject gunHolder;
    [SerializeField]private GameObject playerSprite;
    [SerializeField]private GameObject currentGun;
    [SerializeField]private float interactDistance;  
    [SerializeField]private HealthScript healthScr;    
    [SerializeField]private GameObject InteractionDisplay;
    [SerializeField]private GameObject GameOverPanel;    
    //for ease in use referencing UI elements
    [SerializeField]private GameObjectReferences GORScript;
    [SerializeField]private GameObject WeaponsPool;
    private bool isDestroyed = false;
    private Player Owner;

    private Vector3 currentSpeed;
    private float timeCounter;
    RaycastHit2D raycast;
    Vector2 mouseWorldPos;
    private void Update(){
        if (!photonView.IsMine) return;
        if(healthScr.isAlive() == false){
            DoPlayerDeath();
        }        
        if(canMove){
            MovementScript();
            CameraFollowPlayer();
            FlipSpriteOnDirection();
            AimGun();
            Interact();
            //checks if left mousebutton is held down to shoot
            if(Input.GetMouseButton(0)){
                //main gun shoot function is within the gun script itself
                currentGun.GetComponent<GenericGunScript>().Shoot();
            }                
        }
        
    }    
    public Player GetOwner(){
        return Owner;
    }
    void Start(){
        healthScr.ResetHealth(); 
    }
    public override void OnEnable(){
        if (!photonView.IsMine) return;
        healthScr.ResetHealth(); 
        if(InteractionDisplay != null){
            InteractionDisplay.SetActive(false);
        }
        Owner = photonView.Owner;
             
        Debug.Log("initializing Player Values"); 
        EquipGun("Pistol");        
        if (!photonView.IsMine) return;
        if(GORScript != null){
            ReloadObjectReferences();
        }
    }

    private void MovementScript(){
        //gets input if up/down or left/right
        //uses GetAxisRaw for more snappy input, no lerping from 0 to 1 values
        float movementInputX = Input.GetAxisRaw("Horizontal");
        float movementInputY = Input.GetAxisRaw("Vertical");
        currentSpeed.x = movementInputX * Time.deltaTime * maxMovementSpeed;
        currentSpeed.y = movementInputY * Time.deltaTime * maxMovementSpeed;        
        transform.position += currentSpeed;  
        //using y in the z parameter allows for auto application of sprite layering based on position
        transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.y);        
    }

    private void FlipSpriteOnDirection(){
        //checks if input is a non zero then sets 1 or -1 to flip object
        if(Input.GetAxisRaw("Horizontal") != 0){
            playerSprite.transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"),1,1);
        }        
    }

    private void AimGun(){  
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);      
        Vector2 InteractOrigin = new Vector2(gunHolder.transform.position.x,gunHolder.transform.position.y);  
        Vector3 gunAimPos = mouseWorldPos - InteractOrigin;
        float aimAngle = Mathf.Atan2(gunAimPos.y, gunAimPos.x) * Mathf.Rad2Deg;        
        gunHolder.transform.rotation = Quaternion.AngleAxis(aimAngle,Vector3.forward);

    }

    public void EquipGun(string gun){    
        photonView.RPC("RPCEquipGun", RpcTarget.AllBuffered,gun);       
    }
    [PunRPC]
    private void RPCEquipGun(string gun){  
        if(!photonView.IsMine)return;
        //removes current gun to be ready for requipping new gun
        if(currentGun!=null){
            GenericGunScript cg = currentGun.GetComponent<GenericGunScript>();
            cg.GunParent = null;
            cg.DestroyOverNetwork();
            currentGun = null;
        }
        
        GameObject newGun = PhotonNetwork.Instantiate(gun,gunHolder.transform.position, Quaternion.Euler(0,0,0));
        Debug.Log("equipping new gun");
        newGun.GetComponent<GenericGunScript>().GunParent = gunHolder;
        newGun.GetComponent<GenericGunScript>().GunOwner = Owner;
        currentGun = newGun;                                   
    }

    private void Interact(){        
        Vector2 InteractOrigin = new Vector2(gunHolder.transform.position.x,gunHolder.transform.position.y);
        Vector2 mousedir = mouseWorldPos - InteractOrigin;
        raycast = Physics2D.Raycast(InteractOrigin, mousedir,interactDistance);
        //gets collider that raycast hit        
        Collider2D raycastColliderHit = raycast.collider;

        //ends script if no collider is hit and hides interact notifier
        if(raycastColliderHit == null){
            InteractionDisplay.SetActive(false); 
            return; 
        }    
        //gets interact script of hit object from raycast                  
        if(raycastColliderHit.TryGetComponent<GenericInteractableScript>(out GenericInteractableScript objScr)){                    
            if(objScr != null){
                ShowInteractableName(raycastColliderHit.transform.position, objScr);
                if(Input.GetMouseButton(1)){
                    objScr.Interact(gameObject);
                }  
            }                        
        }      
    }

    private void ShowInteractableName(Vector2 objectPos, GenericInteractableScript objScript){             
        string interactName = objScript.GetInteractableID();                       
        //sets text of notification based on what object they are about to interact
        InteractionDisplay.GetComponent<TextMeshProUGUI>().text = $"Press [RMB] to interact with {interactName}";
        InteractionDisplay.SetActive(true);
         //sets UI element into position of object to interact
        InteractionDisplay.transform.position = Input.mousePosition;
    }

    public void SetCanMove(bool value){
        canMove = value;
    }

    private void DoPlayerDeath(){
        Debug.Log("Player has died");
        GameOverPanel.SetActive(true);
        DestroyOverNetwork();
        
        
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Enemy")){
            if(collider.gameObject.GetComponent<MeleeMinion>().GetAttackCooldown() <= 0){
                healthScr.takeDamage(collider.gameObject.GetComponent<MeleeMinion>().GetDamage());
                healthScr.UpdateHealthBar();
                collider.gameObject.GetComponent<MeleeMinion>().ResetAttackCooldown();
            }
            
        }
        else if(collider.gameObject.CompareTag("Boss")){
            healthScr.takeDamage(collider.gameObject.GetComponent<BossBT>().GetDamage());
            healthScr.UpdateHealthBar();
        }
    }
    private void CameraFollowPlayer(){
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y, -100);
    }
    public void SetGameObjectReferenceSource(GameObjectReferences GORScr){
        GORScript = GORScr;
        ReloadObjectReferences();
        canMove = true;

    }
    //reloads references to UI Elements
    private void ReloadObjectReferences(){
        InteractionDisplay = GORScript.InteractionDisplayOBJ;
        healthScr.SetObjectReferences(GORScript.HealthBar,GORScript.HealthBarTxt);
        WeaponsPool = GORScript.WeaponPoolList;
        GameOverPanel = GORScript.GameOverPanel;
    }
    public void DestroyOverNetwork(){
        photonView.RPC("RPCDestroyOverNetwork", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void RPCDestroyOverNetwork()
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

    public void TakeDamage(float damage){        
        healthScr.takeDamage(damage);        
        healthScr.UpdateHealthBar();
    }
}
