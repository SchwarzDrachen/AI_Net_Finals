using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEnemyScript : MonoBehaviour
{
    [SerializeField] private HealthScript healthScr;
    void Update()
    {
        if(healthScr.isAlive() == false){
            Destroy(this.gameObject);
        }
    }
}
