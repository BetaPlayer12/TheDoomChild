using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [CreateAssetMenu(fileName = "ArmyGroupData", menuName = "DChild/Gameplay/Army/Group")]
    public class ArmyGroupTemplateData : SerializedScriptableObject
    {
        [SerializeField]
        private ArmyCharacterGroup m_armyCharacterGroup;
        [SerializeField]
        private SpecialSkill m_specialSkill;
        [SerializeField]
        private DamageType m_damageType;

        public ArmyCharacterGroup armyCharacterGroup => m_armyCharacterGroup;
        public SpecialSkill specialSkill => m_specialSkill;
        public DamageType damageType => m_damageType;
    }
}