using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAnimationEvent : MonoBehaviour
{
    KeyBoardController keyBoardAnim;

    public void Start()
    {
        keyBoardAnim = GetComponentInChildren<KeyBoardController>();


    }

    void PushAnim()
    {
        keyBoardAnim.AttackPush();
    }
}
