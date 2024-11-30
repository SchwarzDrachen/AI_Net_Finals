using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManagerScript : SingletonPUN<GameManagerScript>
{
    [SerializeField]private int BaseEnemyWaveCount;
    [SerializeField]private int enemiesLeftToSpawn;
    [SerializeField]public int enemiesLeftToKill;
    [SerializeField]private int MaxWaves;
    [SerializeField]private int CurrentWave;
    [SerializeField]private GameObject[] Spawners;
    [SerializeField]private float SpawnerFrequency = 4;
    [SerializeField]private float SpawnerCountdown;
    [SerializeField]private TextMeshProUGUI ScoreDisplayTXT;
    [SerializeField]private TextMeshProUGUI EnemyCountDisplayTXT;
    [SerializeField]private TextMeshProUGUI WavesDisplayTXT;
    [SerializeField]private GameObject PlayersList;
    [SerializeField]private GameObject WinPanel;
    [SerializeField]private string Enemy = "MeleeEnemy";
    [SerializeField]private string Boss = "Boss";
    private Player playerLocal;
    private void Start(){
     enemiesLeftToSpawn = BaseEnemyWaveCount;
     enemiesLeftToKill = BaseEnemyWaveCount;
     CurrentWave = 4;
    }
   public void AddScore(int value, Player player){
        player.AddScore(value);        
        playerLocal = player;
   }
   public void UpdateLocalScoreDisplay(){
        if(playerLocal == null) return;
        string score = playerLocal.GetScore().ToString();
        ScoreDisplayTXT.text = $"Gold: {score}";
   }
   private void SpawnEnemy(){
     if(CurrentWave < 5){
        if(SpawnerCountdown <= 0 && enemiesLeftToSpawn > 0){
            int r = Random.Range(0, Spawners.Length-1);
            Vector3 SpawnPos = Spawners[r].transform.position;
            GameObject testenemy = PhotonNetwork.Instantiate(Enemy, SpawnPos, Quaternion.identity);
            testenemy.GetComponent<MeleeMinion>().SetPlayerList(PlayersList);
            SpawnerCountdown = SpawnerFrequency;
            enemiesLeftToSpawn--;
        }
     }
     else{
        if(SpawnerCountdown <= 0 && enemiesLeftToSpawn > 0){
          int r = Random.Range(0, Spawners.Length-1);
          Vector3 SpawnPos = Spawners[r].transform.position;
          GameObject boss = PhotonNetwork.Instantiate(Boss, SpawnPos, Quaternion.identity);
          boss.GetComponent<BossBT>().SetPlayerList(PlayersList);
          SpawnerCountdown = SpawnerFrequency;
          enemiesLeftToSpawn--;
        }
     }
    SpawnerCountdown -= Time.deltaTime;
   }
   private void UpdateEnemyCount(){
    EnemyCountDisplayTXT.text = $"Enemies Left: {enemiesLeftToKill}";
   }
   public void ModifyEnemyCount(int value){     
     photonView.RPC("ModifyEnemyCountRPC", RpcTarget.AllBuffered,value);
   }
   [PunRPC]
   private void ModifyEnemyCountRPC(int value){
     enemiesLeftToKill += value;
   }
   private void UpdateWaveCount(){
    WavesDisplayTXT.text = $"Wave: {CurrentWave}";
   }
   private void CheckIfWaveClear(){
    if(enemiesLeftToKill <= 0){
        Debug.Log("Wave Clear");
          if(CurrentWave >= MaxWaves){
               ShowWinPanel();
          }
          else{
               CurrentWave++;
               enemiesLeftToSpawn = BaseEnemyWaveCount * CurrentWave;
               enemiesLeftToKill = enemiesLeftToSpawn;
               if(CurrentWave == 5){
                enemiesLeftToSpawn = 1;
                enemiesLeftToKill = 1;
               }
          }
    }
    
   }
   public void ShowWinPanel(){
     photonView.RPC("ShowWinPanelRPC", RpcTarget.AllBuffered);
   }
   [PunRPC]
   private void ShowWinPanelRPC(){
     WinPanel.SetActive(true);
   }
   
   void Update(){
     UpdateLocalScoreDisplay();
     UpdateEnemyCount();
     UpdateWaveCount();
     if (!PhotonNetwork.IsMasterClient) return;
     SpawnEnemy();     
     CheckIfWaveClear();
   }
   public void QuitGame(){
      Application.Quit();
   }

}
