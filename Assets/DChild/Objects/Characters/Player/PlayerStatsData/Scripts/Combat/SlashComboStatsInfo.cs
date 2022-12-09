using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlashComboStatsInfo
{
    [SerializeField]
    private int m_slashStateAmount;
    public int slashStateAmount => m_slashStateAmount;
    [SerializeField]
    private float m_slashComboCooldown;
    public float slashComboCooldown => m_slashComboCooldown;
    [SerializeField]
    private float m_slashMovementCooldown;
    public float slashMovementCooldown => m_slashMovementCooldown;
    [SerializeField]
    private List<Vector2> m_pushForce;
    public List<Vector2> pushForce => m_pushForce;

    public void CopyInfo(SlashComboStatsInfo reference)
    {
        m_slashStateAmount = reference.slashStateAmount;
        m_slashComboCooldown = reference.slashComboCooldown;
        m_slashMovementCooldown = reference.slashMovementCooldown;
        m_pushForce = reference.pushForce;
    }
}
