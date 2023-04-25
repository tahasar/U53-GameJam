using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyBase : ScriptableObject
{
    public string enemyName;
    public string enemyInfo;
    public float health;
    public float maxHealth;
    public float attackDamage;
    public float range;
    public float speed;
    public float speedMultiplier = 1;
    public float xpReward;
    public Sprite sprite;
    public Animator animator;
}
