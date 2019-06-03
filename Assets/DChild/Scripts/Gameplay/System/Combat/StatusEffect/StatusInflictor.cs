using System;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    [System.Serializable]
    public struct StatusInflictionInfo
    {
        [SerializeField]
        private StatusEffectType m_effect;
        [SerializeField, Range(0.1f, 100f)]
        private float m_chance;

        public StatusInflictionInfo(StatusEffectType m_effect, float m_chance)
        {
            this.m_effect = m_effect;
            this.m_chance = m_chance;
        }

        public StatusEffectType effect => m_effect;
        public float chance
        {
            get => m_chance; set
            {
                m_chance = Mathf.Clamp01(value);
            }
        }
    }

    public class StatusInflictor : MonoBehaviour
    {
        [SerializeField]
        private StatusInflictionInfo[] m_statusToInflict;

        private IAttacker m_source;

        public void InflictStatusTo(IStatusReciever reciever) => GameplaySystem.combatManager.InflictStatusTo(reciever, m_statusToInflict);

        private void OnAttackerAttacked(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (DChildUtility.HasInterface<IStatusReciever>(eventArgs.target))
            {
                InflictStatusTo((IStatusReciever)eventArgs.target);
            }
        }

        private void OnEnable()
        {
            m_source = GetComponentInParent<IAttacker>();
            m_source.TargetDamaged += OnAttackerAttacked;
        }

        private void OnDisable()
        {
            m_source.TargetDamaged += OnAttackerAttacked;
        }
    }
}