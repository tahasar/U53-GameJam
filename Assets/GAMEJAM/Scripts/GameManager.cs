using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public void ScoreUpdate(int score)
    {
        this.score += score;
        scoreCount.SetText($"Skor: {this.score}");
    }
}
