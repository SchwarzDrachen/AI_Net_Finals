using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] 
    private float minSpawnInterval = 0.50f;
    [SerializeField] 
    private float maxSpawnInterval = 2.50f;

    private const string MELEE_ENEMY_PREFAB = "Melee";
    private const string RANGE_ENEMY_PREFAB = "Range";
    private float spawnInterval;
    private Boundary boundary;
    private Coroutine spawner;

    private void Start(){
        boundary = new();
        boundary.CalculateScreenRestrictions();
    }
    void Update()
    {
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
    }

    private Vector2 GetSpawnPosition(){
        // Generate a random position along an axis
        float xRandomPosition = Random.Range(-boundary.Bounds.x, boundary.Bounds.x);
        float yRandomPosition = Random.Range(-boundary.Bounds.y, boundary.Bounds.y);
        int areaToSpawn = Random.Range(0, 4);
        switch(areaToSpawn){
            // Upper part
            case 0:
                return new Vector2(xRandomPosition, boundary.Bounds.y);
            //Right part
            case 1:
                return new Vector2(boundary.Bounds.x, yRandomPosition);
            // Bottom part
            case 2:
                return new Vector2(xRandomPosition, -boundary.Bounds.y);
            //Left part
            case 3:
                return new Vector2(-boundary.Bounds.x, yRandomPosition);
        }
        return Vector2.zero;
    }
}
