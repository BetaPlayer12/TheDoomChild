using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    [System.Serializable]
    public class CombatArtUnlockHandle
    {
        private CombatArtList m_referenceList;
        private BattleAbilities m_progress;
        [SerializeField]
        private UIButton m_unlockButton;

        public void InitializeReferences(BattleAbilities progress, CombatArtList artList)
        {
            m_progress = progress;
            m_referenceList = artList;
        }

        public void UnlockCombatArt(CombatArt art, int level)
        {
            m_progress.SetAbilityLevel(art, level);
        }

        public void VerifyUnlockFunction(CombatArtSelectButton reference)
        {
            m_unlockButton.interactable = reference.currentState == CombatArtUnlockState.Unlockable;
        }

        public void DisableUnlockFunction()
        {
            m_unlockButton.interactable = false;
        }
    }

}