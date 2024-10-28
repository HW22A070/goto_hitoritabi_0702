using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// 時間倍率
    /// </summary>
    public static void ChangeTimeValue(float timeValue)
    {
        if (timeValue < 0) Time.timeScale = 0.0f;
        else Time.timeScale = timeValue;
    }
}
