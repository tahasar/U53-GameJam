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
    
    [SerializeField] private EnemyProbabilities[] enemies;

    [SerializeField] private bool isNight = true;
    
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    
    [SerializeField] Vector2 spawnArea;
    GameObject player;

    private void Awake()
    {
        CalculateWeights();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (isNight)
        {
            InvokeRepeating("SpawnRandomEnemy", 2, spawnTime);
        }
    }

    private void SpawnRandomEnemy()
    {
        EnemyProbabilities randomEnemy = enemies[GetRandomEnemyIndex()];

        Instantiate(randomEnemy.Prefab, GenerateRandomPosition(), Quaternion.identity, transform);
    }

    private Vector2 GenerateRandomPosition()
    {
        Vector2 position = new Vector2();

        float f = Random.value > 0.5f ? -1f : 1f;
        if (Random.value > 0.5f)
        {
            position.x = Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        }
        else
        {
            position.y = Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;
        }
        
        position += (Vector2)player.transform.position;
        
        return position;
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
