using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrading : MonoBehaviour
{
    public bool upgradeHealth = false;

    public HealthBar healthBar;

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
                this.GetComponent<PlayerHealth>().maxHealth += (this.GetComponent<PlayerHealth>().maxHealth * 0.1f);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "HealthUpgrade")
        {
            upgradeHealth = true;
        }
    }

}
