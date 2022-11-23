using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CombatReadinessStatsInfo
{
    [SerializeField, MinValue(0.1f)]
    private float m_duration;
    public float duration => m_duration;

    public void CopyInfo(CombatReadinessStatsInfo reference)
    {
        m_duration = reference.duration;
    }
}
