using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningArea : MonoBehaviour
{
    [SerializeField] private EnemySpawnManager spawnManager1 = null;
    [SerializeField] private EnemySpawnManager spawnManager2 = null;
    [SerializeField] private EnemySpawnManager spawnManager3 = null;

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("a");
        if (other.gameObject.CompareTag("Player"))
        {        
            Debug.Log("b");
            spawnManager1.SpawnRandomEnemy();
            spawnManager2.SpawnRandomEnemy();
            spawnManager3.SpawnRandomEnemy();
        }
    }
}
