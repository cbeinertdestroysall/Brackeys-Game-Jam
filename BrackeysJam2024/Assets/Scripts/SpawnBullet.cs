using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawningBullet", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawningBullet()
    {
        Instantiate(bullet, spawnLocation.transform.position, Quaternion.identity);
        Debug.Log("Bullet has spawned");
    }
}
