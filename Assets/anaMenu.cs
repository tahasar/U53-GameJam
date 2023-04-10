using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anaMenu : MonoBehaviour
{
    public void oyunuBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void oyundanCik()
    {
        Application.Quit();
        Debug.Log("Çýktýn");
    }

    public void emekMenusune()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


}
