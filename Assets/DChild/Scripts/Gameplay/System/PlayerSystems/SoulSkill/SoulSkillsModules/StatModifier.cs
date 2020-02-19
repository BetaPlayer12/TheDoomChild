using System;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct StatModifier : ISoulSkillModule
    {
        private enum Stats
        {
            Health,
            Magic,
            Attack,
            MagicAttack,
            Crit_Damage,
            Crit_Chance,
        }

        [SerializeField, OnValueChanged("OnStatTypeChange"), LabelText("Stat")]
        private Stats m_toChange;
        [SerializeField, HideInInspector]
        private int m_value;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            switch (m_toChange)
            {
                case Stats.Attack:
                    AddStat(player.stats, PlayerStat.Attack, m_value);
                    break;
                case Stats.MagicAttack:
                    AddStat(player.stats, PlayerStat.MagicAttack, m_value);
                    break;
                case Stats.Crit_Chance:
                    AddStat(player.stats, PlayerStat.CritChance, m_value);
                    break;
                case Stats.Crit_Damage:
                    //player.modifiers.critDamageModifier += ToFloat(m_value);
                    break;
                case Stats.Health:
                    AddStat(player.stats, PlayerStat.Health, m_value);
                    break;
                case Stats.Magic:
                    AddStat(player.stats, PlayerStat.Magic, m_value);
                    break;
            }
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            switch (m_toChange)
            {
                case Stats.Attack:
                    AddStat(player.stats, PlayerStat.Attack, -m_value);
                    break;
                case Stats.MagicAttack:
                    AddStat(player.stats, PlayerStat.MagicAttack, -m_value);
                    break;
               
                case Stats.Crit_Chance:
                    AddStat(player.stats, PlayerStat.CritChance, -m_value);
                    break;
                case Stats.Crit_Damage:
                    //player.modifiers.critDamageModifier -= ToFloat(m_value);
                    break;
                case Stats.Health:
                    AddStat(player.stats, PlayerStat.Health, -m_value);
                    break;
                case Stats.Magic:
                    AddStat(player.stats, PlayerStat.Magic, -m_value);
                    break;
            }
        }

        private float ToFloat(int value) => value / 100f;
        private void AddStat(IPlayerStats reference, PlayerStat stat, int value)
        {
            reference.AddStat(stat, value);
        }
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_useFloat;
        [SerializeField, HideIf("m_useFloat"), LabelText("Value"), OnValueChanged("UpdateValue")]
        private int m_intValue;
        [SerializeField, ShowIf("m_useFloat"), LabelText("Value"), OnValueChanged("UpdateValue"), SuffixLabel("%", overlay: true)]
        private int m_floatValue;

        private void UpdateValue()
        {
            if (m_useFloat)
            {
                m_value = m_floatValue;
            }
            else
            {
                m_value = m_intValue;
            }
        }

        private void OnStatTypeChange()
        {
            switch (m_toChange)
            {
                case Stats.Crit_Damage:
                    m_useFloat = true;
                    break;

                default:
                    m_useFloat = false;
                    break;
            }
            m_intValue = 0;
            m_floatValue = 0;
            UpdateValue();
        }
#endif
    }
}