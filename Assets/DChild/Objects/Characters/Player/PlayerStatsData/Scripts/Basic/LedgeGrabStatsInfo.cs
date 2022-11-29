using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LedgeGrabStatsInfo 
{
    [SerializeField, MinValue(0)]
    private float m_destinationFromWallOffset;
    public float distanceFromWallOffset => m_destinationFromWallOffset;

    public void CopyInfo(LedgeGrabStatsInfo reference)
    {
        m_destinationFromWallOffset = reference.distanceFromWallOffset;
    }
}
