using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    [Header("Hedefler")]
    public GameObject target; // Oyuncu karakteri
    [Space]

    [Header("ENEMY")]
    public float health;
    public float range;
    public float attackDamage;
    public int scoreReward;
    public bool inRange;
    public float turnSpeed;
    bool isDead = false;

    [Header("ANİM")]
    public Animator animator;
    int attackRandomIndex;

    [SerializeField] Vector2 movement;

    [HideInInspector] PlayerHealth playerHealth;
    private GameManager gameManager;
    NavMeshAgent enemyAgent;

    #region Ragdoll
    Rigidbody[] rigRagdoll;
    Collider[] colRagdoll;
    bool ragdollMode = false;
    #endregion

    #region SoundEffects

    public AudioClip[] enemyDeadSesleri;
    private AudioSource audio;

    #endregion

    public bool attackMode = false;
    private Collider collider;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyAgent = GetComponent<NavMeshAgent>();

        audio = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();

        //attackRandomIndex = UnityEngine.Random.Range(0, 3);
        gameManager = GameManager.instance;

        GetRagdollbits();
        RagdollModeOff();
    }

    public void Update()
    {
        GetTargetDistance(); // Hedef ile düşman arasındaki mesafeyi ölçer.

        if (!isDead)
        {
            if (!inRange && !ragdollMode)
            {
                Move(movement);
            }
            else
                Attack();
        }
       
    }

    private void GetTargetDistance()        // Hedef ile düşman arasındaki mesafeyi ölçer.
    {
        inRange = Vector3.Distance(target.transform.position, transform.position) <= range;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;


        if (health <= 0f )
        {
            isDead = true;
            health = 0f;
            EnemyDead();
        }
    }
    void EnemyDead()
    {
        audio.clip = enemyDeadSesleri[Random.Range(0, 3)];
        audio.Play();
        gameManager.score += scoreReward;
        gameManager.ScoreUpdate();
        Destroy(gameObject, 2f);
        enemyAgent.updatePosition = false;
        enemyAgent.updateRotation = false;
    }

    public void Attack()
    {
        animator.SetInteger("AttackType", Random.Range(0,2));
        animator.SetBool("isAttack", true);

        Vector3 direction = target.transform.position - transform.position;//bakıcağımız pozisyonu belirledik
        direction.y = 0;//yukarıya ve aşağıya bakamıcagı için 0'a eşitledik

        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);//yönümüzü player a çevirdik

        enemyAgent.updatePosition = false;
      

    }
    public void DamagePlayer() //AnimationEvent
    {
        playerHealth.DamagePlayer(attackDamage);
    }

    public void Move(Vector3 direction) // Hedef menzilde(inRange) değilse hareket eder.
    {
        if(!attackMode){
            enemyAgent.SetDestination(target.transform.position);
            enemyAgent.updatePosition = true;
            enemyAgent.updateRotation = true;
        }
        
        animator.SetBool("isAttack", false);
    }

    #region ****RAGDOLL****
    void GetRagdollbits()
    {
        rigRagdoll = GetComponentsInChildren<Rigidbody>();
        colRagdoll = GetComponentsInChildren<Collider>();
    }

    void RagdollModeOn()
    {
        ragdollMode = true;

        for (int i = 0; i < rigRagdoll.Length; i++)
        {
            Rigidbody rig = rigRagdoll[i];
            rig.isKinematic = false;

        }
        for (int i = 0; i < colRagdoll.Length; i++)
        {
            Collider col = colRagdoll[i];
            col.enabled = true;
        }
        animator.enabled = false;
        enemyAgent.enabled = false;
        enemyAgent.updatePosition = false;
        enemyAgent.updateRotation = false;
      
    }

    void RagdollModeOff()
    {
        for (int i = 0; i < rigRagdoll.Length; i++)
        {
            Rigidbody rig = rigRagdoll[i];
            rig.isKinematic = true;

        }
        for (int i = 0; i < colRagdoll.Length; i++)
        {
            Collider col = colRagdoll[i];
            col.enabled = true;
        }
        //zombieHealth.RagdollCollider();       //xd
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        RagdollModeOn();

        Rigidbody hitRigidbody = rigRagdoll.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);


    }
    #endregion
}
