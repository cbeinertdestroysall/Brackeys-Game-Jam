using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource coinCollect;
    public AudioSource enemyKill;
    public AudioSource hurt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCoinSound()
    {
        coinCollect.Play();
        
    }

    public void PlayEnemyKill()
    {
        enemyKill.Play();
    }

    public void PlayHurt()
    {
        hurt.Play();
    }
}
