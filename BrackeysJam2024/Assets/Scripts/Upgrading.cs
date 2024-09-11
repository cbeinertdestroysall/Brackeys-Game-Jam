using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrading : MonoBehaviour
{
    public bool CanUpgradeHP = false;

    public HealthBar healthBar;

    int payment;

    public SoundManager soundManager;

    public float healthUpgradeAmount;

    GameObject healthUpgrade;

    /*IEnumerator IsNotUpgrading()
    {
        yield return new WaitForSeconds(0.1f);
        isUpgrading = false;
    }*/

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
        this.GetComponent<PlayerHealth>().maxHealth += healthUpgradeAmount;
        this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
        healthBar.SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
        this.GetComponent<CoinCollection>().coins -= payment;
        soundManager.PlayUpgrade();

        if (healthUpgrade.GetComponent<PaymentManager>() != null)
        {
            healthUpgrade.GetComponent<PaymentManager>().cost += 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            healthUpgrade = other.gameObject;

            if (other.gameObject.GetComponent<PaymentManager>() != null && this.GetComponent<CoinCollection>().coins >= other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeHP = true;
                payment = other.gameObject.GetComponent<PaymentManager>().cost;
            }
            else if (this.GetComponent<CoinCollection>().coins < other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeHP = false;
            }

            /*if (isUpgrading && other.gameObject.GetComponent<PaymentManager>() != null)
            {
                other.gameObject.GetComponent<PaymentManager>().cost += 1;
                StartCoroutine(IsNotUpgrading());
                
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HealthUpgrade")
        {
            healthUpgrade = null;
            CanUpgradeHP = false;
            payment = 0;
        }
    }

}
