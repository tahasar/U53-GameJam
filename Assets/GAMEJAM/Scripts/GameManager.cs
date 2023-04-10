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
    public GameObject warning;
    private float fillAmount = 0f;
    public bool brokenDirection = false;
    public bool brokenRotate = false;

    public void Update()
    {
        codeBar.fillAmount += 2f / 100f * Time.deltaTime;
        codeBar.color = Color.Lerp(Color.green, Color.red, codeBar.fillAmount);
        if (codeBar.fillAmount > 0.75f && !brokenDirection)
        {
            warning.SetActive(true);
            if (!brokenDirection)
            {
                brokenDirection = true;
            }
        }
         if(codeBar.fillAmount < 0.75f)
        {
            warning.SetActive(false);
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

        codeBar.fillAmount -= 2/4f;

    }
    
    public void ScoreUpdate()
    {
        scoreCount.SetText($"Skor: {this.score}");
    }
}
