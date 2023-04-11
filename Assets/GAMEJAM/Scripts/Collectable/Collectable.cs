using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;
public class Collectable : MonoBehaviour
{
    public int scoreReward;
    private GameObject player;
    private GameManager gameManager;

    [HideInInspector] KeyBoardController keyBoard;
    [HideInInspector] PlayerRotate rotateFix;
    [SerializeField] private ScaleData[] scaleDatas;
    private int index;

    private void Start()
    {
        keyBoard = GameObject.FindGameObjectWithTag("KeyBoard").GetComponent<KeyBoardController>();
        player = GameObject.FindGameObjectWithTag("Player");
        rotateFix = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerRotate>();
        gameManager = GameManager.instance;

        ScaleObj(); //Collectable-Dotween
    }

    private void Update()
    {
        transform.LookAt(player.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("CollectKey"))
            {
                gameManager.brokenDirection = false;
            }

            if (gameObject.CompareTag("CollectMouse"))
            {
                gameManager.brokenRotate = false;
            }
            if (gameObject.CompareTag("CollectKeyBoard"))
            {
                keyBoard.Reload();
                keyBoard.KeyBoardFix();
                keyBoard.carriedAmmo += 103 - keyBoard.keysCount;
                keyBoard.keysCount = 103;
            }


            gameManager.ScoreUpdate(scoreReward);
        }

        Destroy(gameObject);
    }

    void ScaleObj()
    {
        if ( index >= scaleDatas.Length )
        {
            index = 0;
        }
        transform.DOScale(scaleDatas[index].size, scaleDatas[index].time).OnComplete(() => ScaleObj());
        index++;
    }


    [System.Serializable]
    public struct ScaleData
    {
        public float size;
        public float time;
    }


    //private void BirŞeyYap();
}
