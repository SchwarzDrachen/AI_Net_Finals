using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]private float maxMovementSpeed;
    [SerializeField]private bool canMove = true;    
    [SerializeField]private GameObject gunHolder;
    [SerializeField]private GameObject playerSprite;
    [SerializeField]private GameObject currentGun;
    [SerializeField]private float interactDistance;  
    [SerializeField]private GameObject InteractionDisplay;
    [SerializeField]private Camera camera;
    private Vector3 currentSpeed;
    private float timeCounter;

    private void Update(){
        if(canMove){
            MovementScript();
            FlipSpriteOnDirection();
            AimGun();
            Interact();
            camera.transform.position = transform.position;
            
        }
        //checks if left mousebutton is held down to shoot
        if(Input.GetMouseButton(0)){
            //main gun shoot function is within the gun script itself
            currentGun.GetComponent<GenericGunScript>().Shoot();
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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = new Vector3(transform.position.x,transform.position.y,0f);
        mouseWorldPos.z = 0f;
        Vector3 gunAimPos = mouseWorldPos - playerPos;
        float aimAngle = Mathf.Atan2(gunAimPos.y, gunAimPos.x) * Mathf.Rad2Deg;        
        gunHolder.transform.rotation = Quaternion.AngleAxis(aimAngle,Vector3.forward);

    }

    public void EquipGun(GameObject gun){
        //removes current gun to be ready for requipping new gun
        Destroy(gunHolder.transform.GetChild(0));

        gun.transform.parent = gunHolder.transform;
    }

    private void Interact(){        
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 InteractOrigin = new Vector2(gunHolder.transform.position.x,gunHolder.transform.position.y);
        Vector2 mousedir = mouseWorldPos - InteractOrigin;
        RaycastHit2D raycast = Physics2D.Raycast(InteractOrigin, mousedir,interactDistance);
        //gets collider that raycast hit
        Collider2D raycastColliderHit = raycast.collider;

        //ends script if no collider is hit
        if(raycastColliderHit == null){
            InteractionDisplay.SetActive(false);
            return;
        } 
        
        if(raycastColliderHit.CompareTag("Interactable")){
            
            raycastColliderHit.TryGetComponent<GenericInteractableScript>(out GenericInteractableScript genInteractScript);
            if(genInteractScript == null) return;
            ShowInteractableName(raycastColliderHit.gameObject);
            if(Input.GetMouseButton(1)){
                genInteractScript.Interact();
            }            
        }           
    }

    private void ShowInteractableName(GameObject interactableObject){
        Vector2 objPos = interactableObject.transform.position;
        Vector2 objToScreenPos = camera.WorldToScreenPoint(objPos);
        string interactName = interactableObject.GetComponent<GenericInteractableScript>().GetInteractableID();
        InteractionDisplay.SetActive(true);
        InteractionDisplay.transform.position = objToScreenPos;
        InteractionDisplay.GetComponent<TextMeshProUGUI>().text = $"Press [RMB] to interact with {interactName}";
    }



}
