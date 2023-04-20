using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyProbabilities
{
    public string Name;

    public GameObject Prefab;
    [Range(0f, 100f)] public float chance = 100;

    [HideInInspector] public double _weight;
}

public class EnemySpawnManager : MonoBehaviour
{
    public float spawnTime;
    CapsuleCollider colRadius;
    
    [SerializeField] private EnemyProbabilities[] enemies;
    GameManager gameManager;
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    
    private void Awake()
    {
        CalculateWeights();

    }

    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", 0f, spawnTime);
        colRadius = GetComponent<CapsuleCollider>();

        gameManager = GameManager.instance;
    }

    private void SpawnRandomEnemy()
    {
        EnemyProbabilities randomEnemy = enemies[GetRandomEnemyIndex()];
      
        Vector3 spawnPoint = Random.insideUnitSphere * colRadius.radius + transform.position;
        spawnPoint.y = 0f;

        Instantiate(randomEnemy.Prefab, spawnPoint, Quaternion.identity);

        //SpawnSettings
        if (spawnTime > 1) spawnTime -= 0.03f;

        if (enemies[2].chance !<=100f) enemies[2].chance += 1f;

        if (enemies[1].chance !<= 100f) enemies[1].chance += 1.5f;
        
        if (enemies[0].chance !>= 5f) enemies[0].chance -= 1f;

    }

    private int GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i]._weight >= r)
                return i;
        }

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (EnemyProbabilities enemy in enemies)
        {
            accumulatedWeights += enemy.chance;
            enemy._weight = accumulatedWeights;
        }
    }
}
