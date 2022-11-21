using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WhipAttackStatsInfo
{
    [SerializeField]
    private float m_aerialGravity;
    public float aerialGravity => m_aerialGravity;

    public void CopyInfo(WhipAttackStatsInfo reference)
    {
        m_aerialGravity = reference.aerialGravity;
    }
}
