using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [Header("Hedefler")]
    public GameObject target; // Oyuncu karakteri
    
    public float health;
    public float range;
    public float attackDamage;
    public Animator animator;
    public string enemyName;
    public string enemyInfo;
    public float maxHealth;
    public float speed;
    public Transform enemyTransform;
    public bool inRange;
    public bool isDead = false;
    public Vector2 movement;
    public AudioSource deathSound;
    public Collider collider;
    public GameObject characterForm;

    public bool isAttacking;
    int attackRandomIndex;
    [HideInInspector] PlayerHealth playerHealth;

    public NavMeshAgent enemy;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        attackRandomIndex = UnityEngine.Random.Range(0, 3);
    }

    public void Update()
    {
        GetTargetDistance(); // Hedef ile düşman arasındaki mesafeyi ölçer.
        
        if(!inRange)
        {
            Move(movement);
            animator.SetBool("isAttack", false);
        }
        else
            Attack();
       
        if (health <= 0)
        {
            //deathSound.Play();
            //dropOnDestroy.Drop();
            EnemyDead();
            isDead = false;
        }
      
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
            Destroy(gameObject);
        }
    }
    void EnemyDead()
    {
        Destroy(gameObject);
    }

    public void Attack()
    {
        animator.SetInteger("Attack", attackRandomIndex);
        animator.SetBool("isAttack", true);

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
