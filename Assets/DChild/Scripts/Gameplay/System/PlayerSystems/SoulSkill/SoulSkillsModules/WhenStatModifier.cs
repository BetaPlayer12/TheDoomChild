using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class WhenStatModifier : ISoulSkillModule
    {
        public class ReferenceInfo
        {
            public IPlayer player { get; }
            public object eventSender { get; }
            public bool effectApplied;

            public ReferenceInfo(IPlayer player, object eventSender)
            {
                this.player = player;
                this.eventSender = eventSender;
                effectApplied = false;
            }
        }

        private enum Stats
        {
            HP,
            MP
        }

        private enum Comparison
        {
            Greater,
            Lesser
        }

        [SerializeField, LabelText("Stat")]
        private Stats m_toChange;
        [SerializeField, Wrap(1f, 100f), SuffixLabel("%", overlay: true)]
        private int m_value;
        [SerializeField, LabelText("Stat")]
        private Comparison m_comparison;
        [OdinSerialize]
        private ISoulSkillModule[] m_modules;

        private int m_connectedSoulSkillID;
        private List<ReferenceInfo> m_reference;
        private bool m_initialized;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            if (m_initialized == false)
            {
                m_connectedSoulSkillID = soulSkillInstanceID;
                m_reference = new List<ReferenceInfo>();
            }

            if (m_toChange == Stats.HP)
            {
                var referenceInfo = new ReferenceInfo(player, player.health);
                if (IsValid((float)player.health.currentValue / player.health.maxValue))
                {
                    for (int i = 0; i < m_modules.Length; i++)
                    {
                        m_modules[i].AttachTo(soulSkillInstanceID, player);
                    }
                    referenceInfo.effectApplied = true;
                }
                player.health.ValueChanged += OnStatChange;
                m_reference.Add(referenceInfo);
            }
            else
            {
                var referenceInfo = new ReferenceInfo(player, player.magic);
                if (IsValid((float)player.magic.currentValue / player.magic.maxValue))
                {
                    for (int i = 0; i < m_modules.Length; i++)
                    {
                        m_modules[i].AttachTo(soulSkillInstanceID, player);
                    }
                    referenceInfo.effectApplied = true;
                }
                player.magic.ValueChanged += OnStatChange;
                m_reference.Add(referenceInfo);
            }
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            if (m_toChange == Stats.HP)
            {
                player.health.ValueChanged -= OnStatChange;
            }
            else
            {
                player.magic.ValueChanged -= OnStatChange;
            }

            for (int i = 0; i < m_reference.Count; i++)
            {
                if (m_reference[i].player == player)
                {
                    m_reference.RemoveAt(i);
                    break;
                }
            }
        }

        private bool IsValid(float currentPercent)
        {
            if (m_comparison == Comparison.Greater)
            {
                return currentPercent > m_value;
            }
            else
            {
                return currentPercent < m_value;
            }
        }

        private void OnStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            var referenceInfo = GetReferenceInfo(sender);

            if (IsValid(eventArgs.percentValue))
            {
                if (referenceInfo.effectApplied == false)
                {
                    for (int i = 0; i < m_modules.Length; i++)
                    {
                        m_modules[i].AttachTo(m_connectedSoulSkillID, referenceInfo.player);
                    }
                    referenceInfo.effectApplied = true;
                }
            }
            else
            {
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].DetachFrom(m_connectedSoulSkillID, referenceInfo.player);
                }
                referenceInfo.effectApplied = false;
            }
        }

        private ReferenceInfo GetReferenceInfo(object sender)
        {
            for (int i = 0; i < m_reference.Count; i++)
            {
                if (m_reference[i].eventSender == sender)
                {
                    return m_reference[i];
                }
            }

            return null;
        }
    }
}