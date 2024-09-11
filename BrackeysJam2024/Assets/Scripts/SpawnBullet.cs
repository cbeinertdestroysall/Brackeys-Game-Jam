using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bullet;
    public float time;
    public float startTime;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawningBullet", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        time -= 1 * Time.deltaTime;
        if (time < 0)
        {
            time = startTime;
        }

        SpawningBullet();
    }

    public void SpawningBullet()
    {
        if (time <= 0.01)
        {
            Instantiate(bullet, this.transform.position, Quaternion.identity);
            Debug.Log("Bullet has spawned");
        }
    }
}
