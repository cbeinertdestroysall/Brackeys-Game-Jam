using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrading : MonoBehaviour
{
    public bool CanUpgradeHP = false;

    public HealthBar healthBar;

    int payment;

    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        checkInputs();
    }

    void checkInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanUpgradeHP)
        {
            Debug.Log("registered upgrade input");
            UpgradeHP();
        }
    }

    void UpgradeHP()
    {
        Debug.Log("increase health");
        this.GetComponent<PlayerHealth>().maxHealth += 10;
        this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
        healthBar.SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
        this.GetComponent<CoinCollection>().coins -= payment;
        soundManager.PlayCoinSound();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            if (other.gameObject.GetComponent<PaymentManager>() != null && this.GetComponent<CoinCollection>().coins >= other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeHP = true;
                payment = other.gameObject.GetComponent<PaymentManager>().cost;
            }
            else if (this.GetComponent<CoinCollection>().coins < other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeHP = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            CanUpgradeHP = false;
            payment = 0;
        }
    }

}
