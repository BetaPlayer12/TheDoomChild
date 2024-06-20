using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatHandle : MonoBehaviour
{
    [SerializeField]
    private EventSounds m_sound;
    [SerializeField]
    private float m_beatFrequency = 3;
    [SerializeField]
    private float m_beatInterval = 0.5f;

    private float m_timer;

    public event Action<int> OnBeat;

    [Button]
    private void TriggerHeartBeatSound(int beatIndex)
    {
        OnBeat?.Invoke(beatIndex);
        m_sound.ActivateCodeTriggeredEvent1();
    }

    private IEnumerator HeartBeatRoutine()
    {
        TriggerHeartBeatSound(1);
        yield return new WaitForSeconds(m_beatInterval);
        TriggerHeartBeatSound(2);
    }

    private void TriggerHeartBeat()
    {
        StopAllCoroutines();
        StartCoroutine(HeartBeatRoutine());
    }

    private void Start()
    {
        m_timer = m_beatFrequency;
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            TriggerHeartBeat();
            m_timer = m_beatFrequency;
        }
    }


}
