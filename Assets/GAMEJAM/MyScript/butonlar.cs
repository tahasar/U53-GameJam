using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class butonlar : MonoBehaviour
{
    public void geriDonme()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
    }

    public void seydaLink()
    {
        Application.OpenURL("https://www.linkedin.com/in/seydarslan/");
    }

    public void tahaLink()
    {
        Application.OpenURL("https://www.linkedin.com/in/tahasar");
    }

    public void huseyinLink()
    {
        Application.OpenURL("https://www.linkedin.com/in/hsynyldrm/");
    }

    public void haticeLink()
    {
        Application.OpenURL("https://www.linkedin.com/in/hatice-altunbay-300218170");
    }

}
