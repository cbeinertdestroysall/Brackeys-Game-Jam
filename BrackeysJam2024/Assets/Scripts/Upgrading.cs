using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
public class Upgrading : MonoBehaviour
{
    public bool CanUpgradeHP = false;
    public bool CanUpgradeSpeed = false;

    public HealthBar healthBar;

    int payment;

    [SerializeField] PlayerController PC;

    public SoundManager soundManager;

    public float healthUpgradeAmount;
    public int speedUpgradeAmount;

    public int costIncreaseAmount;

    GameObject healthUpgrade;
    GameObject speedUpgrade;

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
            Debug.Log("registered health upgrade input");
            UpgradeHP();
        }
        else if (Input.GetKeyDown(KeyCode.E) && CanUpgradeSpeed == true)
        {
            Debug.Log("registered speed upgrade input");
            UpgradeSpeed();
        }
    }

    void UpgradeHP()
    {
        //Debug.Log("increase health");
        this.GetComponent<PlayerHealth>().maxHealth += healthUpgradeAmount;
        this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
        healthBar.SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
        this.GetComponent<CoinCollection>().coins -= payment;
        soundManager.PlayUpgrade();

        if (healthUpgrade.GetComponent<PaymentManager>() != null)
        {
            healthUpgrade.GetComponent<PaymentManager>().cost += costIncreaseAmount;
        }
    }

    void UpgradeSpeed()
    {
        //Debug.Log("increase health");
        this.GetComponent<PlayerController>().maxDash += speedUpgradeAmount;
        //this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
        //healthBar.SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
        PC.DM.meter.maxValue = PC.maxDash;
        this.GetComponent<CoinCollection>().coins -= payment;
        soundManager.PlayUpgrade();

        if (speedUpgrade.GetComponent<PaymentManager>() != null)
        {
            speedUpgrade.GetComponent<PaymentManager>().cost += costIncreaseAmount;
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
        else if (other.gameObject.tag == "SpeedUpgrade")
        {
            speedUpgrade = other.gameObject;

            if (other.gameObject.GetComponent<PaymentManager>() != null && this.GetComponent<CoinCollection>().coins >= other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeSpeed = true;
                payment = other.gameObject.GetComponent<PaymentManager>().cost;
            }
            else if (this.GetComponent<CoinCollection>().coins < other.gameObject.GetComponent<PaymentManager>().cost)
            {
                CanUpgradeSpeed = false;
            }
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
        else if (other.gameObject.tag == "SpeedUpgrade")
        {
            speedUpgrade = null;
            CanUpgradeSpeed = false;
            payment = 0;
        }
    }

}
