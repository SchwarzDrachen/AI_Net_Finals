using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] 
    private float minSpawnInterval;
    [SerializeField] 
    private float maxSpawnInterval;
    [SerializeField] 
    private GameObject MELEE_ENEMY_PREFAB;
    [SerializeField] 
    private GameObject RANGE_ENEMY_PREFAB;
    [SerializeField]
    private GameObject[] waypoints;
    private float spawnInterval;
    private Coroutine spawner;

    void Update()
    {
        if(spawner != null){
            return;
        }
        spawner = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine(){
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(spawnInterval);
        SpawnEnemy();
        spawner = null;
    }

    private void SpawnEnemy(){
        Instantiate(MELEE_ENEMY_PREFAB, GetSpawnPosition(),Quaternion.identity);
        Instantiate(RANGE_ENEMY_PREFAB, GetSpawnPosition(),Quaternion.identity);
    }

    private Vector3 GetSpawnPosition(){
        int i = Random.Range(0,3);
        return waypoints[i].transform.position;
    }
}
