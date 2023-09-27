using DChild.Gameplay.Characters.Players;
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
        private UIButton m_controller;
        [SerializeField]
        private Image m_icon;

        [SerializeField]
        private GameObject m_lockedUIAnimations;
        [SerializeField]
        private GameObject m_unlockableUIAnimations;
        [SerializeField]
        private GameObject m_unlockedUIAnimations;

        [SerializeField]
        private BaseUISelectableAnimator[] m_lockedUISelectables;
        [SerializeField]
        private BaseUISelectableAnimator[] m_unlockableUISelectables;
        [SerializeField]
        private BaseUISelectableAnimator[] m_unlockedUISelectables;

        public void Initialize()
        {
            m_lockedUISelectables = m_lockedUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>(true);
            m_unlockableUISelectables = m_unlockableUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>(true);
            m_unlockedUISelectables = m_unlockedUIAnimations.GetComponentsInChildren<BaseUISelectableAnimator>(true);
        }

        public void DisplayAs(CombatArtLevelData artLevelData)
        {
            m_icon.sprite = artLevelData.icon;
        }

        public void RevertToNormalSelectionState()
        {
            m_controller.SetState(Doozy.Runtime.UIManager.UISelectionState.Normal);
        }

        public void SetState(CombatArtUnlockState state)
        {
            DisableAllAnimations();
            switch (state)
            {
                case CombatArtUnlockState.Locked:
                    m_lockedUIAnimations.SetActive(true);
                    for (int i = 0; i < m_lockedUISelectables.Length; i++)
                    {
                        m_lockedUISelectables[i].SetController(m_controller);
                    }
                    break;
                case CombatArtUnlockState.Unlockable:
                    m_unlockableUIAnimations.SetActive(true);
                    for (int i = 0; i < m_unlockableUISelectables.Length; i++)
                    {
                        m_unlockableUISelectables[i].SetController(m_controller);
                    }
                    break;
                case CombatArtUnlockState.Unlocked:
                    m_unlockedUIAnimations.SetActive(true);
                    for (int i = 0; i < m_unlockedUISelectables.Length; i++)
                    {
                        m_unlockedUISelectables[i].SetController(m_controller);
                    }
                    break;
            }
        }

        private void DisableAllAnimations()
        {
            m_lockedUIAnimations.SetActive(false);
            m_unlockableUIAnimations.SetActive(false);
            m_unlockedUIAnimations.SetActive(false);

            for (int i = 0; i < m_lockedUISelectables.Length; i++)
            {
                m_lockedUISelectables[i].SetController(null);
            }

            for (int i = 0; i < m_unlockableUISelectables.Length; i++)
            {
                m_unlockableUISelectables[i].SetController(null);
            }

            for (int i = 0; i < m_unlockedUISelectables.Length; i++)
            {
                m_unlockedUISelectables[i].SetController(null);
            }
        }
    }

}