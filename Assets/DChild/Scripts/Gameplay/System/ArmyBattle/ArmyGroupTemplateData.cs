using DChild.Gameplay.ArmyBattle.SpecialSkills;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
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
        [SerializeField, ValueDropdown("GetArmyGroupCharacters", IsUniqueList = true)]
        private ArmyCharacterData[] m_requiredCharactersToBeAValidGroup;
        [SerializeField]
        private DamageType m_damageType;
        [SerializeField]
        private bool m_hasSpecialSkill;
        [OdinSerialize, ShowIf("m_hasSpecialSkill"), Indent, HideReferenceObjectPicker, HideLabel, BoxGroup("Special Skill Info")]
        private SpecialSkill m_specialSkill = new SpecialSkill();

        public int id => m_id;
        public ArmyCharacterGroup armyCharacterGroup => m_armyCharacterGroup;
        public ArmyCharacterData[] requiredCharactersToBeAValidGroup => m_requiredCharactersToBeAValidGroup;
        public SpecialSkill specialSkill => m_hasSpecialSkill ? m_specialSkill : null;
        public DamageType damageType => m_damageType;

        private IEnumerable GetArmyGroupCharacters()
        {
            ValueDropdownList<ArmyCharacterData> dropdownList = new ValueDropdownList<ArmyCharacterData>();
            for (int i = 0; i < m_armyCharacterGroup.memberCount; i++)
            {
                var character = m_armyCharacterGroup.GetCharacter(i);
                dropdownList.Add(character.name, character);
            }

            return dropdownList;
        }
    }
}