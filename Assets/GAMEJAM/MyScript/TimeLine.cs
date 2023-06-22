using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    
    void TimeLineStop()
    {

        Time.timeScale = 0f;
    }

    void TimeLinePlay()
    {
        Time.timeScale = 1f;
    }
}
