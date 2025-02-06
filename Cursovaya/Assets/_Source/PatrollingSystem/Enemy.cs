using UnityEngine;
using System.Collections;
using PlayerSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 50;
    [SerializeField] private int damage = 10;
    
    [SerializeField] private Transform[] patrolPoints;
    
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float runSpeed = 4;
    
    [SerializeField] private float waitTimeAtPoint = 2;
    [SerializeField] private float detectionAngle = 45;
    [SerializeField] private float detectionRange = 5;
    [SerializeField] private float attackRange = 1;
    
    [SerializeField] private float attackCooldown = 2;
    [SerializeField] private float lostPlayerTime = 3;
    
    [SerializeField] private Transform player;
    private int currentPointIndex;
    private bool isChasing;
    private bool isAttacking;
    private float lostPlayerTimer;

    private void Awake()
    {
        StartCoroutine(Patrol());
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        DetectPlayer();
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (!isChasing)
            {
                Transform targetPoint = patrolPoints[currentPointIndex];
                MoveTowards(targetPoint.position, patrolSpeed);
                
                if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
                {
                    yield return new WaitForSeconds(waitTimeAtPoint);
                    currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                }
            }
            yield return null;
        }
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (angleToPlayer < detectionAngle && distanceToPlayer < detectionRange)
        {
            isChasing = true;
            StopCoroutine(Patrol());
            MoveTowards(player.position, runSpeed);
            
            if (distanceToPlayer < attackRange && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
        else if (isChasing)
        {
            lostPlayerTimer += Time.deltaTime;
            if (lostPlayerTimer >= lostPlayerTime)
            {
                isChasing = false;
                lostPlayerTimer = 0;
                StartCoroutine(Patrol());
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Enemy attacks!");
        player.GetComponent<Player>().TakeDamage(damage);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(0, 90f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
