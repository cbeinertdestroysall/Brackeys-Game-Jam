using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBase : MonoBehaviour
{
    [SerializeField] TurretScript TS;

    void OnTriggerEnter(Collider other)
    {
        if(TS.alive)
        {
            if(other.tag == "Enemy")
        {
            TS.TakeDamage(25);
        }
        else if(other.tag == "EnemyBullet")
        {
            TS.TakeDamage(1);
        }
        }
    }
}
