using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    [RequireComponent(typeof(CombatArtSelectButton))]
    public class CombatArtSelectRequirements : MonoBehaviour
    {
        [SerializeField]
        private CombatArtSelectButton[] m_requirements;
        private CombatArtSelectButton m_button;

        public void ValidateButtonState()
        {
            if (AreRequiredArtsUnlocked())
            {
                if (m_button.currentState != CombatArtUnlockState.Unlocked)
                {
                    m_button.SetState(CombatArtUnlockState.Unlockable);
                }
            }
            else
            {
                m_button.SetState(CombatArtUnlockState.Locked);
            }
            m_button.ForceVisualSync();
        }

        private bool AreRequiredArtsUnlocked()
        {
            for (int i = 0; i < m_requirements.Length; i++)
            {
                if (m_requirements[i].currentState != CombatArtUnlockState.Unlocked)
                {
                    return false;
                }
            }
            return true;
        }

        private void Awake()
        {
            m_button = GetComponent<CombatArtSelectButton>();
        }
    }

}