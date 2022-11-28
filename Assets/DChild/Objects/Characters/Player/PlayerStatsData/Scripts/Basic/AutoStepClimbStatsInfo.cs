using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AutoStepClimbStatsInfo
{
    [SerializeField]
    private float m_distanceFromLegContactForDestination;
    public float distanceFromLegContactForDestination => m_distanceFromLegContactForDestination;
    [SerializeField]
    private Vector2 m_spaceCheckerOffset;
    public Vector2 spaceCheckerOffset => m_spaceCheckerOffset;

    public void CopyInfo(AutoStepClimbStatsInfo reference)
    {
        m_distanceFromLegContactForDestination = reference.distanceFromLegContactForDestination;
        m_spaceCheckerOffset = reference.spaceCheckerOffset;
    }
}
