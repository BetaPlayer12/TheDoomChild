using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BlockStatsInfo
{
    [SerializeField]
    private float m_blockDuration;
    public float duration => m_blockDuration;

    public void CopyInfo(BlockStatsInfo reference)
    {
        m_blockDuration = reference.duration;
    }
}
