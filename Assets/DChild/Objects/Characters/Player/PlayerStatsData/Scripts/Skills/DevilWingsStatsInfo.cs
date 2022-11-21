using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DevilWingsStatsInfo
{
    [SerializeField, MinValue(0)]
    private int m_sourceRequiredAmount;
    public int sourceRequiredAmount => m_sourceRequiredAmount;
    [SerializeField, MinValue(0)]
    private float m_sourceConsumptionRate;
    public float sourceConsumptionRate => m_sourceConsumptionRate;

    public void CopyInfo(DevilWingsStatsInfo reference)
    {
        m_sourceRequiredAmount = reference.m_sourceRequiredAmount;
        m_sourceConsumptionRate = reference.m_sourceConsumptionRate;
    }
}
