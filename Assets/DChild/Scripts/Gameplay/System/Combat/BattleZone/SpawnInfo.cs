using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.BattleZoneComponents
{

    [System.Serializable]
    public class SpawnInfo
    {
        [System.Serializable]
        public struct SpawnData
        {
            [SerializeField, MinValue(0)]
            private float m_spawnDelay;
            [SerializeField]
            private Vector2 m_spawnLocation;

            public float spawnDelay => m_spawnDelay;
            public Vector2 spawnLocation
            {
                get
                {
                    return m_spawnLocation;
                }

#if UNITY_EDITOR
                set
                {
                    m_spawnLocation = value;
                }
#endif
            }
        }

        [SerializeField, DrawWithUnity]
        private GameObject m_entity;
        [SerializeField]
        private GameObject m_spawnFX;
        [SerializeField,MinValue(0)]
        private float m_fxThenInstantiateDelay;

        [SerializeField, TableList(ShowIndexLabels =true)]
        private SpawnData[] m_datas;

        public GameObject entity => m_entity;
        public GameObject spawnFX => m_spawnFX;
        public float fxThenInstantiateDelay => m_fxThenInstantiateDelay;
        public SpawnData[] datas => m_datas;
    }
}