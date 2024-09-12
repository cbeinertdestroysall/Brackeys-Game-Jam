using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    BoxCollider col;
    public GameObject boat;
    PlayerController playController;
    CharacterController characterController;

    PlayerHealth health;

    public float ResetTime;

    //public Transform spawnPos;

    public bool functionCalled = false;

    // Start is called before the first frame update
    void Awake()
    {
        functionCalled = false;
        col = this.GetComponent<BoxCollider>();
        playController = this.GetComponent<PlayerController>();
        health = this.GetComponent<PlayerHealth>();
        characterController = this.GetComponent<CharacterController>();
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(ResetTime);
        functionCalled = false;
        col.enabled = true;
        boat.SetActive(true);
        playController.enabled = true;
        characterController.enabled = true;
        //characterController.enabled = true;
        health.currentHealth = health.maxHealth;
        this.transform.position = new Vector3(0, 1.11f, 0);
        
        Debug.Log("player is reset");
        Debug.Log("Character Controller position: " + characterController.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (health.currentHealth <= 0)
        {
            if (!functionCalled)
            {
                functionCalled = true;
                ResetPos();
            }
        }
    }

    void ResetPos()
    {
        col.enabled = false;
        boat.SetActive(false);
        playController.enabled = false;
        characterController.enabled = false;
        //this.transform.position = spawnPos.transform.position;
        StartCoroutine(Reset());
        
    }
}
