using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IdleHandleStatsInfo
{
    [SerializeField, MinValue(1f)]
    private float m_playExtendedIdleAnimAfter;
    public float playExtendedIdleAnimAfter => m_playExtendedIdleAnimAfter;
    [SerializeField, MinValue(1)]
    private int m_maxIdleAnimCount;
    public int maxIdleAnimCount => m_maxIdleAnimCount;

    public void CopyInfo(IdleHandleStatsInfo reference)
    {
        m_playExtendedIdleAnimAfter = reference.playExtendedIdleAnimAfter;
        m_maxIdleAnimCount = reference.m_maxIdleAnimCount;
    }
}
