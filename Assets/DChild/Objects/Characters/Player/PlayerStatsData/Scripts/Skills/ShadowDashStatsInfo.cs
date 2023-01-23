using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShadowDashStatsInfo 
{
    [SerializeField, MinValue(0)]
    private int m_baseSourceRequiredAmount;
    public int baseSourceRequiredAmount => m_baseSourceRequiredAmount;

    public void CopyInfo(ShadowDashStatsInfo reference)
    {
        m_baseSourceRequiredAmount = reference.baseSourceRequiredAmount;
    }
}
