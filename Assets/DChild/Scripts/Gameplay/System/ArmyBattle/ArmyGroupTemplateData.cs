using DChild.Gameplay.ArmyBattle.SpecialSkills;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [CreateAssetMenu(fileName = "ArmyGroupData", menuName = "DChild/Gameplay/Army/Group")]
    public class ArmyGroupTemplateData : SerializedScriptableObject
    {
        [SerializeField]
        private int m_id;
        [SerializeField]
        private ArmyCharacterGroup m_armyCharacterGroup;
        [SerializeField]
        private DamageType m_damageType;
        [SerializeField]
        private bool m_hasSpecialSkill;
        [OdinSerialize, ShowIf("m_hasSpecialSkill"), Indent, HideReferenceObjectPicker,HideLabel,BoxGroup("Special Skill Info")]
        private SpecialSkill m_specialSkill = new SpecialSkill();

        public int id => m_id;
        public ArmyCharacterGroup armyCharacterGroup => m_armyCharacterGroup;
        public SpecialSkill specialSkill => m_hasSpecialSkill ? m_specialSkill : null;
        public DamageType damageType => m_damageType;
    }
}