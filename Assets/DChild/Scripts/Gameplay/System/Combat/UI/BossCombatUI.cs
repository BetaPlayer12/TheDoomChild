using DChild.Gameplay.Characters.Enemies;
using Doozy.Engine;
using Holysoft.Gameplay.UI;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossCombatUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_bossName;        
        [SerializeField]
        private TextMeshProUGUI m_bossTitle;
        [SerializeField]
        private SliderStatUI m_bossHealth;

        public void SetBoss(Boss boss)
        {
            m_bossHealth.MonitorInfoOf(boss.health);
            m_bossName.text = boss.creatureName;
            m_bossTitle.text = boss.creatureTitle;
        }
    }
}