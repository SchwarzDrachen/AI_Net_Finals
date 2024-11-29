using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private GameObject HealthBar;
    [SerializeField] private TextMeshProUGUI HealthValueLabel;

    // public void OnInitialize(){
    //     ResetHealth();
    //     //UpdateHealthBar();

    // }
    public void Start(){
        
    }

    public bool isAlive(){
        if(currentHealth > 0){
            return true;
        }
        else{
            return false;
        }
    }

    public float GetCurrentHealth(){
        return currentHealth;
    }
    public void SetmaxHealth(int healthValue){
        maxHealth = healthValue;
        currentHealth = healthValue;
    }


    // public void takeDamage(float damageValue){
    //      photonView.RPC("RPCtakeDamage", RpcTarget.AllBuffered, damageValue);
    // }
    // [PunRPC]
    public void takeDamage(float damageValue){
        if(currentHealth > 0){
            currentHealth -= damageValue;
        }
        math.clamp(currentHealth,0,maxHealth);
    }
    public void healHealth(float healValue){
        if(currentHealth < maxHealth){
            currentHealth += healValue;
        }
        math.clamp(currentHealth,0,maxHealth);
        //UpdateHealthBar();
    }
    // public void UpdateHealthBar(){
    //     photonView.RPC("RPCUpdateHealthBar", RpcTarget.AllBuffered);
    // }
    // [PunRPC]
    public void UpdateHealthBar(){                  
        float HealthPercent = ((float)currentHealth / (float)maxHealth);       
        HealthBar.GetComponent<UnityEngine.UI.Image>().fillAmount = HealthPercent;
        if(HealthValueLabel != null){
            HealthValueLabel.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }

    }

    public void ResetHealth(){
        currentHealth = maxHealth;
        //UpdateHealthBar();
    }

    public void SetObjectReferences(GameObject healthBar, TextMeshProUGUI healthValueLabel){
        HealthBar = healthBar;
        HealthValueLabel = healthValueLabel;
        UpdateHealthBar();
    }


}
