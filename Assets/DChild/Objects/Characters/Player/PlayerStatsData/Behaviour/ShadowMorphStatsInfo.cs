using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShadowMorphStatsInfo
{
    [SerializeField]
    private float m_sourceRequiredAmount;
    public float sourceRequiredAmount => m_sourceRequiredAmount;
    [SerializeField]
    private float m_sourceConsumptionRate;
    public float sourceConsumptionRate => m_sourceConsumptionRate;

    public void CopyInfo(ShadowMorphStatsInfo reference)
    {
        m_sourceRequiredAmount = reference.sourceRequiredAmount;
        m_sourceConsumptionRate = reference.sourceConsumptionRate;
    }
}
