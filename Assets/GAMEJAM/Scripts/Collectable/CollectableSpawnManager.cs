using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using Random = UnityEngine.Random;

[Serializable]
public class CollectableProbabilities
{
    public string Name;

    public GameObject Prefab;
    [Range(0f, 100f)] public float chance = 100;

    [HideInInspector] public double _weight;
}

public class CollectableSpawnManager : MonoBehaviour
{
    public float spawnTime;
    
    [SerializeField] private CollectableProbabilities[] codes;
    
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    
    [SerializeField] Vector3 spawnArea;

    private void Awake()
    {
        CalculateWeights();
    }

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        
        InvokeRepeating("SpawnRandomCode", 0f, spawnTime);

    }

    private void SpawnRandomCode()
    {
        CollectableProbabilities randomEnemy = codes[GetRandomCodeIndex()];

        Instantiate(randomEnemy.Prefab, GenerateRandomPosition(), Quaternion.identity, transform);
        
        Debug.Log(GameManager.instance.score);
    }

    private Vector3 GenerateRandomPosition()
    {
        var position = new Vector3();
        
        position.x = Random.Range(-spawnArea.x,spawnArea.x);
        position.z = Random.Range(-spawnArea.z,spawnArea.z);

        
        Vector3 result = new Vector3(position.x, position.y, position.z);

        //position += (Vector3)player.transform.position;
        
        return result;
    }

    private int GetRandomCodeIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < codes.Length; i++)
        {
            if (codes[i]._weight >= r)
                return i;
        }

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (CollectableProbabilities enemy in codes)
        {
            accumulatedWeights += enemy.chance;
            enemy._weight = accumulatedWeights;
        }
    }
}
