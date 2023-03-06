using DChild.Menu;
using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameGuard : MonoBehaviour
{
    [SerializeField]
    private SignalSender m_pauseGameSignal;
    
    public void CanPauseGame()
    {
        if (!LoadingHandle.isLoading)
        {
            m_pauseGameSignal.SendSignal();
        }
    }
}
