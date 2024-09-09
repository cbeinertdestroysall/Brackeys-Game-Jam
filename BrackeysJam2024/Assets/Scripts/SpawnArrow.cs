using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    public float arrowDisplayTime = 2f;             //How long the arrow stays on screen 
    public Vector3 arrowOffset;
    private Transform player;
    private Transform spawner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;              //Find the player through their tag
    }

    public void Initialize(Transform spawner)                                       //Initializes the arrow
    {
        this.spawner = spawner;
        Destroy(gameObject, arrowDisplayTime);
    }

    void Update()
    {
        if (spawner != null && player != null)
        {
            transform.position = player.position + arrowOffset;                     //Positions the arrow signifier above the player no matter where they are

            Vector2 direction = spawner.position - player.position;                 //Point the arrow towards the current spawner
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

        }
    }
}
