using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RoomItemScript : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI RoomNameTXT;
    [SerializeField]private string RoomNameData;    
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(RoomNameData);
    }
    public void SetRoomInfo(string roomName){
        RoomNameTXT.text = roomName;
        RoomNameData = roomName;
    }
}
