using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [Header("Hedefler")]
    public GameObject target; // Oyuncu karakteri
    [Space]
    public float health;
    public float range;
    public float attackDamage;
    public Animator animator;
    public Transform enemyTransform;
    public bool inRange;
    public Vector2 movement;
    [Space]
    public AudioSource deathSound;

    int attackRandomIndex;
    [HideInInspector] PlayerHealth playerHealth;
    private GameManager gameManager;

    public NavMeshAgent enemy;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        attackRandomIndex = UnityEngine.Random.Range(0, 3);
        gameManager = GameManager.instance;
    }

    public void Update()
    {
        GetTargetDistance(); // Hedef ile düşman arasındaki mesafeyi ölçer.
        
        if(!inRange)
        {
            Move(movement);
        }
        else
            Attack();
    }

    private void GetTargetDistance()        // Hedef ile düşman arasındaki mesafeyi ölçer.
    {
        inRange = Vector3.Distance(target.transform.position, enemyTransform.position) <= range;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            EnemyDead();
        }
    }
    void EnemyDead()
    {
        gameManager.ScoreUpdate(1);
        Destroy(gameObject);
    }

    public void Attack()
    {
        animator.SetInteger("AttackType", attackRandomIndex);
        animator.SetTrigger("Attack");

    }
    public void DamagePlayer() //AnimationEvent
    {
        playerHealth.DamagePlayer(attackDamage);
    }

    public void Move(Vector3 direction) // Hedef menzilde(inRange) değilse hareket eder.
    {
        enemy.SetDestination(target.transform.position);
    }
}
