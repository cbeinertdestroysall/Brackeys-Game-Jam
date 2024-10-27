using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public int cost;
    public bool canShow;
    [SerializeField] AudioClip PopIn,PopOut;
    [SerializeField] AudioSource SFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !canShow)
        {
            SFX.clip = PopIn;
            SFX.Play();
            canShow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && canShow)
        {
            SFX.clip = PopOut;
            SFX.Play();
            canShow = false;
        }
    }
}
