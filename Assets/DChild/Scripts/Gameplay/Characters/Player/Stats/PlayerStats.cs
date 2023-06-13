using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayerStats
    {
        event EventAction<StatValueEventArgs> StatsChanged;
        void AddStat(PlayerStat stat, int value);
        void SetBaseStat(PlayerStat stat, int value);
        int GetTotalStat(PlayerStat stat);
        int GetBaseStat(PlayerStat stat);
        int GetAddedStat(PlayerStat stat);
    }

    [AddComponentMenu("DChild/Gameplay/Player/Player Stats")]
    public class PlayerStats : MonoBehaviour, IPlayerStats
    {

        [SerializeField]
        private PlayerBaseStatsData m_defaultStatData;

        [HorizontalGroup("Split")]

        [SerializeField, HideLabel, OnValueChanged("CalculateStats"), BoxGroup("Split/Base")]
        private PlayerStatsInfo m_baseStat;
        [ShowInInspector, HideInEditorMode, HideLabel, OnValueChanged("CalculateStats"), BoxGroup("Split/Added")]
        private PlayerStatsInfo m_addedStats;
        [ShowInInspector, HideInEditorMode, HideLabel, OnValueChanged("SendEvent"), BoxGroup("Total"), ReadOnly]
        private PlayerStatsInfo m_totalStats;

        public event EventAction<StatValueEventArgs> StatsChanged;


        public void AddStat(PlayerStat stat, int value)
        {
            m_addedStats.AddStat(stat, value);
            m_totalStats.SetStat(stat, m_addedStats.GetStat(stat) + m_baseStat.GetStat(stat));
            StatsChanged?.Invoke(this, new StatValueEventArgs(stat, GetTotalStat(stat)));
        }

        public int GetTotalStat(PlayerStat stat) => m_totalStats.GetStat(stat);
        public int GetBaseStat(PlayerStat stat) => m_baseStat.GetStat(stat);

        public int GetAddedStat(PlayerStat stat) => m_addedStats.GetStat(stat);

        public void SetBaseStat(PlayerStat stat, int value)
        {
            m_baseStat.SetStat(stat, value);
            m_totalStats.SetStat(stat, value + m_addedStats.GetStat(stat));
            StatsChanged?.Invoke(this, new StatValueEventArgs(stat, m_totalStats.GetStat(stat)));
        }

        public void Initialize()
        {
            m_baseStat.CopyInfo(m_defaultStatData.info);
        }

        private void Awake()
        {
            CalculateStats();
        }

        private void CalculateStats()
        {
            var size = (int)PlayerStat._COUNT;
            for (int i = 0; i < size; i++)
            {
                var stat = (PlayerStat)i;
                m_totalStats.SetStat(stat, m_addedStats.GetStat(stat) + m_baseStat.GetStat(stat));
            }
        }
#if UNITY_EDITOR
        private void SendEvent()
        {
            StatsChanged?.Invoke(this, new StatValueEventArgs(PlayerStat._COUNT, 0));
        }
#endif
    }
}