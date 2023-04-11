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
    public Vector3 spawnAreaCenter;
    public float spawnAreaRadius = 5f;

    [SerializeField] private EnemyProbabilities[] enemies;
    
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    
    private void Awake()
    {
        CalculateWeights();
    }

    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", 0f, spawnTime);
    }

    private void SpawnRandomEnemy()
    {
        EnemyProbabilities randomEnemy = enemies[GetRandomEnemyIndex()];
      
        Vector3 spawnPoint = Random.insideUnitSphere * spawnAreaRadius + spawnAreaCenter;
        spawnPoint.y = 0f;
       
        Instantiate(randomEnemy.Prefab, spawnPoint, Quaternion.identity);

        //Instantiate(randomEnemy.Prefab, GenerateRandomPosition(), Quaternion.identity, transform);
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
    /*private Vector3 GenerateRandomPosition()
   {
       var position = new Vector3();
       position.x = Random.Range(-spawnArea.x,spawnArea.x);
       position.y = spawnArea.y;
       position.z = Random.Range(-spawnArea.z,spawnArea.z);


       Vector3 result = new Vector3(position.x, position.y, position.z);

       return result;
   }*/
}
