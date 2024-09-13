using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBase : MonoBehaviour
{
    [SerializeField] TurretScript TS;
    [SerializeField] int DMGfromBullet,DMGfromCollision;

    void OnTriggerEnter(Collider other)
    {
        if (TS.alive)
        {
            if (other.tag == "Enemy")
            {
                TS.TakeDamage(DMGfromCollision);
            }
            else if (other.tag == "EnemyBullet")
            {
                TS.TakeDamage(DMGfromBullet);
            }
        }
    }
}
