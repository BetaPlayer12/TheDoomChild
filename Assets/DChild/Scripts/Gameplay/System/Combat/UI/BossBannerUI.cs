using DChild.Gameplay.Characters.Enemies;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossBannerUI : MonoBehaviour
    {
        [System.Serializable]
        private class Panel
        {
            [SerializeField]
            private GameObject m_upper;
            [SerializeField]
            private GameObject m_lower;

            public void Enable(bool value)
            {
                m_upper.SetActive(value);
                m_lower.SetActive(value);
            }
        }

        [SerializeField]
        private BossNameUI m_bossName;
        [SerializeField]
        private Panel m_normal;
        [SerializeField]
        private Panel m_unique;

        public void SetBannerInfo(Boss boss)
        {
            m_bossName.SetName(boss);
            if(boss.creatureName == "Black Death")
            {
                m_normal.Enable(false);
                m_unique.Enable(true);
            }
            else
            {
                m_normal.Enable(true);
                m_unique.Enable(false);
            }
        }
    }
}