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
                if (m_button.currentState != CombatArtSelectButton.State.Unlocked)
                {
                    m_button.SetState(CombatArtSelectButton.State.Unlockable);
                }
            }
            else
            {
                m_button.SetState(CombatArtSelectButton.State.Locked);
            }
        }

        private bool AreRequiredArtsUnlocked()
        {
            for (int i = 0; i < m_requirements.Length; i++)
            {
                if (m_requirements[i].currentState != CombatArtSelectButton.State.Unlocked)
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