using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class WhenStatusInflictedModifier : ISoulSkillModule
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

        [SerializeField]
        private StatusEffectType m_type;
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

            var reciever = player.statusEffectReciever;
            reciever.StatusRecieved += OnStatusRecieved;
            reciever.StatusEnd += OnStatusEnd;

            var referenceInfo = new ReferenceInfo(player, player.statusEffectReciever);
            if (reciever.IsInflictedWith(m_type))
            {
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].AttachTo(soulSkillInstanceID, player);
                }
                referenceInfo.effectApplied = true;
            }
            m_reference.Add(referenceInfo);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            var reciever = player.statusEffectReciever;
            reciever.StatusRecieved -= OnStatusRecieved;
            reciever.StatusEnd -= OnStatusEnd;

            if (reciever.IsInflictedWith(m_type))
            {
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].DetachFrom(soulSkillInstanceID, player);
                }
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

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            if (eventArgs.type == m_type)
            {
                var referenceInfo = GetReferenceInfo(sender);
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].DetachFrom(m_connectedSoulSkillID, referenceInfo.player);
                }
                referenceInfo.effectApplied = false;
            }
        }

        private void OnStatusRecieved(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            if (eventArgs.type == m_type)
            {
                var referenceInfo = GetReferenceInfo(sender);
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].AttachTo(m_connectedSoulSkillID, referenceInfo.player);
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