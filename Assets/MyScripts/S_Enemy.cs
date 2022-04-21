using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class S_Enemy : MonoBehaviour
{
    
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    public int damage;
    public int EnemyDamage;
    [SerializeField] GameObject vfxEnemyExplosion;
    [SerializeField] GameObject PlayerHealthBar;
    public S_PlayerHealth _PlayerHealth;
    public S_EnemyHealth _EnemyHealth;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject bullet;
    public GameObject bulletSpawnPosition;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInAttackRange && !playerInSightRange) Func_Patroling();
        if (!playerInAttackRange && playerInSightRange) Func_ChasePlayer();
        if (playerInAttackRange && playerInSightRange) Func_AttackPlayer();

    }

    private void Func_Patroling()
    {
        if(!walkPointSet)
        {
            Func_SearchWalkPoint();
        }
        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void Func_SearchWalkPoint()
    {
        // Calculate Random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) // Check if the walkPoint is on the Ground
        {
            walkPointSet = true;
        }
    }

    private void Func_ChasePlayer()
    {
        agent.SetDestination(player.position );
    }

    private void Func_AttackPlayer()
    {
        // Enemy DON'T MOVE
        agent.SetDestination(transform.position);

        // Enemy Look to Player
        transform.LookAt(player);

        if(!alreadyAttacked) // If don't attacked yet
        {

            //Attack code here
            Rigidbody rb = Instantiate(bullet, bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse); // For Parabula


            alreadyAttacked = true;
            PlayerHealthBar.GetComponent<Slider>().value -= EnemyDamage;
            if(PlayerHealthBar.GetComponent<Slider>().value <= 0)
            {
                _PlayerHealth.Func_HeroDeath();
            }
            Invoke(nameof(Func_ResetAttack), timeBetweenAttacks); // Attack with timeBetweenAttacks delay
        }
    }

    private void Func_ResetAttack()
    {
        alreadyAttacked = false; 
        
    }

    // Apply damage to Enemy
    private void Func_TakeDamage(int damage)
    {
        health -= damage; // Update enemy Health
        _EnemyHealth.GetComponent<Slider>().value = health;
        if (health <= 0) Invoke(nameof(Func_DestroyEnemy), .5f); // Call Death 
    }

    //On Bullet Touch Enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<S_ShootController>() != null) // If HAVE
        {
            Func_TakeDamage(damage);
        }
    }

    // Enemy Death
    private void Func_DestroyEnemy()
    {
        Vector3 enemyPosition = gameObject.transform.position;
        Quaternion enemyRotation = Quaternion.identity;

        GameObject vfxExplosion; // Create vfx Game Object || Temporary Object to Assign Spawned Particle System to Destroy it after Done with Animation

        Destroy(gameObject); // Destroy himself (Enemy)
        vfxExplosion = Instantiate(vfxEnemyExplosion, enemyPosition, enemyRotation);

        Destroy(vfxExplosion, 15f);
    }

}
