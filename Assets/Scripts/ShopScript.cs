using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : GenericInteractableScript
{
    [SerializeField]private GameObject ShopUI;
    [SerializeField]private GameObject ShopItemListBox;
    [SerializeField]private GameObject GunInfoPanel;

    private List<ShopItemScript> ShopItemList = new();    
    private PlayerControllerScript playerScr;
    

    public override void Interact(GameObject Player)
    {
        playerScr = Player.GetComponent<PlayerControllerScript>();
        //Opens shop ui
        ShopUI.SetActive(true); 
        playerScr.SetCanMove(false);

        //Auto Setup for the shop item informations
        if(ShopItemList.Count <= 0){
            foreach(ShopItemScript childScr in ShopItemListBox.transform.GetComponentsInChildren<ShopItemScript>()){                
                ShopItemList.Add(childScr);
            }

        }
        //sets each shop item to reference player for gun equipping
        foreach(ShopItemScript item in ShopItemList){
            item.SetPlayerInteracted(playerScr);
        }

        //closes gun info panel on start
        GunInfoPanel.SetActive(false);

    }

    public void CloseShop(){                
        playerScr.SetCanMove(true);
        playerScr = null;
        ShopUI.SetActive(false);
        foreach(ShopItemScript item in ShopItemList){
            item.ClearInteractedPlayerInfo();
        }
    }

}
