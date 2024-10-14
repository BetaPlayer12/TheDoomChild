using DChild.Gameplay.ArmyBattle.Units;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    public class ArmyBattalionGenerator : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private ArmyBattalionGeneratorData m_data;

        public List<ArmyUnit> GenerateArmy(List<DamageType> types)
        {
            var unitCountPerType = m_data.maxUnitCount / types.Count;
            List<ArmyUnit> units = new List<ArmyUnit>();

            for (int i = 0; i < types.Count; i++)
            {
                GenerateArmyType(m_data.GetConfiguration(types[i]), unitCountPerType, ref units);
            }

            return units;
        }

        public List<ArmyUnit> GenerateArmy(params DamageType[] types)
        {
            var unitCountPerType = m_data.maxUnitCount / types.Length;
            List<ArmyUnit> units = new List<ArmyUnit>();

            for (int i = 0; i < types.Length; i++)
            {
                GenerateArmyType(m_data.GetConfiguration(types[i]), unitCountPerType, ref units);
            }

            return units;
        }

        private void GenerateArmyType(ArmyUnitGeneratorConfiguration configuration, int count, ref List<ArmyUnit> spawnedUnits)
        {
            var bounds = m_data.armyBounds;
            bounds.center = transform.position;

            var gridPositions = configuration.GenerateGridPositions(bounds);
            for (int i = 0; i < count; i++)
            {
                var position = GetRandomPosition();
                var instance = Instantiate(configuration.GetRandomUnitTemplate());
                instance.gameObject.name += $"[{i}]";
                instance.transform.position = position;
                spawnedUnits.Add(instance.GetComponent<ArmyUnit>());
            }

            Vector2 GetRandomPosition()
            {
                var index = Random.Range(0, gridPositions.Count);
                var position = gridPositions[index];
                gridPositions.RemoveAt(index);
                return position;
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (m_data == null)
                return;

            var bounds = m_data.armyBounds;
            bounds.center = transform.position;

            bounds.DrawGizmos();
            m_data.GetConfiguration(DamageType.Melee).DrawGizmo(bounds);
            m_data.GetConfiguration(DamageType.Range).DrawGizmo(bounds);
            m_data.GetConfiguration(DamageType.Magic).DrawGizmo(bounds);
        }
    }
}