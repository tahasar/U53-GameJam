using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
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

    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject storySlide;
    [SerializeField] private CanvasGroup playerStats;
    [SerializeField] private GameObject anaMenu;

    public TextMeshProUGUI scoreCount;
    public int score;
    public Image codeBar;
    public GameObject warning;
    private float fillAmount = 0f;
    public bool brokenDirection = false;
    public bool brokenRotate = false;
    
    
    #region MENU

    public GameObject duraklatmaEkranı;
    public GameObject oyunSonuEkranı;
    public TextMeshProUGUI oyunSonuSkor;

    #endregion

   

    private void Start()
    {
        playerStats.alpha = 0;
        playerStats.interactable = false;
        director.Pause();
        director.stopped += OnDirectorFinished;
    }

    public void timeLineStart()
    {
        director.Play();
        storySlide.SetActive(true);    
    }


    public void Update()
    {
       
       if (storySlide.activeSelf == false)
       {
           Cursor.lockState = CursorLockMode.None;
       }
       else Cursor.lockState = CursorLockMode.Locked;


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
        if (codeBar.fillAmount < 0.75f)
        {
            warning.SetActive(false);
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
        scoreCount.SetText($"Score: {this.score}");

        codeBar.fillAmount -= 2/4f;

    }
    
    public void ScoreUpdate()
    {
        scoreCount.SetText($"Score: {this.score}");
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
        Debug.Log("fdsf");
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        oyunSonuSkor.SetText($"{score}");
        oyunSonuEkranı.SetActive(true);
    }
    
    private void OnDirectorFinished(PlayableDirector director)
    {
        // Playable Director tamamlandığında yapılması gereken işlem
        Debug.Log("Director tamamlandı. İşlem yapılabilir.");

        director.gameObject.SetActive(false);
        storySlide.SetActive(false);
        playerStats.alpha = 1;
        playerStats.interactable = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
