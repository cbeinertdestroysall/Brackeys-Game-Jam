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
    public int minEnemiesToSpawn = 2;               //Min enemies to spawn at one time
    public int maxEnemiesToSpawn = 5;               //Max enemies to spawn at one time
    public float minDistanceBetweenEnemies = 3f;    //Minimum space between enemies when spawning
    public float spawnRadius = 1f;                  //Barrier around each spawner
    public LayerMask overlapLayerMask;              //Checks for overlapping enemies

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

            int enemiesToSpawnNow = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);                     //Determines the random number of enemies to spawn at once in a single spawner
            List<Vector3> spawnedPositions = new List<Vector3>();                           //Tracks the spawn positions
            /*for (int i = 0; i < enemiesToSpawnNow; i++)
            {
                Vector3 randomPosition = GetRandomPositionWithinSpawner(currentSpawnerOfEnemy.position, spawnRadius);

                if(!Physics.CheckSphere(randomPosition, 0.5f, overlapLayerMask))                                //Ensures no overlap when spawning
                {
                    Instantiate(enemyPrefab, currentSpawnerOfEnemy.position, Quaternion.identity);
                    enemiesSpawned++;

                    if (enemiesSpawned >= totalEnemiesToSpawn) break;                       //Stops when max enemy limit is reached
                }
                else
                {
                    Debug.Log("Enemies are overlapping at this spawn position. Retrying now.");
                    i--;
                }
            }
            */
            for (int i = 0; i < enemiesToSpawnNow; i++)
            {
                Vector3 spawnPosition;
                bool validPosition = false;

                do
                {
                    spawnPosition = GetRandomPositionWithinSpawner(currentSpawnerOfEnemy.position, spawnRadius);

                    validPosition = true;                                
                    foreach (Vector3 spawnPos in spawnedPositions)      //Check if the position of the enemy spawned is valid and far enough from other enemies
                    {
                        if (Vector3.Distance(spawnPosition, spawnPos) < minDistanceBetweenEnemies)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    if (validPosition && !Physics.CheckSphere(spawnPosition, 0.5f, overlapLayerMask))       //Another check for enemies currently in the scene
                    {
                        validPosition = true;
                    }
                    else
                    {
                        validPosition = false;
                    }
                } while (!validPosition);

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);               //Spawn the enemy in the valid position 
                spawnedPositions.Add(spawnPosition);                                        //Add the valid spawn position to the list
                enemiesSpawned++;

                if (enemiesSpawned >= totalEnemiesToSpawn) break;                           //End of the round, stop spawning enemies
            }

            yield return new WaitForSeconds(spawnInterval - arrowDisplayTime);                              //Another interval so enemies don't spawn rapidly
        }
    }

    Vector3 GetRandomPositionWithinSpawner (Vector3 center, float rad)
    {
        Vector2 randomPoint = Random.insideUnitSphere * rad;                                //Random point within a circle
        return new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);   //Converting back to a 3D Vector                                                                        
    }
    void Update()
    {
        
    }
}
