using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct RegeneratingItemEffect : IUpdatableItemEffect
    {
        private enum Stat
        {
            Health,
            Magic
        }

        [SerializeField]
        private Stat m_toRegenerate;
        [SerializeField, MinValue(0)]
        private int m_value;
        [SerializeField, MinValue(0)]
        private float m_interval;
        [SerializeField]
        private bool m_applyPerUpdate;

        private float m_stackingValue;
        private float m_timer;

        private RegeneratingItemEffect(Stat toRegenerate, int value, float interval, bool applyPerUpdate) : this()
        {
            m_toRegenerate = toRegenerate;
            m_value = value;
            m_interval = interval;
            m_applyPerUpdate = applyPerUpdate;
        }

        public IUpdatableItemEffect CreateCopy() => new RegeneratingItemEffect(m_toRegenerate, m_value, m_interval, m_applyPerUpdate);

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
                if (m_toRegenerate == Stat.Health)
                {
                    if (player.health.isFull == false)
                    {
                        GameplaySystem.combatManager.Heal(player.healableModule, valueToApply);
                    }
                }
                else
                {
                    if (player.magic.isFull == false)
                    {
                        player.magic.AddCurrentValue(valueToApply);
                    }
                }
            }
        }

        public override string ToString()
        {
            var value = m_applyPerUpdate ? m_value / m_interval : m_interval;
            var second = m_applyPerUpdate ? 1 : m_interval;
            if (m_value >= 0)
            {
                return $"Regenerate {value} {m_toRegenerate.ToString()} per {second} second(s)";
            }
            return "";
        }
    }
}
