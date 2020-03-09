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
        [SerializeField]
        private SegmentedStatUI m_segmentedBossHealth;


        public void SetBoss(Boss boss)
        {
            m_bossHealth.MonitorInfoOf(boss.health);
            m_bossName.text = boss.creatureName;
            m_bossTitle.text = boss.creatureTitle;

            //if (m_segmentedBossHealth)
            //{
            //    var healthSegments = boss.healthSegments;
            //    var previousSegmentHealth = 0;
            //    for (int i = 0; i < m_segmentedBossHealth.segementCount; i++)
            //    {
            //        if (i >= healthSegments.Length)
            //        {
            //            m_segmentedBossHealth.SetMaxValueOfSegment(i, 0);
            //        }
            //        else
            //        {
            //            var currentSegment = healthSegments[i];
            //            var difference = currentSegment - previousSegmentHealth;
            //            m_segmentedBossHealth.SetMaxValueOfSegment(i, difference);
            //            previousSegmentHealth = currentSegment;
            //        }
            //    }
            //    m_segmentedBossHealth.MonitorInfoOf(boss.health);
            //}
        }
    }
}