using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField]
    private SignalSender m_pauseSignal;
    public bool onPause;
    
    private void OnPause(InputValue value)
    {
        if (enabled == true)
        {
            onPause = value.Get<float>() == 1;
            m_pauseSignal.SendSignal();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
