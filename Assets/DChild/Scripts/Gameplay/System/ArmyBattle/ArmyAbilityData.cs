using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAbilityData", menuName = "DChild/Gameplay/Army/Ability")]
    public class ArmyAbilityData : SerializedScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField, TextArea]
        private string m_description;
        [SerializeField]
        private bool m_useCharactersForUseCount;
        [SerializeField]
        private ArmyCharacterData[] m_members;
        [SerializeField]
        private IArmyAbilityEffect[] m_effects = new IArmyAbilityEffect[0];

        public string abilityName => m_name;
        public string description => m_description;
        public bool useCharactersForUseCount => m_useCharactersForUseCount;
        public int membersCount => m_members.Length;
        public ArmyCharacterData GetMember(int index) => m_members[index];

        public void ApplyEffect(Army owner, Army opponent)
        {
            for (int i = 0; i < m_effects.Length; i++)
            {
                m_effects[i].ApplyEffect(owner, opponent);
            }
        }
    }
}