using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float damageN;
    float minHealth = 0;

    [SerializeField] PlayerController PC;

    public HealthBar healthBar;
    public TMP_Text healthNumber;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(damageN);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }

        healthNumber.text = "Health: " + currentHealth;
    }

    void TakeDamage(float damage)
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
        if(other.gameObject.tag == "EnemyBullet")
        {
            TakeDamage(damageN);
        }
        else if (other.gameObject.tag == "Enemy" && !PC.dash)
        {
            TakeDamage(damageN);
            Debug.Log("Current health " + currentHealth);
        }
    }
}
