using System;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class PercentStatModifier : ISoulSkillModule
    {
        private enum Stats
        {
            Attack,
            Magic,
            Health
        }

        [SerializeField, LabelText("Stat")]
        private Stats m_toChange;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_value;

        private IPlayerStats m_reference;
        private int m_currentIncreasedValue;
        private float value => m_value / 100f;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            m_reference = player.stats;
            switch (m_toChange)
            {
                case Stats.Attack:
                    IncreaseAttack(m_reference);
                    break;
                case Stats.Magic:
                    IncreaseMagic(m_reference);
                    break;
                case Stats.Health:
                    IncreaseHealth(m_reference);
                    break;
            }
            m_reference.StatsChanged += OnStatsChange;
        }


        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            m_reference.StatsChanged -= OnStatsChange;
            switch (m_toChange)
            {
                case Stats.Attack:
                    m_reference.AddStat(PlayerStat.Attack, -m_currentIncreasedValue);
                    break;
              
                case Stats.Magic:
                    m_reference.AddStat(PlayerStat.Magic, -m_currentIncreasedValue);
                    break;
                case Stats.Health:
                    m_reference.AddStat(PlayerStat.Health, -m_currentIncreasedValue);
                    break;
            }
            m_reference = null;
        }
        private void IncreaseAttack(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetTotalStat(PlayerStat.MaxAttack) * value);
            stats.AddStat(PlayerStat.Attack, m_currentIncreasedValue);
        }

     

        private void IncreaseMagic(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetTotalStat(PlayerStat.Magic) * value);
            stats.AddStat(PlayerStat.Magic, m_currentIncreasedValue);
        }

        private void IncreaseHealth(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetTotalStat(PlayerStat.Health) * value);
            stats.AddStat(PlayerStat.Health, m_currentIncreasedValue);
        }


        private void OnStatsChange(object sender, StatValueEventArgs eventArgs)
        {
            switch (eventArgs.stat)
            {
                case PlayerStat.Attack:
                case PlayerStat.MagicAttack:
                    m_reference.AddStat(PlayerStat.Attack, -m_currentIncreasedValue);
                    IncreaseAttack(m_reference);
                    break;
                case PlayerStat.Health:
                    m_reference.AddStat(PlayerStat.Health, -m_currentIncreasedValue);
                    IncreaseHealth(m_reference);
                    break;
                case PlayerStat.Magic:
                    m_reference.AddStat(PlayerStat.Magic, -m_currentIncreasedValue);
                    IncreaseMagic(m_reference);
                    break;

            }
        }
    }
}