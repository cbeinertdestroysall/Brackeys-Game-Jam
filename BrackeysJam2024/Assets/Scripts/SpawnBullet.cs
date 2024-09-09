using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bullet;

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
        Instantiate(bullet, this.transform.position, Quaternion.identity);
        Debug.Log("Bullet has spawned");
    }
}
