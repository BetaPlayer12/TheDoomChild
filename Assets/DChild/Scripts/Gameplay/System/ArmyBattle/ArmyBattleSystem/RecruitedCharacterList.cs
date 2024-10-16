using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class RecruitedCharacterList
    {
        [SerializeField]
        private int[] m_recruitedCharacterIds;

        public RecruitedCharacterList(ArmyCharacterData[] characters)
        {
            m_recruitedCharacterIds = new int[characters.Length];
            for (int i = 0; i < m_recruitedCharacterIds.Length; i++)
            {
                m_recruitedCharacterIds[i] = characters[i].ID;
            }
        }
        public bool HasCharacter(ArmyCharacterData character)
        {
            for (int i = 0; i < m_recruitedCharacterIds.Length; i++)
            {
                if (m_recruitedCharacterIds[i] == character.ID)
                    return true;
            }
            return false;
        }
    }
}