using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FlinchStatsInfo
{
    [SerializeField, MinValue(0)]
    private int m_numberOfFlinchStates;
    public int numberOfFlinchStates => m_numberOfFlinchStates;
    [SerializeField, MinValue(0)]
    private float m_xKnockBackPower;
    public float xKnockBackPower => m_xKnockBackPower;
    [SerializeField, MinValue(0)]
    private float m_yKnockBackPower;
    public float yKnockBackPower => m_yKnockBackPower;
    [SerializeField, MinValue(0)]
    private float m_aerialKnockBackMultiplier;
    public float aerialKnockBackMultiplier => m_aerialKnockBackMultiplier;
    [SerializeField, MinValue(0)]
    private float m_flinchDuration;
    public float flinchDuration => m_flinchDuration;
    [SerializeField, MinValue(0)]
    private float m_flinchGravityScale;
    public float flinchGravityScale => m_flinchGravityScale;

    public void CopyInfo(FlinchStatsInfo reference)
    {
        m_numberOfFlinchStates = reference.numberOfFlinchStates;
        m_xKnockBackPower = reference.xKnockBackPower;
        m_yKnockBackPower = reference.yKnockBackPower;
        m_aerialKnockBackMultiplier = reference.aerialKnockBackMultiplier;
        m_flinchDuration = reference.flinchDuration;
        m_flinchGravityScale = reference.m_flinchGravityScale;
    }
}
