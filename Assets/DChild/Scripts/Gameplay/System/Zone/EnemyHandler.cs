using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Serialization
{
    [System.Serializable]
    public class EnemyHandler
    {
        [SerializeField]
        [ListDrawerSettings(CustomAddFunction = "Initialize", NumberOfItemsPerPage = 5)]
        private EnemySerializer[] m_serializedEnemies;

        public void ResetEnemies()
        {
            for (int i = 0; i < m_serializedEnemies.Length; i++)
            {
                m_serializedEnemies[i].InitializeEnemyAs(true);
            }
        }

#if UNITY_EDITOR
        private void Initialize()
        {
            m_serializedEnemies = GameObject.FindObjectsOfType<EnemySerializer>();
        }
#endif
    }
}
