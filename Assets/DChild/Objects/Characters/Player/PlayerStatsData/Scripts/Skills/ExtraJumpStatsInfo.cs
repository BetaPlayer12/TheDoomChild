using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ExtraJumpStatsInfo
{
    [SerializeField, MinValue(0)]
    private int m_count;
    public int count => m_count;
    [SerializeField, MinValue(0)]
    private float m_power;
    public float power => m_power;

    public void CopyInfo(ExtraJumpStatsInfo reference)
    {
        m_count = reference.count;
        m_power = reference.power;
    }
}
