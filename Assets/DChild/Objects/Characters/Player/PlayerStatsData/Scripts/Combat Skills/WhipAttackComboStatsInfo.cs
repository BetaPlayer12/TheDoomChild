using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WhipAttackComboStatsInfo
{
    [SerializeField]
    private int m_whipStateAmount;
    public int whipStateAmount => m_whipStateAmount;
    [SerializeField]
    private float m_whipComboCooldown;
    public float whipComboCooldown => m_whipComboCooldown;
    [SerializeField]
    private float m_whipMovementCooldown;
    public float whipMovementCooldown => m_whipMovementCooldown;
    [SerializeField]
    private List<Vector2> m_pushForce;
    public List<Vector2> pushForce => m_pushForce;

    public void CopyInfo(WhipAttackComboStatsInfo reference)
    {
        m_whipStateAmount = reference.whipStateAmount;
        m_whipComboCooldown = reference.whipComboCooldown;
        m_whipMovementCooldown = reference.whipMovementCooldown;
        m_pushForce = reference.pushForce;
    }
}
