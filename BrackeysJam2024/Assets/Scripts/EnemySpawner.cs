using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefabGuppy;
    public GameObject enemyPrefabPirate;
    public GameObject enemyPrefabAngular;
    public GameObject spawnArrowPrefab;

    [SerializeField] Transform enemyParent;
    public List<Transform> spawnerLocations;        //List of all the spawner locations
    public float spawnInterval = 5f;                //Time between enemy spawning
    public int totalEnemiesToSpawn = 20;            //Max number of enemies spawned/round
    public float arrowDisplayTime = 2f;             //Time that the directional signifier is shown
    public int minEnemiesToSpawn = 2;               //Min enemies to spawn at one time
    public int maxEnemiesToSpawn = 5;               //Max enemies to spawn at one time
    public float minDistanceBetweenEnemies = 3f;    //Minimum space between enemies when spawning
    public float spawnRadius = 5f;                  //Barrier around each spawner
    public LayerMask overlapLayerMask;              //Checks for overlapping enemies
    public int maxEnemySpawnPosAttempts;            //Limits how many potential locations for spawning enemies
    public float calmBeforeStorm = 10f;             //The time between rounds
    public TextMeshProUGUI intervalTimerText;                  //UI component to show the timer between rounds

    private int enemiesSpawned = 0;                 //Enemies spawned in real time
    private Transform currentSpawnerOfEnemy;        //Tracks what spawner will spawn the next enemy
    private bool spawningInProgress;                //Tracks whether or not the coroutine for spawning is running
    private int currentRound = 1;                   //Tracks the current round the player is in 
    private bool roundInProgress = false;           //Tracks whether or not a round is in progress
    public List <GameObject> activeEnemies = new List<GameObject>();           //Active enemies in the scene. This will be used to change the rounds
    
    public int enemyIncrementPerRound = 5;          //Round based increment for enemies to spawn per round
    public int initialGuppies = 15;                 //Starting amount of guppies
    public int initialPirates = 0;                  //Starting amount of pirates 
    public int initialAngulars = 0;                 //Starting amount of angulars

    private int guppiesToSpawn;                     //Amount of guppies each round
    private int piratesToSpawn;                     //Amount of pirates each round 
    private int angularsToSpawn;                    //Amount of angulars each round

    public AudioSource audioSource;
    public AudioClip restartStartSound;
   
    void Start()
    {
        StartCoroutine(RoundManager());
    }

    IEnumerator RoundManager()
    {
        while (true)
        {
            yield return StartCoroutine(StartRound());
            //yield return StartCoroutine(AllEnemiesDead());
            yield return StartCoroutine(NextRoundInterval());
        }
    }

    IEnumerator StartRound()
    {
        Debug.Log($"Starting round {currentRound}");

        if(audioSource != null && restartStartSound != null)
        {
            audioSource.PlayOneShot(restartStartSound);
        }

        guppiesToSpawn = Mathf.Max(initialGuppies - (currentRound - 1), 5);                     //Reduces the amount of guppies over later rounds, but keeps a minimum
        //piratesToSpawn = initialPirates + (currentRound - 1);                                   //Increases pirates each round
        //angularsToSpawn = initialAngulars + (currentRound - 1) * 2;                             //Increases angulars faster than pirates
        
        if (currentRound >= 2)
        {
            piratesToSpawn = initialPirates + (currentRound - 2);                                   //Increases pirates each round past the first round
        }
        else
        {
            piratesToSpawn = 0;
        }

        if (currentRound >= 3)
        {
            angularsToSpawn = initialAngulars + (currentRound - 3) * 2;                             //Increases angulars faster than pirates after the second round
        }
        else
        {
            angularsToSpawn = 0;
        }

        totalEnemiesToSpawn = guppiesToSpawn + piratesToSpawn + angularsToSpawn;

        enemiesSpawned = 0;
        spawningInProgress = true;
        roundInProgress = true;

        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            currentSpawnerOfEnemy = spawnerLocations[Random.Range(0, spawnerLocations.Count)];              //Selecting the spawner for the next enemy

            if (currentSpawnerOfEnemy == null)
            {
                Debug.LogError("There is no spawner location. Check the list and fix it");
                yield break;
            }

            Debug.Log($"Current spawner: {currentSpawnerOfEnemy.name}");

            if (spawnArrowPrefab != null)
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

            for (int i = 0; i < enemiesToSpawnNow; i++)
            {
                if (enemiesSpawned >= totalEnemiesToSpawn) { break; }

                Vector3 spawnPosition;
                bool validPosition = false;
                int attemptCount = 0;

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

                    attemptCount++;

                    if (attemptCount >= maxEnemySpawnPosAttempts)               //Exits the loop if the max attempts has been reached
                    {
                        Debug.LogWarning($"Could not find a valid spawn position after {maxEnemySpawnPosAttempts}");
                        break;
                    }
                } while (!validPosition);

                if (validPosition)
                {
                    GameObject enemyToSpawn = ChooseEnemyType();
                    GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, enemyParent);               //Spawn the enemy in the valid position 
                    activeEnemies.Add(spawnedEnemy);
                    spawnedPositions.Add(spawnPosition);
                    enemiesSpawned++;
                    //GenericEnemyAi enemyAI = spawnedEnemy.GetComponent<GenericEnemyAi>();

                    /*if (enemyAI != null )
                    {
                        enemyAI.enemySpawner = this;
                    }
                    */
                    //activeEnemies.Add(spawnedEnemy);                                        //Add the valid spawn position to the list


                }

                if (enemiesSpawned >= totalEnemiesToSpawn) break;                           //End of the round, stop spawning enemies
            }

            yield return new WaitForSeconds(spawnInterval - arrowDisplayTime);                              //Another interval so enemies don't spawn rapidly
        }

        spawningInProgress = false;

        while (activeEnemies.Count > 0)
        {
            yield return null; 
        }

        roundInProgress = false;
        currentRound++;
    }

    GameObject ChooseEnemyType()
    {
        if (angularsToSpawn > 0)
        {
            angularsToSpawn--;
            return enemyPrefabAngular;
        }
        else if (piratesToSpawn > 0)
        {
            piratesToSpawn--;
            return enemyPrefabPirate;
        }
        else
        {
            guppiesToSpawn--;
            return enemyPrefabGuppy;
        }
    }
    /*IEnumerator AllEnemiesDead()
    {
        while (activeEnemies.Count > 0)         //Waits until all of the enemies have been removed from the list
        {
            yield return null; 
        }
    }
    */
    IEnumerator NextRoundInterval()
    {
        float timer = calmBeforeStorm;
        while (timer > 0)
        {
            if (intervalTimerText != null)
            {
                intervalTimerText.text = $"Next Storm in:{Mathf.Ceil(timer)} seconds";
            }

            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        if (intervalTimerText != null)
        {
            intervalTimerText.text = "";
        }

        Debug.Log("The next round is starting");
    }

    Vector3 GetRandomPositionWithinSpawner(Vector3 center, float rad)
    {
        Vector2 randomPoint = Random.insideUnitSphere * rad;                                //Random point within a circle
        return new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);   //Converting back to a 3D Vector                                                                        
    }

    public void HandleEnemyDeath(GameObject enemy)                 //Logic for the enemy deaths
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);            //Removes enemy from the list when the player kills it
            Debug.Log("Enemy removed. Remaining enemies in scene is: " + activeEnemies.Count);
        }
    }

    void SpawnEnemyAtPosition(Vector3 pos)
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefabGuppy, pos, Quaternion.identity, enemyParent);
        GenericEnemyAi enemyAI = spawnedEnemy.GetComponent<GenericEnemyAi>();

        if (enemyAI != null)
        {
            enemyAI.enemySpawner = this;
        }

        activeEnemies.Add(spawnedEnemy);
    }
    void Update()
    {

    }


    /*IEnumerator SpawnEnemies()
    {
        if (spawningInProgress) yield break;            //Checks to see if the coroutine is running multiple times 
        spawningInProgress = true;

        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            currentSpawnerOfEnemy = spawnerLocations[Random.Range(0, spawnerLocations.Count)];              //Selecting the spawner for the next enemy
            
            if (currentSpawnerOfEnemy == null)
            {
                Debug.LogError("There is no spawner location. Check the list and fix it");
                yield break;
            }

            Debug.Log($"Current spawner: {currentSpawnerOfEnemy.name}");

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
                    Instantiate(enemyPrefab, currentSpawnerOfEnemy.position, Quaternion.identity,enemyParent);
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

    /*for (int i = 0; i < enemiesToSpawnNow; i++)
    {
        Vector3 spawnPosition;
        bool validPosition = false;
        int attemptCount = 0;

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

            attemptCount++;

            if (attemptCount >= maxEnemySpawnPosAttempts)               //Exits the loop if the max attempts has been reached
            {
                Debug.LogWarning($"Could not find a valid spawn position after {maxEnemySpawnPosAttempts}");
                break;
            }
        } while (!validPosition);

        if (validPosition)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);               //Spawn the enemy in the valid position 
            spawnedPositions.Add(spawnPosition);                                        //Add the valid spawn position to the list
            enemiesSpawned++;
        }

        if (enemiesSpawned >= totalEnemiesToSpawn) break;                           //End of the round, stop spawning enemies
    }

    yield return new WaitForSeconds(spawnInterval - arrowDisplayTime);                              //Another interval so enemies don't spawn rapidly
}

spawningInProgress = false;
}


Vector3 GetRandomPositionWithinSpawner (Vector3 center, float rad)
{
Vector2 randomPoint = Random.insideUnitSphere * rad;                                //Random point within a circle
return new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);   //Converting back to a 3D Vector                                                                        
}
void Update()
{

}*/
}
