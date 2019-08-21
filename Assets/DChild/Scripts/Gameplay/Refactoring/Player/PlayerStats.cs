using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public interface IPlayerStats
    {
        event EventAction<StatValueEventArgs> StatsChanged;
        void AddStat(PlayerStat stat, int value);
        int GetStat(PlayerStat stat);
        int GetBaseStat(PlayerStat stat);
        void SetBaseStat(PlayerStat stat, int value);
    }

    public class PlayerStats : MonoBehaviour, IPlayerStats
    {
        [SerializeField, HideInPlayMode, HideLabel]
        private PlayerStatsInfo m_baseStat;

        [ShowInInspector, HideInEditorMode, HideLabel, OnValueChanged("SendEvent")]
        private PlayerStatsInfo m_totalStats;
        private PlayerStatsInfo m_addedStats;

        public event EventAction<StatValueEventArgs> StatsChanged;

        public void AddStat(PlayerStat stat, int value)
        {
            m_addedStats.AddStat(stat, value);
            m_totalStats.SetStat(stat, value + m_baseStat.GetStat(stat));
            StatsChanged?.Invoke(this, new StatValueEventArgs(stat, GetStat(stat)));
        }

        public int GetStat(PlayerStat stat) => m_totalStats.GetStat(stat);

        public int GetBaseStat(PlayerStat stat) => m_baseStat.GetStat(stat);
        public void SetBaseStat(PlayerStat stat, int value)
        {
            m_baseStat.SetStat(stat, value);
            m_totalStats.SetStat(stat, value + m_addedStats.GetStat(stat));
            StatsChanged?.Invoke(this, new StatValueEventArgs(stat, m_totalStats.GetStat(stat)));
        }

#if UNITY_EDITOR
        private void SendEvent()
        {
            StatsChanged?.Invoke(this, new StatValueEventArgs(PlayerStat._COUNT, 0));
        }
#endif
    }
}