using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damageN;
    int minHealth = 0;

    public HealthBar healthBar;
    public TMP_Text healthNumber;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(damageN);
        }

        healthNumber.text = "Health: " + currentHealth;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth < minHealth)
        {
            currentHealth = minHealth;
            healthBar.SetHealth(currentHealth);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(damageN);
            Debug.Log("Current health " + currentHealth);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(damageN);
            Debug.Log("Current health " + currentHealth);
        }
    }
}
