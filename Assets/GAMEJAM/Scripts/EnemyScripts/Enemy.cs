using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [Header("Hedefler")]
    public GameObject target; // Oyuncu karakteri
    [Space]

    [Header("ENEMY")]
    public float health;
    public float range;
    public float attackDamage;
    public bool inRange;
<<<<<<< Updated upstream
    public float turnSpeed;
    bool isDead = false;

    [Header("ANİM")]
    public Animator animator;
    int attackRandomIndex;

    [SerializeField] Vector2 movement;
=======
    public Vector2 movement;
    public float turnSpeed;
>>>>>>> Stashed changes
    [Space]
    public AudioSource deathSound;

    [HideInInspector] PlayerHealth playerHealth;
    private GameManager gameManager;
    NavMeshAgent enemyAgent;

<<<<<<< Updated upstream
=======
    public NavMeshAgent enemyNav;

>>>>>>> Stashed changes
    #region Ragdoll
    Rigidbody[] rigRagdoll;
    Collider[] colRagdoll;
    bool ragdollMode = false;
<<<<<<< Updated upstream
    #endregion
=======
    public GameObject hipsRb;

    #endregion

>>>>>>> Stashed changes

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
<<<<<<< Updated upstream
        enemyAgent = GetComponent<NavMeshAgent>();
=======
        enemyNav = GetComponent<NavMeshAgent>();
>>>>>>> Stashed changes

        //attackRandomIndex = UnityEngine.Random.Range(0, 3);
        gameManager = GameManager.instance;

        GetRagdollbits();
        RagdollModeOff();
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes
    }

    public void Update()
    {
       /* Rigidbody rb = hipsRb.GetComponent<Rigidbody>();
        rb.freezeRotation.
       */


        GetTargetDistance(); // Hedef ile düşman arasındaki mesafeyi ölçer.

<<<<<<< Updated upstream
        if (!isDead)
        {
            if (!inRange && !ragdollMode)
            {
                Move(movement);
            }
            else
                Attack();
=======

        if(!inRange && !ragdollMode)
        {
            Move(movement);
           
>>>>>>> Stashed changes
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
        gameManager.ScoreUpdate(1);
<<<<<<< Updated upstream
        Destroy(gameObject, 2f);
        enemyAgent.updatePosition = false;
        enemyAgent.updateRotation = false;
=======
        Destroy(gameObject,2f);
>>>>>>> Stashed changes
    }

    public void Attack()
    {
<<<<<<< Updated upstream
        //animator.SetInteger("AttackType", attackRandomIndex);
        animator.SetBool("isAttack", true);

        Vector3 direction = target.transform.position - transform.position;//bakıcağımız pozisyonu belirledik
        direction.y = 0;//yukarıya ve aşağıya bakamıcagı için 0'a eşitledik

        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);//yönümüzü player a çevirdik

        enemyAgent.updatePosition = false;
      
=======
        animator.SetInteger("AttackType", attackRandomIndex);
        animator.SetBool("isAttack", true);
        animator.SetBool("isRun", false);

        enemyNav.updatePosition = false;
        enemyNav.updateRotation = false;
>>>>>>> Stashed changes

    }
    public void DamagePlayer() //AnimationEvent
    {
        playerHealth.DamagePlayer(attackDamage);
    }

    public void Move(Vector3 direction) // Hedef menzilde(inRange) değilse hareket eder.
    {
<<<<<<< Updated upstream
        enemyAgent.SetDestination(target.transform.position);
        enemyAgent.updatePosition = true;
        enemyAgent.updateRotation = true;

        animator.SetBool("isAttack", false);
    }

    #region ****RAGDOLL****
=======
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", true);
        enemyNav.SetDestination(target.transform.position);
        enemyNav.updatePosition = true;
        enemyNav.updateRotation = true;

        direction = target.transform.position - transform.position;//bakıcağımız pozisyonu belirledik
        direction.y = 0;//yukarıya ve aşağıya bakamıcagı için 0'a eşitledik

        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);


    }
    #region Ragdoll
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        enemyAgent.enabled = false;
        enemyAgent.updatePosition = false;
        enemyAgent.updateRotation = false;
      
=======
        enemyNav.enabled = false;
        enemyNav.updatePosition = false;
        enemyNav.updateRotation = false;
       
       
>>>>>>> Stashed changes
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
