using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItemScript : MonoBehaviour
{
    [SerializeField]private GameObject GunObject;
    //Gun Info display
    [SerializeField]private TextMeshProUGUI GunInfoNameTXT;
    [SerializeField]private TextMeshProUGUI GunStatsTXT;
    //Item Info
    [SerializeField]private TextMeshProUGUI GunNameListItemTXT;
    [SerializeField]private TextMeshProUGUI GunCostListItemTXT;
    [SerializeField]private int GunCost;
    private GenericGunScript GunInfo;
    private PlayerControllerScript interactedPlayer;

    void Awake(){
        GunInfo = GunObject.GetComponent<GenericGunScript>();
    }

    public void BuyGun(){
        interactedPlayer.EquipGun(GunObject);

    }
    public void ShowGunItemInfo(){
        GunInfoNameTXT.text = GunInfo.Get_GunName();
        GunStatsTXT.text = $"Damage: {GunInfo.Get_GunDamage()}<br>FireRate: {GunInfo.Get_GunFireRate()}<br>Spread: {GunInfo.Get_GunSpread()}<br>Description:<br>{GunInfo.Get_GunDesc()}<br>";

    }

    public void SetPlayerInteracted(PlayerControllerScript player){
        if(interactedPlayer == null){
            interactedPlayer = player;
        }
    }

    public void ClearInteractedPlayerInfo(){
        interactedPlayer = null;
    }

}
