using DChild.Gameplay.Environment.Interractables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEyeAnimationGroup : MonoBehaviour
{
    [SerializeField]
    private TimerEyeAnimation[] m_eyeTimers;
   
    public void StartAllEyeCloseAnimation()
    {
        for (int i = 0; i < m_eyeTimers.Length; i++)
        {
            m_eyeTimers[i].UseOpenAnimation();
            m_eyeTimers[i].StartClosingAnimation();
        }
    }
    public void UseAllEyeCloseAnimation()
    {
        for (int i = 0; i < m_eyeTimers.Length; i++)
        {
            m_eyeTimers[i].UseCloseAnimation();
        }
    }

}
