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
        [SerializeField, LabelText("@\"Members \" + GetMemberNames()"), AssetSelector]
        private ArmyCharacterData[] m_members;

        public string name => m_name;
        public int memberCount => m_members.Length;

        public ArmyCharacterGroup(string name, ArmyCharacterData[] members)
        {
            m_name = name;
            m_members = members;
        }

        public ArmyCharacterData GetCharacter(int index)
        {
            return m_members[index];
        }

        private string GetMemberNames()
        {
            string characterNames = "(";
            for (int i = 0; i < m_members.Length; i++)
            {
                if (m_members[i] == null)
                    continue;

                characterNames += m_members[i].name;
                if (i < m_members.Length - 1)
                {
                    characterNames += " | ";
                }
            }
            characterNames += ")";

            return characterNames;
        }
    }
}