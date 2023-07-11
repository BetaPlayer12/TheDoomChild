using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnDemoUnityEvent : MonoBehaviour
{
    public string purposeOfScript;

    [SerializeField]
    private UnityEvent m_onDemoEnd;

    public void EndDemo()
    {
        m_onDemoEnd?.Invoke();
    }    
}
