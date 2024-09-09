using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    public float arrowLifetime = 2f;       //How long the arrow stays visible
    public float circleRadius = 3f;        //The distance from the player where the arrow will appear (circle radius)
    private Transform player;
    private Transform spawner;

    
    public Vector3 rotationOffset = new Vector3(0, 0, 0);                   //Aligns the arrow's forward direction

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;      //Find the player by tag
    }

    public void Initialize(Transform spawner)
    {
        this.spawner = spawner;
        Destroy(gameObject, arrowLifetime);                                 //Destroy the arrow after its lifetime
    }

    void Update()
    {
        if (spawner != null && player != null)
        {
            Vector3 direction = (spawner.position - player.position).normalized;        //Calculate the direction from the player to the spawner

            
            if (direction.sqrMagnitude > 0.01f)
            {
                Vector3 offsetPosition = player.position + direction * circleRadius;    //Positions the arrow at a certain distance (circleRadius) from the player in the direction of the spawner

                transform.position = new Vector3(offsetPosition.x, player.position.y + 2f, offsetPosition.z);       //Apply the offset while keeping the Y position of the arrow slightly above/below the player

                Vector3 lookDirection = spawner.position - transform.position;          //Direction towards the spawner
                lookDirection.y = 0;                                                    //Keeping the arrow level with the ground
                if (lookDirection.sqrMagnitude > 0.01f) 
                {
                    Quaternion rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(rotationOffset);
                    transform.rotation = rotation;                                      //Set the arrow's rotation to face the spawner with the offset
                }
            }
        }
    }
}







