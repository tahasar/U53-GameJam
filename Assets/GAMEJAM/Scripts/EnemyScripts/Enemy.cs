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

    public NavMeshAgent enemy;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        GetTargetDistance(); // Hedef ile düşman arasındaki mesafeyi ölçer.
        
        if(!isAttacking)
        {
            Move(movement);
        }

        if (isDead)
        {
            deathSound.Play();
            //dropOnDestroy.Drop();
            Destroy(gameObject);
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
            Destroy(characterForm);
            collider.enabled = false;
            isDead = true;
        }
    }

    public void Attack()
    {
        if(inRange)
        {
            //karaktere hasar verecek.
        }
    }

    public void Move(Vector3 direction) // Hedef menzilde(inRange) değilse hareket eder.
    {
        if (inRange)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            //enemyTransform.position = ((Vector3)enemyTransform.position + (direction * (speed * Time.deltaTime)));
            enemy.SetDestination(target.transform.position);
        }
    }
}
