using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyCharacterGroup
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private ArmyCharacterData[] m_members;

        public string name => m_name;

        public ArmyCharacterGroup(string name, ArmyCharacterData[] members)
        {
            m_name = name;
            m_members = members;
        }

        public ArmyCharacterData GetCharacter(int index)
        {
            return m_members[index];
        }
    }
}