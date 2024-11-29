using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using System.Linq;
using TMPro;

public class ConnectionScript : MonoBehaviourPunCallbacks
{
    [SerializeField]private GameObject ConnectionPanel;
    [SerializeField]private GameObject RoomPanel;
    [SerializeField]private GameObject PlayerContentList;
    [SerializeField]private TextMeshProUGUI roomInfoText;  
    [SerializeField]private GameObject LoadingTXT;  
    //prefabs
    [SerializeField]private GameObject PlayerListItem;
    
    [SerializeField]private int MaxPlayersPerRoom = 4;
    [SerializeField]private int LevelIDToLoad = 1;
    
    void Start()
    {
        //resets opening of panels on start up
        ConnectionPanel.SetActive(true);
        RoomPanel.SetActive(false);
        LoadingTXT.SetActive(false);
    }

    //Connects to online multiplayer servers only, doesnt create a room out right
    public void Connect()
    {
        if(PhotonNetwork.IsConnected){
            Debug.Log("Already connected");
            return;
        }
        Debug.Log("Connecting to Server....");
        // Connect to photon network
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connected to Server");
        LoadingTXT.SetActive(true);

    }
    //panel switching functions
    public void GoToConnectServerPanel(){
        ConnectionPanel.SetActive(true);
        RoomPanel.SetActive(false);
    }
    public void GoToJoinedRoomPanel(){
        ConnectionPanel.SetActive(false);
        RoomPanel.SetActive(true);
    }

    public void StartGame(){
        if(PhotonNetwork.IsMasterClient){
            photonView.RPC("StartGameRPC", RpcTarget.AllBuffered);
        }
        
    }
    //button functions
    [PunRPC]
    public void StartGameRPC(){
        LoadingTXT.SetActive(true);
        PhotonNetwork.LoadLevel(LevelIDToLoad);
        //loads world
    }
    public void LeaveRoom(){        
        PhotonNetwork.LeaveRoom();
        LoadingTXT.SetActive(true);
    }    
    //Updates when new rooms are made
     public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join a room: {message}");
        Debug.Log($"Create a room instead..");
        // If no rooms are available (all rooms are full or there are no rooms at all)..
        // Create own room
        PhotonNetwork.CreateRoom(
            roomName: $"{PhotonNetwork.NickName}'s Room",
            roomOptions: new RoomOptions{
                MaxPlayers = MaxPlayersPerRoom
            });
    }
    //Updates Room Info
    public override void OnJoinedRoom(){ 
        LoadingTXT.SetActive(false);     
        GoToJoinedRoomPanel();
        UpdateRoomInfoDisplay();
        
    }
    private void UpdateRoomInfoDisplay(){
        Debug.Log("Joined a Room");
        //checks if there are items to hide then hides list items to be reused again
        if(PlayerContentList.transform.childCount > 0){
            foreach(Transform item in PlayerContentList.transform){
                item.gameObject.SetActive(false);
            }
        }
        //adds more items if player count is larger
        

        if(PhotonNetwork.PlayerList.Length > PlayerContentList.transform.childCount){
            int itemsToAdd = PhotonNetwork.PlayerList.Length - PlayerContentList.transform.childCount;
            for(int i = itemsToAdd; i > 0; i--){
                GameObject newListItem = Instantiate(PlayerListItem,PlayerContentList.transform);
            }
        }
        int PlayerListID = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            GameObject PlayerItemListObj = PlayerContentList.transform.GetChild(PlayerListID).gameObject;  
            //Gets RoomList Item in the content box list                      
            TextMeshProUGUI PlayerItemTXT = PlayerContentList.transform.GetChild(PlayerListID).GetChild(0).GetComponent<TextMeshProUGUI>();
            if(player.IsMasterClient){
                PlayerItemTXT.text = $"Host: {player.NickName}";
            }
            else{
                PlayerItemTXT.text = player.NickName;
            }
            
            PlayerItemListObj.SetActive(true);
            PlayerListID++; 
            Debug.Log(PlayerListID);
        }
        roomInfoText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are connected To Master");
        PhotonNetwork.JoinRandomRoom();
        LoadingTXT.SetActive(true);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomInfoDisplay();
    }


}
