using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class StatPercentageChange : IStatusEffectModule
    {
        public enum StatReferenceType
        {
            Base,
            Total
        }

        [System.Serializable]
        public struct Info
        {
            [SerializeField]
            private PlayerStat m_stat;
            [SerializeField]
            private StatReferenceType m_statReferenceType;
            [SerializeField]
            private float m_value;

            public PlayerStat stat => m_stat;
            public StatReferenceType statReferenceType => m_statReferenceType;
            public float value => m_value;
        }

        [SerializeField]
        private Info[] m_infos;

        private int[] m_values;

        public StatPercentageChange(Info[] infos)
        {
            m_infos = infos;
            m_values = new int[infos.Length];
        }

        public IStatusEffectModule GetInstance() => new StatPercentageChange(m_infos);

        public void Start(Character character)
        {
            var playerControlledObject = character.GetComponent<PlayerControlledObject>();
            if (playerControlledObject != null)
            {
                var stats = playerControlledObject.owner.stats;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    int value = 0;
                    if (info.statReferenceType == StatReferenceType.Base)
                    {
                        value = Mathf.CeilToInt(stats.GetBaseStat(info.stat) * info.value);
                    }
                    else
                    {
                        value = Mathf.CeilToInt(stats.GetTotalStat(info.stat) * info.value);
                    }
                    stats.AddStat(info.stat, value);
                    m_values[i] = value;
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
                    stats.AddStat(info.stat, -m_values[i]);
                }
            }
        }
    }
}