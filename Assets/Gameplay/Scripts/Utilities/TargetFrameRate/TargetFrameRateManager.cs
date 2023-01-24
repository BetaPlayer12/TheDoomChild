using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRateManager : MonoBehaviour
{

    public int mobileTargetFrameRate = 30;

    void Start()
    {
        #if UNITY_IOS  || UNITY_ANDROID
            SetTargetFrameRate(mobileTargetFrameRate);
        #endif
    }

    void SetTargetFrameRate(int newFrameRate)
    {
        Application.targetFrameRate = newFrameRate;        

    }
}
