using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProjectileThrowStatsInfo
{
    [SerializeField]
    private float m_skullThrowCooldown;
    public float skullThrowCooldown => m_skullThrowCooldown;
    [SerializeField, BoxGroup("Aim")]
    private Vector2 m_defaultAim;
    public Vector2 defaultAim => m_defaultAim;
    [SerializeField, BoxGroup("Aim")]
    private RangeFloat m_horizontalThreshold;
    public RangeFloat horizontalThreshold => m_horizontalThreshold;
    [SerializeField, BoxGroup("Aim"), MinValue(0f)]
    private float m_verticalThreshold;
    public float verticalThreshold => m_verticalThreshold;
    [SerializeField, BoxGroup("Aim"), MinValue(0f)]
    private float m_aimSensitivity;
    public float aimSensitivity => m_aimSensitivity;
    [SerializeField]
    private bool m_adjustableXSpeed;
    public bool adjustableXSpeed => m_adjustableXSpeed;

    public void CopyInfo(ProjectileThrowStatsInfo reference)
    {
        m_skullThrowCooldown = reference.skullThrowCooldown;
        m_defaultAim = reference.defaultAim;
        m_horizontalThreshold = reference.horizontalThreshold;
        m_verticalThreshold = reference.verticalThreshold;
        m_aimSensitivity = reference.m_aimSensitivity;
        m_adjustableXSpeed = reference.adjustableXSpeed;
    }
}
