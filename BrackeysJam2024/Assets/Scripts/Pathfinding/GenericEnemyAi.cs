using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.VisualScripting;

public class GenericEnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    GameObject player;
    PlayerController PC;
    public Transform playerPos, baseTarget;

    public LayerMask whatIsGround, whatIsPlayer;

    public int health;
    [SerializeField] int coinsToDrop;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    //bool alreadyAttacked;
    //public GameObject projectile;
    public bool showsHP;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public List<Transform> baseList = new List<Transform>();
    [SerializeField] GameObject BaseParent,enemyHealthBarPFB;
    [SerializeField] float HPoffsetY;
    HealthBarWS HPBar;
    Canvas mainCanvas;
    GameObject eHealthBar;
    public GameObject coin;

    private void Awake()
    {
        BaseParent = GameObject.Find("BaseParent");
        player = GameObject.Find("Player");
        playerPos = player.transform;
        PC = player.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();

        if(showsHP)
        {
            mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            eHealthBar = Instantiate(enemyHealthBarPFB,mainCanvas.transform);
            HPBar = eHealthBar.GetComponent<HealthBarWS>();
            HPBar.meter.maxValue = health;
            HPBar.ShowBar();
        }
    }

    private void Update()
    {
        if(showsHP)
        {
            HPBar.WorldSpaceTarget = new Vector3(transform.position.x,transform.position.y + HPoffsetY,transform.position.z);
        }
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) TargetBase();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PC.dash)
        {
            TakeDamage(PC.RamDMG);

        }
    }

    Transform SeekTarget(List<Transform> baseList)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in baseList)
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

    /*private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }*/

    private void ChasePlayer()
    {
        if (baseTarget)
        {
            baseTarget = null;
        }
        agent.SetDestination(playerPos.position);
    }

    void TargetBase()
    {
        if (baseTarget == null)
        {
            GetNewBaseTarget();
        }
        else if (baseTarget)
        {
            if (baseTarget.parent != BaseParent)
            {
                GetNewBaseTarget();
            }
            agent.SetDestination(baseTarget.position);
        }
    }

    void GetNewBaseTarget()
    {
        baseList.Clear();
        foreach (Transform t in BaseParent.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Lighthouse" || t.name == "Turret")
            {
                baseList.Add(t.transform);
            }
        }
        baseTarget = SeekTarget(baseList);
    }

    /*private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(playerPos);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    /*private void ResetAttack()
    {
        alreadyAttacked = false;
    }*/

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(showsHP)
        {
            HPBar.SetBarValue(health);
        }

        if (health <= 0)
        {
            gameObject.GetComponent<SpawnBullet>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).GameObject().SetActive(false);
            if(showsHP)
            {
                Destroy(eHealthBar);
            }

            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        for(int i = 0; i < coinsToDrop;i++)
        {
            Instantiate(coin, new Vector3(transform.position.x + Random.Range(-2, 2),transform.position.y,transform.position.z +Random.Range(-2, 2)), Quaternion.identity);   
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
