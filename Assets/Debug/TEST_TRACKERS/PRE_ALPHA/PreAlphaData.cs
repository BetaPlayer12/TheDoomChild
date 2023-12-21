using UnityEngine;

namespace DChild.Testing.PreAlpha
{
    [System.Serializable]
    public class PreAlphaData
    {
        [SerializeField]
        private PlayTimeTracker.SaveData m_playTime;
        [SerializeField]
        private PlayerDeathCountTracker.SaveData m_deathCount;
        [SerializeField]
        private ItemUsageTracker.SaveData m_itemUsage;

        public PreAlphaData(PlayTimeTracker.SaveData playTime, PlayerDeathCountTracker.SaveData deathCount, ItemUsageTracker.SaveData itemUsage)
        {
            m_playTime = playTime;
            m_deathCount = deathCount;
            m_itemUsage = itemUsage;
        }

        public PlayTimeTracker.SaveData playTimeData => m_playTime;
        public PlayerDeathCountTracker.SaveData deathCountData => m_deathCount;
        public ItemUsageTracker.SaveData itemUsageData => m_itemUsage;
    }

}