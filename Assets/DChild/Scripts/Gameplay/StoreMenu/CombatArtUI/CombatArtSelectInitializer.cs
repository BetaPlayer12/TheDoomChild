using DChild.Gameplay.Characters.Players;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    [System.Serializable]
    public class CombatArtSelectionInitializer
    {
        private CombatArtList m_referenceList;
        private Characters.Players.CombatArts m_progressionReference;
        private CombatArtSelectRequirements[] m_artRequirements;

        private Dictionary<CombatArt, CombatArtSelectButton[]> m_abilityButtonPair;
        private Dictionary<CombatArtSelectButton, bool> m_hasPerformedOperationCache;

        public CombatArtSelectionInitializer(Characters.Players.CombatArts progressionReference, CombatArtSelectButton[] selections, CombatArtSelectRequirements[] requirements)
        {
            m_progressionReference = progressionReference;
            m_artRequirements = requirements;
            m_hasPerformedOperationCache = new Dictionary<CombatArtSelectButton, bool>();
            for (int i = 0; i < selections.Length; i++)
            {
                m_hasPerformedOperationCache.Add(selections[i], false);
            }

        }

        public void InitializeSelectionStates()
        {
            ResetOperationCache();



            for (int i = 0; i < m_artRequirements.Length; i++)
            {
                var requirement = m_artRequirements[i];
            }
        }

        private void ResetOperationCache()
        {
            foreach (var key in m_hasPerformedOperationCache.Keys)
            {
                m_hasPerformedOperationCache[key] = false;
            }
        }
    }

}