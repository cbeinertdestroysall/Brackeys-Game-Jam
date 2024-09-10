using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrading : MonoBehaviour
{
    public bool upgradeHealth = false;

    public HealthBar healthBar;

    int payment;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (upgradeHealth)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("increase health");
                this.GetComponent<PlayerHealth>().maxHealth += 10;
                this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
                healthBar.SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
                this.GetComponent<CoinCollection>().coins -= payment;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            if (other.gameObject.GetComponent<PaymentManager>() != null && this.GetComponent<CoinCollection>().coins >= other.gameObject.GetComponent<PaymentManager>().cost)
            {
                upgradeHealth = true;
                payment = other.gameObject.GetComponent<PaymentManager>().cost;
            }
            else if (this.GetComponent<CoinCollection>().coins < other.gameObject.GetComponent<PaymentManager>().cost)
            {
                upgradeHealth = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            upgradeHealth = false;
            payment = 0;
        }
    }

}
