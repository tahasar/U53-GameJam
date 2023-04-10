using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;
   


    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion

    public TextMeshProUGUI scoreCount;
    public int score;
    public Image codeBar;
    public bool brokenDirection = false;
    public bool brokenRotate = false;

    public void Update()
    {
        codeBar.fillAmount += 2f / 100f * Time.deltaTime;
        if (codeBar.fillAmount > 0.75f && !brokenDirection)
        {
            brokenDirection = true;
        }
        if (codeBar.fillAmount > 0.99f && !brokenRotate)
        {
            brokenRotate = true;
        }
    }

    public void ScoreUpdate(int score)
    {
        this.score += score;
        scoreCount.SetText($"Skor: {this.score}");

        codeBar.fillAmount -= 3/4f;

    }
    
    public void ScoreUpdate()
    {
        scoreCount.SetText($"Skor: {this.score}");
    }
}
