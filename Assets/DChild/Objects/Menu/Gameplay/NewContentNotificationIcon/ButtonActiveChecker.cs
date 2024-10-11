using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActiveChecker : MonoBehaviour
{
    [SerializeField]
    private UIToggle m_buttonToCheck;

    public bool CheckButtonActiveness()
    {
        if(m_buttonToCheck.interactable && m_buttonToCheck.isOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
