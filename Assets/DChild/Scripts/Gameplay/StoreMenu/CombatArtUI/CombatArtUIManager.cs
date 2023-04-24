using DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{

    public class CombatArtUIManager : MonoBehaviour
    {
        [SerializeField]
        private CombatArtList m_referenceList;
        [SerializeField]
        private BattleAbilities m_progressionReference;

        [SerializeField]
        private CombatArtUIDetail m_uiDetail;
        [SerializeField]
        private CombatArtUnlockHandle m_unlockArtHandler;

        private Dictionary<CombatArt, CombatArtSelectButton[]> m_abilityButtonPair;
        private CombatArtSelectRequirements[] m_artRequirements;

        private CombatArtSelectButton m_currentSelectedButton;

        public void Initialize()
        {
            m_unlockArtHandler.InitializeReferences(m_progressionReference, m_referenceList);
            InitializeButtonStates();
            ValidateButtonStates();
        }

        public void Select(CombatArtSelectButton button)
        {
            if (button == m_currentSelectedButton)
                return;

            m_currentSelectedButton = button;
            m_uiDetail.Display(m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock), m_currentSelectedButton.unlockLevel);

            m_unlockArtHandler.VerifyUnlockFunction(m_currentSelectedButton);
        }

        public void UnlockSelectedCombatArt()
        {
            if (m_currentSelectedButton.currentState != CombatArtUnlockState.Unlockable)
                return;

            m_unlockArtHandler.UnlockCombatArt(m_currentSelectedButton.skillUnlock, m_currentSelectedButton.unlockLevel);
            m_unlockArtHandler.DisableUnlockFunction();
            m_currentSelectedButton.SetState(CombatArtUnlockState.Unlocked);
            ValidateButtonStates();
        }

        private void PopulateCombatArtList(CombatArtSelectButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                button.Selected += OnCombatArtSelected;
                if (m_abilityButtonPair.TryGetValue(button.skillUnlock, out CombatArtSelectButton[] array))
                {
                    array[button.unlockLevel - 1] = button;
                }
                else
                {
                    var combatArtData = m_referenceList.GetCombatArtData(button.skillUnlock);
                    array = new CombatArtSelectButton[combatArtData.maxLevel];
                    array[button.unlockLevel - 1] = button;
                    m_abilityButtonPair.Add(button.skillUnlock, array);
                }
            }
        }

        private void ValidateButtonStates()
        {
            for (int i = 0; i < m_artRequirements.Length; i++)
            {
                m_artRequirements[i].ValidateButtonState();
            }
        }

        private void OnCombatArtSelected(CombatArtSelectButton obj)
        {
            Select(obj);
        }

        private void InitializeButtonStates()
        {
            var combatArtCount = (int)CombatArt._Count;
            for (int i = 0; i < combatArtCount; i++)
            {
                var combatArt = (CombatArt)i;
                InitializeButtonStates(combatArt);
            }
        }

        private void InitializeButtonStates(CombatArt combatArt)
        {
            if (m_abilityButtonPair.TryGetValue(combatArt, out CombatArtSelectButton[] array))
            {
                if (m_progressionReference.IsAbilityActivated(combatArt))
                {
                    var currentLevel = m_progressionReference.GetAbilityLevel(combatArt);
                    for (int k = 0; k < currentLevel; k++)
                    {
                        array[k].SetState(CombatArtUnlockState.Unlocked);
                    }

                }
                else
                {
                    for (int k = 0; k < array.Length; k++)
                    {
                        array[k].SetState(CombatArtUnlockState.Locked);
                    }
                }
            }
        }

        private void Awake()
        {
            m_abilityButtonPair = new Dictionary<CombatArt, CombatArtSelectButton[]>();
            var buttons = GetComponentsInChildren<CombatArtSelectButton>();
            PopulateCombatArtList(buttons);
            m_artRequirements = GetComponentsInChildren<CombatArtSelectRequirements>();
        }

        private void Start()
        {
            Initialize();
        }
    }

}