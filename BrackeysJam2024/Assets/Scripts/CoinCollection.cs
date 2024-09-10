using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollection : MonoBehaviour
{
    public int coins = 0;

    public TMP_Text coinNumber;

    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinNumber.text = "Coins: " + coins;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Debug.Log("colliding with coin");
            coins += 1;
            Destroy(other.gameObject);
            soundManager.PlayCoinSound();
        }
    }
}
