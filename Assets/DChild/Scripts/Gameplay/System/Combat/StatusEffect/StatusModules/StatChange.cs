using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class StatChange : IStatusEffectModule
    {
        [System.Serializable]
        private struct Info
        {
            [SerializeField]
            private PlayerStat m_stat;
            [SerializeField]
            private int m_value;

            public PlayerStat stat => m_stat;
            public int value => m_value;
        }

        [SerializeField]
        private Info[] m_infos;

        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
            var playerControlledObject = character.GetComponent<PlayerControlledObject>();
            if (playerControlledObject != null)
            {
                var stats = playerControlledObject.owner.stats;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    stats.AddStat(info.stat, info.value);
                }
            }
        }

        public void Stop(Character character)
        {
            var playerControlledObject = character.GetComponent<PlayerControlledObject>();
            if (playerControlledObject != null)
            {
                var stats = playerControlledObject.owner.stats;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    stats.AddStat(info.stat, -info.value);
                }
            }
        }
    }
}