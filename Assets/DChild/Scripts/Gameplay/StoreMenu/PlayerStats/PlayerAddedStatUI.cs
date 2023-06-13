using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI.PlayerStats
{
    public class PlayerAddedStatUI : MonoBehaviour
    {
        [SerializeField]
        private UIContainer m_container;
        [SerializeField]
        private TextMeshProUGUI m_valueLabel;
        [SerializeField]
        private PlayerStat m_stat;

        private void DisplayStat(int value)
        {
            if (value == 0)
            {
                m_container.Hide();
            }
            else
            {
                m_container.Show();
                m_valueLabel.text = value.ToString();
            }
        }

        private void OnStatChange(object sender, StatValueEventArgs eventArgs)
        {
            if (eventArgs.stat == m_stat)
            {
                var stats = GameplaySystem.playerManager.player.stats;
                DisplayStat(stats.GetAddedStat(m_stat));
            }
        }

        private void Start()
        {
            var stats = GameplaySystem.playerManager.player.stats;
            stats.StatsChanged += OnStatChange;

            DisplayStat(stats.GetAddedStat(m_stat));
        }
    }
}