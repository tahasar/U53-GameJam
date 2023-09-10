using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using GAMEJAM.MyScript;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
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
    public GameObject riglayers;
    public bool brokenDirection = false;
    public bool brokenRotate = false;
    public bool storyActive;
    public bool menuActive;

    #region MENU

    public GameObject duraklatmaEkranı;
    public GameObject oyunSonuEkranı;
    public TextMeshProUGUI oyunSonuSkor;
    public GameObject storyImage;

    #endregion

    [SerializeField] PlayerController playerMove;
    [SerializeField] KeyBoardController keyboard;

    [SerializeField] AudioSource aSource;
    [SerializeField] AudioClip aClipBackGround;
    [SerializeField] AudioClip aClip;

    private void Start()
    {
        playerStats.alpha = 0;
        playerStats.interactable = false;
        director.Pause();
        director.stopped += OnDirectorFinished;
        storyActive = true;

        playerMove.enabled = false;
        keyboard.enabled = false;
    }

    public void TimeLineStart()
    {
        director.Play();
        storySlide.SetActive(true);
        storyActive = true;
        storyImage.SetActive(true);
    }


    public void Update()
    {
       
       if (storySlide.activeSelf == false &&storyActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("none");
        }
        else if (!storyActive &&!menuActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


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

        if (Input.GetKeyDown(KeyCode.Escape) && !storyActive || Input.GetKeyDown(KeyCode.Tab) && !storyActive)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            duraklatmaEkranı.SetActive(true);
            menuActive = true;
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
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
    
    private void OnDirectorFinished(PlayableDirector director)
    {
        director.gameObject.SetActive(false);
        storySlide.SetActive(false);
        playerStats.alpha = 1;
        playerStats.interactable = true;

        Cursor.lockState = CursorLockMode.Locked;
        storyActive = false;

        riglayers.SetActive(true);
        playerMove.enabled = true;
        keyboard.enabled = true;
        storyImage.SetActive(false);
        aSource.PlayOneShot(aClip);
    }

}
