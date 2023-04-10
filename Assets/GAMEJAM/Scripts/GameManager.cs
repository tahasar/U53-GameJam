using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    #region menüler

    public GameObject duraklatmaEkranı;
    public GameObject oyunSonuEkranı;
    public TextMeshProUGUI oyunSonuSkor;

    #endregion

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

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            duraklatmaEkranı.SetActive(true);
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

    public void OyunaDevamEt()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        duraklatmaEkranı.SetActive(false);
    }
    
    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("anaMenu");
        Time.timeScale = 1;
    }
    
    public void OyunSonuEkranı()
    {
        oyunSonuEkranı.SetActive(true);
    }

    public void Cikis()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        oyunSonuSkor.SetText($"{score}");
        oyunSonuEkranı.SetActive(true);
    }
}
