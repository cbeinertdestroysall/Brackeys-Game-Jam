using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    Vector3 playerPos;
    Vector3 playerDir;

    public float timeToDestroy;

    IEnumerator Destroy()
    {
        Debug.Log("Destroy coroutine has started");
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }

    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());

        //when the bullets spawn, they will rotate towards and follow the position of the mouse while travelling at bulletSpeed
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        //trying to find the direction of the player when spawned
        playerDir = new Vector3(this.transform.position.x - playerPos.x, 0, this.transform.position.z - playerPos.z).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //moving in the direction of the player (not directly towards player)
        this.transform.position -= (playerDir * moveSpeed) * Time.deltaTime;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.layer == 31)
        {
            Destroy(this.gameObject);
        }
    }
}