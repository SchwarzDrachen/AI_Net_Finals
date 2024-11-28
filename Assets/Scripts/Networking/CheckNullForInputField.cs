using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckNullForInputField : MonoBehaviour
{
    [SerializeField]
    private Button connectButton;

    public void IsInputFieldNull(string value){
        if(string.IsNullOrEmpty(value)){
           connectButton.interactable = false;
           return;
        }
        connectButton.interactable = true;
    }
}
