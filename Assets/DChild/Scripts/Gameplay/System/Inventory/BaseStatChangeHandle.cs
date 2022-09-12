using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class BaseStatChangeHandle : IShardCompletionHandle
    {
        [SerializeField]
        private PlayerStat m_stat;
        [SerializeField]
        private int m_modification;

        public void Execute(Player m_currentplayer)
        {

            int current = m_currentplayer.stats.GetBaseStat(m_stat);
            int modified = current + m_modification;
            m_currentplayer.stats.SetBaseStat(m_stat, modified);
        }
    }
}

