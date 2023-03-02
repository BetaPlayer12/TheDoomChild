using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GroundJumpStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_jumpPower;
    public float jumpPower => m_jumpPower;
    [SerializeField, MinValue(0)]
    private float m_jumpCutoffPower;
    public float jumpCutoffPower => m_jumpCutoffPower;
    [SerializeField, MinValue(0)]
    private float m_allowCutoffAfterDuration;
    public float allowCutoffAfterDuration => m_allowCutoffAfterDuration;

    public void CopyInfo(GroundJumpStatsInfo reference)
    {
        m_jumpPower = reference.jumpPower;
        m_jumpCutoffPower = reference.jumpCutoffPower;
        m_allowCutoffAfterDuration = reference.allowCutoffAfterDuration;
    }
}
