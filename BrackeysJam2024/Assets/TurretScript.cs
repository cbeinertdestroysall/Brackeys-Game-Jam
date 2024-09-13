using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretScript : MonoBehaviour
{
    [SerializeField] ActivationArea area;
    [SerializeField] public int fireInterval,maxFireDelay;
    public Transform target;
    [SerializeField] GameObject EnemiesParent, bullet, TurretHead, FirePoint, HealthBarPFB;
    public List<Transform> activeEnemies = new List<Transform>();
    Canvas mainCanvas;

    [SerializeField] public int maxHP,curHP,turretRange;

    [SerializeField] float HPoffsetY;

    public bool alive = false;

    GameObject PC, BaseParent, DisabledTurrets,HealthBar;
    HealthBarWS HPB;
    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        HealthBar = Instantiate(HealthBarPFB,mainCanvas.transform);
        BaseParent = GameObject.Find("BaseParent");
        DisabledTurrets = GameObject.Find("DisabledTurrets");
        HPB = HealthBar.GetComponent<HealthBarWS>();
        HPB.WorldSpaceTarget = new Vector3(transform.position.x,transform.position.y + HPoffsetY,transform.position.z);
        HPB.meter.maxValue = maxHP;
        PC = GameObject.Find("Player");
        EnemiesParent = GameObject.Find("EnemyParent");
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            AttackSeq();
        }
        else if(!alive && area.playerInArea && PC.GetComponent<CoinCollection>().coins >= 10 && Input.GetKeyDown(KeyCode.E))
        {
            curHP = maxHP;
            area.GetComponent<MeshRenderer>().enabled = false;
            area.Prompt.SetActive(false);
            HPB.ShowBar();
            HPB.SetBarValue(curHP);
            TurretHead.SetActive(true);
            PC.GetComponent<CoinCollection>().coins -= 10;
            this.gameObject.transform.SetParent(BaseParent.transform);
            alive = true;
        }
    }
    void AttackSeq()
    {
        if (target == null && EnemiesParent.transform.childCount > 0)
        {
            activeEnemies.Clear();
            foreach (Transform t in EnemiesParent.GetComponentsInChildren<Transform>())
            {
                if(t.name != "EnemyParent")
                {
                    activeEnemies.Add(t.transform);
                }
            }
            target = SeekTarget(activeEnemies);
        }
        else if (target != null)
        {   
            RotateHead();
            
            if(fireInterval <= 0)
            {
                Instantiate(bullet, FirePoint.transform.position, Quaternion.identity, gameObject.transform);
                fireInterval = maxFireDelay;
            }
            else
            {
                fireInterval -=1; 
            }
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
            if (dSqrToTarget < closestDistanceSqr && dSqrToTarget < turretRange)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    public void TakeDamage(int DMG)
    {
        if(curHP - DMG <= 0)
        {
            gameObject.transform.SetParent(DisabledTurrets.transform);
            TurretHead.SetActive(false);
            HPB.HideBar();
            area.Prompt.SetActive(true);
            area.GetComponent<MeshRenderer>().enabled = true;
            alive = false;
        }
        else
        {
            curHP -= DMG;
            HPB.SetBarValue(curHP);
        }
    }

    void RotateHead()
    {
        TurretHead.transform.LookAt(new Vector3(target.position.x,TurretHead.transform.position.y,target.transform.position.z));
    }
}
