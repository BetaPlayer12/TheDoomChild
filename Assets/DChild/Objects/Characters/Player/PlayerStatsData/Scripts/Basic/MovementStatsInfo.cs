using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_jogSpeed;
    public float jogSpeed => m_jogSpeed;
    [SerializeField, MinValue(0)]
    private float m_crouchSpeed;
    public float crouchSpeed => m_crouchSpeed;
    [SerializeField, MinValue(0)]
    private float m_midAirSpeed;
    public float midAirSpeed => m_midAirSpeed;
    [SerializeField, MinValue(0)]
    private float m_pullSpeed;
    public float pullSpeed => m_pullSpeed;
    [SerializeField, MinValue(0)]
    private float m_pushSpeed;
    public float pushSpeed => m_pushSpeed;

    public void CopyInfo(MovementStatsInfo reference)
    {
        m_jogSpeed = reference.jogSpeed;
        m_crouchSpeed = reference.crouchSpeed;
        m_midAirSpeed = reference.midAirSpeed;
        m_pullSpeed = reference.pullSpeed;
        m_pushSpeed = reference.pushSpeed;
    }
}