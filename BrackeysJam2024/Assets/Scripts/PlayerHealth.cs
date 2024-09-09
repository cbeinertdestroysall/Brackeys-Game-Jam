using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damageN;
    int minHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < minHealth)
        {
            currentHealth = minHealth;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(damageN);
            Debug.Log("Current health " + currentHealth);
        }
    }
}
