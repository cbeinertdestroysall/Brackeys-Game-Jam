using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBullet : MonoBehaviour
{
    Vector3 targetPos;
    Vector3 targetDir;

    public float timeToDestroy;

    public AudioSource audioS;

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
        if(GetComponentInParent<TurretScript>().target != null)
        {
            targetPos = GetComponentInParent<TurretScript>().target.transform.position;
        }
       

        //trying to find the direction of the player when spawned
        targetDir = new Vector3(this.transform.position.x - targetPos.x, 0, this.transform.position.z - targetPos.z).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //moving in the direction of the player (not directly towards player)
        this.transform.position -= (targetDir * moveSpeed) * Time.deltaTime;
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
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<GenericEnemyAi>().TakeDamage(1);
            audioS.Play();
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;
            timeToDestroy = 1f;
            StartCoroutine(Destroy());
        }
        else if(other.gameObject.layer == 31)
        {
            Destroy(this.gameObject);
        }
    }
}
