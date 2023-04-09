using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Collectable : MonoBehaviour
{
    public int scoreReward;
    private GameObject player;
    private LookAtConstraint _lookAtConstraint;
    private ConstraintSource playerConstraintSource;
    private GameManager gameManager;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        transform.LookAt(player.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.ScoreUpdate(scoreReward);
        }
        
        Destroy(gameObject);
    }
    
    //private void BirŞeyYap();
}
