using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Units
{
    [CreateAssetMenu(fileName = "ArmyGeneratorData", menuName = "DChild/Gameplay/Army/Army Generator Data")]
    public class ArmyGeneratorData : ScriptableObject
    {

        [SerializeField, Min(3)]
        private int m_maxUnitCount;
        [SerializeField]
        private ArmyBounds m_armyBounds;
        [SerializeField,TabGroup("Configuration","Melee"),HideLabel]
        private ArmyUnitGeneratorConfiguration m_meleeUnitGeneration;
        [SerializeField, TabGroup("Configuration", "Range"), HideLabel]
        private ArmyUnitGeneratorConfiguration m_rangeUnitGeneration;
        [SerializeField, TabGroup("Configuration", "Magic"), HideLabel]
        private ArmyUnitGeneratorConfiguration m_mageUnitGeneration;

        public int maxUnitCount => m_maxUnitCount;

        public ArmyBounds armyBounds => m_armyBounds;

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