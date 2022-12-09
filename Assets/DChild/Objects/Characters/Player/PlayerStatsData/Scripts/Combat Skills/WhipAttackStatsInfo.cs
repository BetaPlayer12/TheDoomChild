using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WhipAttackStatsInfo
{
    [SerializeField]
    private float m_aerialGravity;
    public float aerialGravity => m_aerialGravity;
    [SerializeField]
    private float m_whipMovementCooldown;
    public float whipMovementCooldown => m_whipMovementCooldown;
    [SerializeField]
    private Vector2 m_momentumVelocity;
    public Vector2 momentumVelocity => m_momentumVelocity;

    public void CopyInfo(WhipAttackStatsInfo reference)
    {
        m_aerialGravity = reference.aerialGravity;
        m_whipMovementCooldown = reference.whipMovementCooldown;
        m_momentumVelocity = reference.momentumVelocity;
    }
}
