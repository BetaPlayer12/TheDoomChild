using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.BattleZoneComponents
{
    [System.Serializable]
    public class WaveInfo
    {
#if UNITY_EDITOR
        public bool showGizmos;
#endif
        [SerializeField, MinValue(0)]
        private float m_numberOfEnemiesToNextWave;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private SpawnInfo[] m_spawnInfo;

        public SpawnInfo[] spawnInfo => m_spawnInfo;

        public float numberOfEnemiesToNextWave => m_numberOfEnemiesToNextWave;
    }
}