using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float moveSpeed = 3f;        //Enemy speed 
    private Transform player;
    //private Transform visualIndicator;  //Reference to the cube attached to the enemy to show the direction they are facing

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;                  //Finds the player through its tag
        //visualIndicator = transform.Find("dir");                                        //Finds the child cube using its name
    }

   
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;          //Calculate the direction from the enemy to the player
            transform.position += direction * moveSpeed * Time.deltaTime;                   //Move enemy towards the player 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,angle);                               //Rotate enemy to face player

            /*if(visualIndicator != null)
            {
                visualIndicator.rotation = Quaternion.Euler(0, 0, angle);
            }
            */
        }
    }
}
