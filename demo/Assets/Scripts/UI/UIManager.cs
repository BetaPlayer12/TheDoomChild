using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UIManager is Null!!!");
            }
            return _instance;
        }
    }

    public Text gemCountText;
    public Image[] healthBars;

    private void Awake()
    {
        _instance = this; 
    }

    public void UpdateGemCount(int count)
    {
        gemCountText.text = " " + count;
    }

    public void UpdateHealth(int healthRemaining)
    {
        for(int i = 0; i <= healthRemaining; i++)
        {
            if(i == healthRemaining)
            {
                healthBars[i].enabled = false;
            }
        }
    }
}
