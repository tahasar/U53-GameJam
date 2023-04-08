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
    
    [SerializeField] Vector3 spawnArea;
    public GameObject player;

    private void Awake()
    {
        CalculateWeights();
    }

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        
        InvokeRepeating("SpawnRandomEnemy", 2, spawnTime);
        
    }

    private void SpawnRandomEnemy()
    {
        EnemyProbabilities randomEnemy = enemies[GetRandomEnemyIndex()];

        Instantiate(randomEnemy.Prefab, GenerateRandomPosition(), Quaternion.identity, transform);
    }

    private Vector3 GenerateRandomPosition()
    {
        var position = new Vector3();

        //float f = Random.value > 0.5f ? -1f : 1f;
        //if (Random.value > 0.0f)
        //{
        //    position.x = Random.Range(-spawnArea.x, spawnArea.x);
        //    //position.z = spawnArea.z * f;
        //    Debug.Log("1");
        //}
        //else
        //{
        //    position.z = Random.Range(-spawnArea.z, spawnArea.z);
        //    //position.x = spawnArea.x * f;
        //    Debug.Log("2");
        //}
        
        position.x = Random.Range(-spawnArea.x,spawnArea.x);
        position.z = Random.Range(-spawnArea.z,spawnArea.z);

        
        Vector3 result = new Vector3(position.x, position.y, position.z);

        //position += (Vector3)player.transform.position;
        
        return result;
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
