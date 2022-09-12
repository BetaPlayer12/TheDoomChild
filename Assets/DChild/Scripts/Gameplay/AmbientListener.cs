using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientListener : MonoBehaviour
{
    private AudioListener m_listener;

    private void Awake()
    {
        m_listener = GetComponent<AudioListener>();
    }

    private void OnEnable()
    {
        if (m_listener)
        {
            MasterAudio.AudioListenerChanged(m_listener);
        }
    }
}
