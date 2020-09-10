using DChild.Gameplay.Characters.Enemies;
using Holysoft.Gameplay.UI;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossCombatUI : MonoBehaviour
    {
        [SerializeField]
        private BossBannerUI m_bossBanner;
        [SerializeField]
        private SliderStatUI m_bossHealth;
        [SerializeField]
        private SegmentedStatUI m_segmentedBossHealth;


        public void SetBoss(Boss boss)
        {
            m_bossBanner.SetBannerInfo(boss);
            m_bossHealth.MonitorInfoOf(boss.health);

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