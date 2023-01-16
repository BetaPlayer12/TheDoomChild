using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WallStickStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_wallStickOffset;
    public float wallStickOffset => m_wallStickOffset;

    public void CopyInfo(WallStickStatsInfo reference)
    {
        m_wallStickOffset = reference.wallStickOffset;
    }
}
