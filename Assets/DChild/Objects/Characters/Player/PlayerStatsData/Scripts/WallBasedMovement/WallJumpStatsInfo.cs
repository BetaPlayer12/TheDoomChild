using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallJumpStatsInfo
{
    [SerializeField]
    private Vector2 m_power;
    public Vector2 power => m_power;

    [SerializeField, MinValue(0f)]
    private float m_disableInputDuration;
    public float disableInputDuration => m_disableInputDuration;

    public void CopyInfo(WallJumpStatsInfo reference)
    {
        m_power = reference.power;
        m_disableInputDuration = reference.disableInputDuration;
    }
}
