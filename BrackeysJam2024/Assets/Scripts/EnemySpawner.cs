using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnArrowPrefab;
    public List<Transform> spawnerLocations;        //List of all the spawner locations
    public float spawnInterval = 5f;                //Time between enemy spawning
    public int totalEnemiesToSpawn = 20;            //Max number of enemies spawned/round
    public float arrowDisplayTime = 2f;             //Time that the directional signifier is shown

    private int enemiesSpawned = 0;                 //Enemies spawned in real time
    private Transform currentSpawnerOfEnemy;        //Tracks what spawner will spawn the next enemy

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            currentSpawnerOfEnemy = spawnerLocations[Random.Range(0, spawnerLocations.Count)];              //Selecting the spawner for the next enemy
            
            if(spawnArrowPrefab != null )
            {
                GameObject arrow = Instantiate(spawnArrowPrefab, transform.position, Quaternion.identity);      //Show arrow pointing to the current enemy spawner
                arrow.GetComponent<SpawnArrow>().Initialize(currentSpawnerOfEnemy);

                yield return new WaitForSeconds(arrowDisplayTime);                                              //Allows the arrow to show on screen for a few seconds before spawning enemy
            }
            else
            {
                Debug.LogError("spawnArrowPrefab is not assigned in the inspector");
            }


            Instantiate(enemyPrefab, currentSpawnerOfEnemy.position, Quaternion.identity);
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval - arrowDisplayTime);                              //Another interval so enemies don't spawn rapidly
        }
    }
    void Update()
    {
        
    }
}
