using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI.CombatArts
{
    [System.Serializable]
    public class CombatArtSelectButtonVisual
    {
        [SerializeField]
        private Image m_icon;

        [SerializeField]
        private GameObject m_lockedUIAnimations;
        [SerializeField]
        private GameObject m_unlockableUIAnimations;
        [SerializeField]
        private GameObject m_unlockedUIAnimations;


        private UIButton m_button;
        private BaseUISelectableAnimator[] m_lockedUIAnimators;
        private BaseUISelectableAnimator[] m_unlockableUIAnimators;
        private BaseUISelectableAnimator[] m_unlockedUIAnimators;

        public void DisplayAs(CombatArtLevelData artLevelData)
        {
            m_icon.sprite = artLevelData.icon;
        }

        public void SetState(CombatArtUnlockState state)
        {
            DisableAllAnimations();
            switch (state)
            {
                case CombatArtUnlockState.Locked:
                    m_lockedUIAnimations.SetActive(true);
                    EnableAnimator(m_lockedUIAnimators);
                    UseAnimator(m_lockedUIAnimators, m_button.selectedState.stateType);
                    break;
                case CombatArtUnlockState.Unlockable:
                    m_unlockableUIAnimations.SetActive(true);
                    EnableAnimator(m_unlockableUIAnimators);
                    UseAnimator(m_unlockableUIAnimators, m_button.selectedState.stateType);
                    break;
                case CombatArtUnlockState.Unlocked:
                    m_unlockedUIAnimations.SetActive(true);
                    EnableAnimator(m_unlockedUIAnimators);
                    UseAnimator(m_unlockedUIAnimators, m_button.selectedState.stateType);
                    break;
            }
        }

        public void Initialize(UIButton uIButton)
        {
            m_button = uIButton;
            m_lockedUIAnimators = m_lockedUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>();
            m_unlockableUIAnimators = m_unlockableUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>();
            m_unlockedUIAnimators = m_unlockedUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>();
        }

        private void UseAnimator(BaseUISelectableAnimator[] animators, UISelectionState buttonState)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].Play(buttonState);
            }
        }

        private void DisableAnimator(BaseUISelectableAnimator[] animators)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetController(null);
            }
        }

        private void EnableAnimator(BaseUISelectableAnimator[] animators)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetController(m_button);
            }
        }

        private void DisableAllAnimations()
        {
            m_lockedUIAnimations.SetActive(false);
            DisableAnimator(m_lockedUIAnimators);
            m_unlockableUIAnimations.SetActive(false);
            DisableAnimator(m_unlockableUIAnimators);
            m_unlockedUIAnimations.SetActive(false);
            DisableAnimator(m_unlockedUIAnimators);
        }
    }

}