using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallSlideStatsInfo
{
    [SerializeField, MinValue(0f)]
    private float m_speed;
    public float speed => m_speed;

    public void CopyInfo(WallSlideStatsInfo reference)
    {
        m_speed = reference.speed;
    }
}
