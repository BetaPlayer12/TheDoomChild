using DChild.Gameplay.Characters.Enemies;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.Gameplay.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossCombatUI : MonoBehaviour
    {
        [SerializeField]
        private UIContainer m_container;
        [SerializeField]
        private UIContainer m_bossHealthContainer;
        [SerializeField]
        private UIContainer m_bossNameContainer;
        [SerializeField]
        private BossBannerUI m_bossBanner;
        [SerializeField]
        private SliderStatUI m_bossHealth;
        [SerializeField]
        private SegmentedStatUI m_segmentedBossHealth;

        public void Show()
        {
            m_container.Show(true);
        }

        public void Hide()
        {
            m_container.Hide(true);
        }

        [Button]
        public void ShowBossHealth()
        {
            m_bossHealthContainer.Show(true);
        }

        [Button]
        public void ShowBossName()
        {
            m_bossNameContainer.Show(true);
        }

        [Button]
        public void HideBossHealth()
        {
            m_bossHealthContainer.Hide(true);
        }

        [Button]
        public void HideBossName()
        {
            m_bossNameContainer.Hide(true);
        }

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