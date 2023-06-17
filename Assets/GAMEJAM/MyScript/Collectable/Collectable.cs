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
    [HideInInspector] PlayerHealth playerHealth;

    [HideInInspector] KeyBoardController keyBoard;
    [HideInInspector] PlayerRotate rotateFix;
    [SerializeField] private ScaleData[] scaleDatas;
    private int index;
    public float destroyTime;

    public float duration = 2f;
    public float angle = 360f;
    public Ease easeType = Ease.Linear;





    private void Start()
    {
        keyBoard = GameObject.FindGameObjectWithTag("KeyBoard").GetComponent<KeyBoardController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        rotateFix = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerRotate>();
        gameManager = GameManager.instance;

        //Collectable-Dotween
        ScaleObj();
        RotateObj();
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("CollectArmor"))
            {
                gameManager.armorMount++;
                gameManager.armorImage[gameManager.armorMount-1].SetActive(true);
                
            }

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
            if (gameObject.CompareTag("CollectHealth"))
            {
                playerHealth.currentHealth += 100f;
            }
            gameManager.ScoreUpdate(scoreReward);
        }

        Destroy(gameObject);
    }

    void RotateObj()
    {
        transform.DORotate(new Vector3(0f, angle, 0f), duration, RotateMode.FastBeyond360)
         .SetEase(easeType)
         .SetLoops(-1, LoopType.Restart);
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
}
