using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{

   [SerializeField] GameManager gameManager;
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (gameManager.storyActive)
        {
            anim.SetBool("Menu", true);
        }
        else
        {
            anim.SetBool("Menu", false);
        }
    }
}
