using DChild.Gameplay.ArmyBattle.Units;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    public class ArmyGeneratorDebugger : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattalionGenerator m_generator;
        [SerializeField]
        private DamageType[] m_typesToSpawn;
        [SerializeField]
        private List<ArmyUnit> m_spawnedUnits;

        [Button]
        private void GenerateArmy()
        {
            if (m_spawnedUnits.Count > 0)
            {
                ClearGeneratedArmy();
            }


            m_spawnedUnits = m_generator.GenerateArmy(m_typesToSpawn);
        }

        [Button]
        private void ClearGeneratedArmy()
        {
            for (int i = m_spawnedUnits.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(m_spawnedUnits[i].gameObject);
            }
            m_spawnedUnits.Clear();
        }
    }
}