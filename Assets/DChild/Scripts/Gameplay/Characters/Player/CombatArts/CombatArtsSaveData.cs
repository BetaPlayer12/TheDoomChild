using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class CombatArtsSaveData
    {
        [SerializeField]
        private CombatArtLevel.SaveData m_level;
        [SerializeField]
        private int m_skillPoints;
        [SerializeField]
        public int[] m_artLevels;

        public CombatArtsSaveData(CombatArtLevel.SaveData level, int skillPoints, int[] abilityLevels)
        {
            m_level = level;
            m_skillPoints = skillPoints;
            m_artLevels = abilityLevels;
        }

        public int skillPoints => m_skillPoints;

        public CombatArtLevel.SaveData level => m_level;

        public int GetArtsLevel(int index)
        {
            if (index >= m_artLevels.Length)
            {
                return 0;
            }
            return m_artLevels[index];
        }
    }
}