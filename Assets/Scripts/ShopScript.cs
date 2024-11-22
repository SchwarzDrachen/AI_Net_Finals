using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : GenericInteractableScript
{
    [SerializeField]private GameObject ShopUI;
    [SerializeField]private int ShopItemList;
    private GameObject player;
    private PlayerControllerScript playerScr;
    public override void Interact(GameObject Player)
    {
        playerScr = Player.GetComponent<PlayerControllerScript>();
        
        ShopUI.SetActive(true); 
        playerScr.SetCanMove(false);

        this.player = Player;
    }

    public void CloseShop(){                
        playerScr.SetCanMove(true);
        player = null;
        playerScr = null;
        ShopUI.SetActive(false);
    }

}
