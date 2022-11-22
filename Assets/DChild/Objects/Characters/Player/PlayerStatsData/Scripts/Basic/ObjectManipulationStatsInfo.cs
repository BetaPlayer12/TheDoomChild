using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectManipulationStatsInfo
{
    [SerializeField, MinValue(0)]
    private float m_pushForce;
    public float pushForce => m_pushForce;
    [SerializeField, MinValue(0)]
    private float m_pullForce;
    public float pullForce => m_pullForce;
    [SerializeField, MinValue(0)]
    private float m_distanceCheck;
    public float distanceCheck => m_distanceCheck;

    public void CopyInfo(ObjectManipulationStatsInfo reference)
    {
        m_pushForce = reference.pushForce;
        m_pullForce = reference.pullForce;
        m_distanceCheck = reference.distanceCheck;
    }
}
