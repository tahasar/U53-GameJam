using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anaMenu : MonoBehaviour
{
    public void OyunuBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OyundanCik()
    {
        Application.Quit();
        Debug.Log("��kt�n");
    }

    public void EmekMenusune()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


}
