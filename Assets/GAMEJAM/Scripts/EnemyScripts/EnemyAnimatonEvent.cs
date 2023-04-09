using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatonEvent : MonoBehaviour
{
    [HideInInspector] Enemy enemyAnimationEvent;


    public void Start()
    {
        enemyAnimationEvent = GameObject.FindGameObjectWithTag("Enemy").GetComponentInParent<Enemy>();
    }

    void AttackDamage()
    {
        enemyAnimationEvent.DamagePlayer();
    }


}
