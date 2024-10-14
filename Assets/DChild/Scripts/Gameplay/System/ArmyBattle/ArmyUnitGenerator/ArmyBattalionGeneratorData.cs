using DChild.Gameplay.ArmyBattle.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    [CreateAssetMenu(fileName = "ArmyBattalionGeneratorData", menuName = "DChild/Gameplay/Army/Army Battalion Generator Data")]
    public class ArmyBattalionGeneratorData : ScriptableObject
    {
        [SerializeField, Min(3)]
        private int m_maxUnitCount;
        [SerializeField]
        private ArmyBattalionBounds m_armyBounds;
        [SerializeField,TabGroup("Configuration","Melee"),HideLabel]
        private ArmyUnitGeneratorConfiguration m_meleeUnitGeneration;
        [SerializeField, TabGroup("Configuration", "Range"), HideLabel]
        private ArmyUnitGeneratorConfiguration m_rangeUnitGeneration;
        [SerializeField, TabGroup("Configuration", "Magic"), HideLabel]
        private ArmyUnitGeneratorConfiguration m_mageUnitGeneration;

        public int maxUnitCount => m_maxUnitCount;

        public ArmyBattalionBounds armyBounds => m_armyBounds;

        public ArmyUnitGeneratorConfiguration GetConfiguration(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Melee:
                    return m_meleeUnitGeneration;
                case DamageType.Range:
                    return m_rangeUnitGeneration;
                case DamageType.Magic:
                    return m_mageUnitGeneration;
                default:
                    return null;
            }
        }
    }
}