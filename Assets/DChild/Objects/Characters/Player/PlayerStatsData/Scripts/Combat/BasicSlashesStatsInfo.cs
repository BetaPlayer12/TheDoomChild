using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BasicSlashesStatsInfo
{
    [SerializeField]
    private Vector2 m_momentumVelocity;
    public Vector2 momentumVelocity => m_momentumVelocity;

    public void CopyInfo(BasicSlashesStatsInfo reference)
    {
        m_momentumVelocity = reference.momentumVelocity;
    }
}
