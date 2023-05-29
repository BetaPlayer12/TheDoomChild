using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class CombatArtsSaveData
    {
        [SerializeField]
        private int m_aetherPoints;
        [SerializeField]
        public int[] m_artLevels;

        public CombatArtsSaveData(int aetherPoints, int[] abilityLevels)
        {
            m_aetherPoints = aetherPoints;
            m_artLevels = abilityLevels;
        }

        public int aetherPoints => m_aetherPoints;

        public int GetArtsLevel(int index) => m_artLevels[index];
    }
}