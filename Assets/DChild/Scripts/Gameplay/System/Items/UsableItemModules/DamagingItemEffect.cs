﻿using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct DamagingItemEffect : IUpdatableItemEffect
    {
        private enum Stat
        {
            Health,
            Magic
        }

        [SerializeField]
        private Stat m_toDamage;
        [SerializeField, MinValue(0)]
        private int m_value;
        [SerializeField,ShowIf("@m_toDamage == Stat.Health")]
        private DamageType m_damageType;
        [SerializeField, MinValue(0)]
        private float m_interval;
        [SerializeField]
        private bool m_applyPerUpdate;

        private float m_stackingValue;
        private float m_timer;

        private DamagingItemEffect(Stat toRegenerate, int value, float interval, bool applyPerUpdate) : this()
        {
            m_toDamage = toRegenerate;
            m_value = value;
            m_interval = interval;
            m_applyPerUpdate = applyPerUpdate;
        }

        public IUpdatableItemEffect CreateCopy() => new DamagingItemEffect(m_toDamage, m_value, m_interval, m_applyPerUpdate);

        public void Execute(IPlayer player, float deltaTime)
        {
            bool apply = false;
            var valueToApply = 0;
            if (m_applyPerUpdate)
            {
                m_stackingValue += m_value * deltaTime;
                if (m_stackingValue > 0)
                {
                    apply = true;
                    var integer = Mathf.FloorToInt(m_stackingValue);
                    m_stackingValue -= integer;
                    valueToApply = integer;
                }
            }
            else
            {
                if (m_timer > 0)
                {
                    m_timer -= deltaTime;
                    if (m_timer <= 0)
                    {
                        apply = true;
                        valueToApply = m_value;
                        m_timer += m_interval;
                    }
                }
            }

            if (apply)
            {
                if (m_toDamage == Stat.Health)
                {
                    GameplaySystem.combatManager.Damage(player.damageableModule, new Damage(m_damageType, valueToApply));
                }
                else
                {
                    player.magic.AddCurrentValue(-valueToApply);
                }
            }
        }

        public override string ToString()
        {
            var value = m_applyPerUpdate ? m_value / m_interval : m_interval;
            var second = m_applyPerUpdate ? 1 : m_interval;
            if (m_value >= 0)
            {
                return $"Consumes {value} {m_toDamage.ToString()} per {second} second(s)";
            }
            return "";
        }
    }
}
