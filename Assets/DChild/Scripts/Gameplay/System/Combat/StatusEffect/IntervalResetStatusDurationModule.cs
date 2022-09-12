using DChild.Gameplay;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalStatusResetModule : IStatusEffectUpdatableModule
{
    [SerializeField]
    private float m_interval;
    [SerializeField]
    private StatusEffectType m_type;
    private float m_timer;
    private StatusEffectReciever m_reciever;

    public IntervalStatusResetModule()
    {
        m_interval = 1;
    }
    public IntervalStatusResetModule(float m_interval) 
    {
        this.m_interval = m_interval;
    }
    public void Initialize(Character character)
    {
        m_reciever = character.GetComponent<StatusEffectReciever>();
        m_timer = 0;
    }

    public void Update(float delta)
    {
       if (m_timer <= 0)
        {
            m_reciever.ResetDuration(m_type);
            m_timer += m_interval;
        }
        else
        {
            m_timer -= delta;
        }

    }

    public IStatusEffectUpdatableModule CreateCopy() => new IntervalStatusResetModule(m_interval);


#if UNITY_EDITOR
    [ShowInInspector, ReadOnly]
    private float m_totalDuration;
    public void CalculateWithDuration(float duration)
    {
        m_totalDuration = Mathf.FloorToInt(duration / m_interval);
    }

#endif
}
