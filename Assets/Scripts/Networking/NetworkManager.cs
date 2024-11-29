using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class NetworkManager : SingletonPUN<NetworkManager>
{
    [SerializeField]private GameObjectReferences GORScr;
    [SerializeField]private GameObject PlayersList;
    [SerializeField]private int[] PlayerCurrencyCount = new int[4];
    private const string PLAYER_PREFAB_NAME = "PlayerOBJ";   
    public bool IsInitialized = false;
    protected override void Awake()
    {
        base.Awake();
        if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }        

        // Spawn the player
        GameObject player = PhotonNetwork.Instantiate(PLAYER_PREFAB_NAME, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerControllerScript>().SetGameObjectReferenceSource(GORScr);
        player.transform.SetParent(PlayersList.transform);
        IsInitialized = true;        
    }
}
