using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    [System.Serializable]
    public class CombatArtSelectButtonVisual
    {
        [SerializeField]
        private GameObject m_lockedUIAnimations;
        [SerializeField]
        private GameObject m_unlockableUIAnimations;
        [SerializeField]
        private GameObject m_unlockedUIAnimations;

        public void SetState(CombatArtUnlockState state)
        {
            DisableAllAnimations();
            switch (state)
            {
                case CombatArtUnlockState.Locked:
                    m_lockedUIAnimations.SetActive(true);
                    break;
                case CombatArtUnlockState.Unlockable:
                    m_unlockableUIAnimations.SetActive(true);
                    break;
                case CombatArtUnlockState.Unlocked:
                    m_unlockedUIAnimations.SetActive(true);
                    break;
            }
        }

        private void DisableAllAnimations()
        {
            m_lockedUIAnimations.SetActive(false);
            m_unlockableUIAnimations.SetActive(false);
            m_unlockedUIAnimations.SetActive(false);
        }
    }

}