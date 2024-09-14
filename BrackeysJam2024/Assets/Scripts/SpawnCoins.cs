using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coin;

    public Transform spawnOrigin;

    public float transformMax;
    public float transformMin;

    //public float coinCount = 0;
    public int coinMax;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {



    }

    public void Spawn()
    {
        for (int i = 0; i < coinMax; i++)
        {
            Instantiate(coin, new Vector3(spawnOrigin.transform.position.x + Random.Range(transformMin, transformMax), spawnOrigin.transform.position.y, spawnOrigin.transform.position.z + Random.Range(transformMin, transformMax)), Quaternion.identity, GameObject.FindGameObjectWithTag("CoinParent").transform);
        }
        //coinCount += 1;
    }
}
