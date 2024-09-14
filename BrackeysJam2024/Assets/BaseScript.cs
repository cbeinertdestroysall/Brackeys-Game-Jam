using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour
{
    public GameObject HealthBar;
    HealthBar HPbar;
    TMP_Text HealthText;
    [SerializeField] public int curHp, MaxHp,collisionDamage;
    [SerializeField] GameStart GM;
    public bool gameOver = false;
    [SerializeField] AudioSource SFX;
    // Start is called before the first frame update
    void Start()
    {
        HPbar = HealthBar.GetComponent<HealthBar>();
        HealthText = HealthBar.transform.GetChild(0).GetComponent<TMP_Text>();
        curHp = MaxHp;
        HPbar.slider.maxValue = MaxHp;
        HealthText.text = "Light House: " + curHp + "/" + MaxHp;
        HPbar.SetHealth(curHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeathAudio()
    {
        SFX.Play();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(curHp - collisionDamage <= 0 && !gameOver)
            {
                gameOver = true;
                GM.EndGame();
                Invoke("DeathAudio",2f);
                curHp = 0;
                HealthText.text = "The Lighthouse Is Lost!";
                HPbar.SetHealth(curHp);
            }
            else if(!gameOver)
            {
                other.GetComponent<GenericEnemyAi>().TakeDamage(10);
                curHp -= collisionDamage;
                HealthText.text = "Light House: " + curHp + "/" + MaxHp;
                HPbar.SetHealth(curHp);
            }
        }
    }
}
