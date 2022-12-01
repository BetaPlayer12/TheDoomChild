using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallMovementStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_speed;
    public float speed => m_speed;

    public void CopyInfo(WallMovementStatsInfo reference)
    {
        m_speed = reference.speed;
    }
}
