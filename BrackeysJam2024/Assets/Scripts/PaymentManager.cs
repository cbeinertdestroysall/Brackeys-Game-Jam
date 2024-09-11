using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public int cost;
    public bool canShow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canShow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canShow = false;
        }
    }
}
