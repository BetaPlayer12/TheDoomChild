using Holysoft.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsVcam : TransistionVcamAnimation
{
    [SerializeField]
    private GameObject m_target;

    [SerializeField]
    private GameObject m_transistionVcam;

    [SerializeField]
    private GameObject m_idleVcam;

    [Button("Play")]
    public sealed override void Play()
    {
        base.Play();
        m_transistionVcam.SetActive(true);
        m_idleVcam.SetActive(false);
    }
    public sealed override void Stop()
    {
        base.Stop();
        m_transistionVcam.SetActive(false);
        m_idleVcam.SetActive(true);
    }
}
