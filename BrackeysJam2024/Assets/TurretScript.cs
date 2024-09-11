using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretScript : MonoBehaviour
{
    [SerializeField] ActivationArea area;
    Transform target;
    [SerializeField] GameObject EnemiesParent, bullet, TurretHead, FirePoint;
    GameObject projectile;
    public List<Transform> activeEnemies = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        EnemiesParent = GameObject.Find("EnemyParent");
    }

    IEnumerator FireProjectile()
    {
        Instantiate(bullet, FirePoint.transform.position, Quaternion.identity, this.gameObject.transform);
        yield return new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
            if (target == null)
            {
                foreach (Transform t in EnemiesParent.GetComponentsInChildren<Transform>())
                {
                    activeEnemies.Add(t.transform);
                }

                if(activeEnemies.Count > 0)
                {
                    target = SeekTarget(activeEnemies);
                }
                else
                {
                    
                }
            }
            else
            {
                RotateHead();
            }
        
    }

    Transform SeekTarget(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    void RotateHead()
    {
        Vector3 direction = (target.position - transform.position).normalized;          //Calculate the direction from the enemy to the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;            //:3
        TurretHead.transform.rotation = Quaternion.Euler(0, 0, angle);                             //Rotate enemy to face player
    }
}
