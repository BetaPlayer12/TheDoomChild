using DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI.PlayerStats
{
    public class PlayerBaseStatUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_valueLabel;
        [SerializeField]
        private PlayerStat m_stat;

        private void OnStatChange(object sender, StatValueEventArgs eventArgs)
        {
            if (eventArgs.stat == m_stat)
            {
                var stats = GameplaySystem.playerManager.player.stats;
                m_valueLabel.text = stats.GetBaseStat(m_stat).ToString();
            }
        }

        private void Start()
        {
            var stats = GameplaySystem.playerManager.player.stats;
            stats.StatsChanged += OnStatChange;

            m_valueLabel.text = stats.GetBaseStat(m_stat).ToString();
        }
    }
}