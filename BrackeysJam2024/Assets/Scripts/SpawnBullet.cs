using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bullet;
    
    [SerializeField] int shootInterval,shootTime;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawningBullet", 0, 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shootTime <= 0)
        {
            SpawningBullet();
            shootTime = shootInterval;
        }
        else
        {
            shootTime -=1;
        }
    }

    public void SpawningBullet()
    {

        Instantiate(bullet, this.transform.position, Quaternion.identity);
        Debug.Log("Bullet has spawned");


    }
}
