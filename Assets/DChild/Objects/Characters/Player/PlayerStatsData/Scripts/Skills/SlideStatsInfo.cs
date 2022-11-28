using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlideStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_velocity;
    public float velocity => m_velocity;
    [SerializeField, MinValue(0)]

    private float m_cooldown;
    public float cooldown => m_cooldown;

    [SerializeField, MinValue(0)]
    private float m_duration;
    public float duration => m_duration;

    public void CopyInfo(SlideStatsInfo reference)
    {
        m_velocity = reference.velocity;
        m_cooldown = reference.cooldown;
        m_duration = reference.duration;
    }
}
