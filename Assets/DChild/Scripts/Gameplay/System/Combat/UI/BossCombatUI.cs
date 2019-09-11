using DChild.Gameplay.Characters.Enemies;
using Doozy.Engine;
using Holysoft.Gameplay.UI;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossCombatUI : MonoBehaviour
    {
        [SerializeField]
        private SliderStatUI m_bossHealth;

        public void SetBoss(Boss boss)
        {
            m_bossHealth.MonitorInfoOf(boss.health);
        }
    }
}