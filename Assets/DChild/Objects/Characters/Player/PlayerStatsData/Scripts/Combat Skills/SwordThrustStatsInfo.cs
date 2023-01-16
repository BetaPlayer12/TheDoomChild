using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SwordThrustStatsInfo
{
    [SerializeField, MinValue(0.1f)]
    private float m_chargeDuration;
    public float chargeDuration => m_chargeDuration;
    [SerializeField, MinValue(0)]
    private float m_thrustVelocity;
    public float thrustVelocity => m_thrustVelocity;
    [SerializeField, MinValue(0)]
    private float m_duration;
    public float duration => m_duration;
    [SerializeField, MinValue(0)]
    private float m_cooldown;
    public float cooldown => m_duration;

    public void CopyInfo(SwordThrustStatsInfo reference)
    {
        m_chargeDuration = reference.chargeDuration;
        m_thrustVelocity = reference.thrustVelocity;
        m_duration = reference.duration;
        m_cooldown = reference.cooldown;
    }
}
