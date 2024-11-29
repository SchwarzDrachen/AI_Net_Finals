using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]private float maxMovementSpeed;
    [SerializeField]private bool canMove = true;    
    [SerializeField]private GameObject gunHolder;
    [SerializeField]private GameObject playerSprite;
    [SerializeField]private GameObject currentGun;
    [SerializeField]private float interactDistance;  
    [SerializeField]private HealthScript healthScr;    
    [SerializeField]private GameObject InteractionDisplay;
    //for ease in use referencing UI elements
    [SerializeField]private GameObjectReferences GORScript;

    private Vector3 currentSpeed;
    private float timeCounter;
    RaycastHit2D raycast;
    Vector2 mouseWorldPos;
    private void Update(){
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
    private void Awake(){
        InteractionDisplay.SetActive(false);
        healthScr.ResetHealth();
    }
    private void OnEnable(){
        Debug.Log("initializing Player Values"); 
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

    public void EquipGun(GameObject gun){
        //removes current gun to be ready for requipping new gun
        Destroy(gunHolder.transform.GetChild(0).gameObject);

        GameObject newGun = Instantiate(gun);
        newGun.transform.SetParent(gunHolder.transform);
        newGun.transform.localPosition = new Vector2(0,0);
        newGun.transform.localRotation = Quaternion.Euler(0,0,0);

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
        //makes interact obj pos into screen pos for displaying
        Vector2 displPos = Camera.main.WorldToScreenPoint(objectPos);
        //sets text of notification based on what object they are about to interact
        InteractionDisplay.GetComponent<TextMeshProUGUI>().text = $"Press [RMB] to interact with {interactName}";
        InteractionDisplay.SetActive(true);
         //sets UI element into position of object to interact
        InteractionDisplay.transform.position = displPos;
    }

    public void SetCanMove(bool value){
        canMove = value;
    }

    private void DoPlayerDeath(){
        Debug.Log("Player has died(requires more polishing)");
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Enemy")){
            healthScr.takeDamage(50);
            Debug.Log("function requires more polishing");
        }
    }
    private void CameraFollowPlayer(){
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y, -100);
    }
    public void SetGameObjectReferenceSource(GameObjectReferences GORScr){
        GORScript = GORScr;
        ReloadObjectReferences();

    }
    //reloads references to UI Elements
    private void ReloadObjectReferences(){
        InteractionDisplay = GORScript.InteractionDisplayOBJ;
        healthScr.SetObjectReferences(GORScript.HealthBar,GORScript.HealthBarTxt);
    }


}
