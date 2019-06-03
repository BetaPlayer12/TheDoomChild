using System;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class MaxStatModifier : ISoulSkillModule
    {
        private enum Stats
        {
            Attack,
            Defense,
            Magic,
            Health
        }

        [SerializeField, LabelText("Stat")]
        private Stats m_toChange;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_value;

        private IPlayerStats m_reference;
        private int m_currentIncreasedValue;

        public void AttachTo(IPlayer player)
        {
            m_reference = player.stats;
            switch (m_toChange)
            {
                case Stats.Attack:
                    IncreaseAttack(m_reference);
                    break;
                case Stats.Defense:
                    IncreaseDefense(m_reference);
                    break;
                case Stats.Magic:
                    IncreaseMagic(m_reference);
                    break;
                case Stats.Health:
                    IncreaseHealth(m_reference);
                    break;
            }
            m_reference.ApplyChanges();
            m_reference.StatsChanged += OnStatsChange;
        }


        public void DetachFrom(IPlayer player)
        {
            switch (m_toChange)
            {
                case Stats.Attack:
                    m_reference.AddStat(PlayerStat.Attack, -m_currentIncreasedValue);
                    break;
                case Stats.Defense:
                    m_reference.AddStat(PlayerStat.Defense, -m_currentIncreasedValue);
                    break;
                case Stats.Magic:
                    m_reference.AddStat(PlayerStat.Magic, -m_currentIncreasedValue);
                    break;
                case Stats.Health:
                    m_reference.AddStat(PlayerStat.Health, -m_currentIncreasedValue);
                    break;
            }
            m_reference.StatsChanged -= OnStatsChange;
            m_reference.ApplyChanges();
            m_reference = null;
        }
        private void IncreaseAttack(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetStat(PlayerStat.MaxAttack) * m_value);
            stats.AddStat(PlayerStat.Attack, m_currentIncreasedValue);
        }

        private void IncreaseDefense(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetStat(PlayerStat.MaxDefense) * m_value);
            stats.AddStat(PlayerStat.Defense, m_currentIncreasedValue);
        }

        private void IncreaseMagic(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetStat(PlayerStat.Magic) * m_value);
            stats.AddStat(PlayerStat.Magic, m_currentIncreasedValue);
        }

        private void IncreaseHealth(IPlayerStats stats)
        {
            m_currentIncreasedValue = Mathf.FloorToInt(stats.GetStat(PlayerStat.Health) * m_value);
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
                case PlayerStat.Defense:
                case PlayerStat.MagicDefense:
                    m_reference.AddStat(PlayerStat.Defense, -m_currentIncreasedValue);
                    IncreaseDefense(m_reference);
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
            m_reference.ApplyChanges();
        }
    }
}