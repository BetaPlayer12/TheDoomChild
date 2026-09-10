using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
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
        private Characters.Players.CombatArts m_progressionReference;

        [SerializeField]
        private CombatArtUIDetail m_uiDetail;
        [SerializeField]
        private CombatArtSelectorHighlight m_selectorHighlight;
        [SerializeField]
        private CombatArtUnlockHandle m_unlockArtHandler;

        [SerializeField]
        private CombatArtSelectButton m_firstSelected;

        private Dictionary<CombatArt, CombatArtSelectButton[]> m_abilityButtonPair;
        private CombatArtSelectRequirements[] m_artRequirements;

        private CombatArtSelectButton m_currentSelectedButton;

        public void Initialize()
        {
            ValidateButtonVisuals();

            m_selectorHighlight.Initialize();
            m_unlockArtHandler.UnlockSuccessful -= OnUnlockSuccessFull;
            m_unlockArtHandler.UnlockSuccessful += OnUnlockSuccessFull;
            m_unlockArtHandler.InitializeReferences(m_progressionReference, m_referenceList);
            m_unlockArtHandler.ResetUnlockProgress();
            Select(m_firstSelected);

        }

        public void SyncButtonStates()
        {
            //InitializeButtonStates();
            ValidateButtonVisuals();
        }

        public void Select(CombatArtSelectButton button)
        {
            if (button == m_currentSelectedButton)
                return;

            m_currentSelectedButton = button;
            var combatArtData = m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock);
            m_uiDetail.Display(combatArtData, m_currentSelectedButton.unlockLevel);
            m_selectorHighlight.Highlight(button);

            m_unlockArtHandler.ResetUnlockProgress();

            var availableSkillPoints = m_progressionReference.skillPoints.points;
            var combatArtCost = combatArtData.GetCombatArtLevelData(m_currentSelectedButton.unlockLevel).cost;
            m_unlockArtHandler.VerifyUnlockFunction(m_currentSelectedButton, availableSkillPoints >= combatArtCost);
            //static bool CanAfford(CombatSkillPoints points, CombatArtLevelData combatArtLevelData) => points.points >= combatArtLevelData.cost;
        }

        public void StartUnlockSelectedCombatArt()
        {
            if (!CanUnlockSelectedCombatArt())
                return;

            m_unlockArtHandler.StartUnlockProgress();
        }

        private bool CanUnlockSelectedCombatArt()
        {
            if (m_currentSelectedButton == null || m_currentSelectedButton.currentState != CombatArtUnlockState.Unlockable)
                return false;

            var combatArtData = m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock);
            var combatArtCost = combatArtData.GetCombatArtLevelData(m_currentSelectedButton.unlockLevel).cost;
            return m_progressionReference.skillPoints.points >= combatArtCost;
        }

        public void ResetUnlock()
        {
            m_unlockArtHandler.ResetUnlockProgress();
            m_unlockArtHandler.ResetBranchingUIProgressors();
        }

        private void OnUnlockSuccessFull()
        {
            m_unlockArtHandler.DisableUnlockFunction();
            ValidateButtonVisuals();

            var combatArtData = m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock);
            var combatArtLevelData = combatArtData.GetCombatArtLevelData(m_currentSelectedButton.unlockLevel);
            m_progressionReference.skillPoints.AddPoint(-combatArtLevelData.cost);
            m_currentSelectedButton.SetState(CombatArtUnlockState.Unlocked);
        }

        private void PopulateCombatArtList(CombatArtSelectButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                button.OnButtonSelected += Select;

                if (m_abilityButtonPair.TryGetValue(button.skillUnlock, out CombatArtSelectButton[] array))
                {
                    array[button.unlockLevel - 1] = button;
                }

                else
                {
                    var combatArtData = m_referenceList.GetCombatArtData(button.skillUnlock);
                    try
                    {
                        array = new CombatArtSelectButton[combatArtData.maxLevel];
                        array[button.unlockLevel - 1] = button;
                        m_abilityButtonPair.Add(button.skillUnlock, array);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Combat Arts Reference File Doesn't Have {button.skillUnlock}");
                    }
                }
            }
        }

        #region Combat Art Button State Handling
        private void ValidateButtonVisuals()
        {
            foreach (var requirementButton in m_artRequirements)
            {
                requirementButton.ValidateButtonState(m_progressionReference);
            }
        }

        //private void InitializeButtonStates()
        //{
        //    var combatArtCount = (int)CombatArt._Count;
        //    for (int i = 0; i < combatArtCount; i++)
        //    {
        //        var combatArt = (CombatArt)i;
        //        InitializeArtLevelButtons(combatArt);
        //    }
        //}

        //private void InitializeArtLevelButtons(CombatArt combatArt)
        //{
        //    if (!m_abilityButtonPair.TryGetValue(combatArt, out CombatArtSelectButton[] levelButtons))
        //        return;

        //    if (!m_progressionReference.IsAbilityActivated(combatArt))
        //    {
        //        for (int k = 0; k < levelButtons.Length; k++)
        //            levelButtons[k].SetState(CombatArtUnlockState.Locked);
        //        return;
        //    }

        //    var currentLevel = m_progressionReference.GetAbilityLevel(combatArt);
        //    for (int k = 0; k < currentLevel; k++)
        //    {
        //        levelButtons[k].SetState(CombatArtUnlockState.Unlocked);
        //    }
        //    return;
        //}
        #endregion


        #region Boilerplate & Editor Utils
#if UNITY_EDITOR
        [ContextMenu("Editor/Update SelectButtonVisuals")]
        private void UpdateSelectButtonVisuals()
        {
            var buttons = GetComponentsInChildren<CombatArtSelectButton>();
            foreach (var button in buttons)
            {
                var data = m_referenceList.GetCombatArtData(button.skillUnlock);
                var levelData = data.GetCombatArtLevelData(button.unlockLevel);
                button.DisplayAs(levelData);
            }
        }
#endif

        private void Awake()
        {
            m_abilityButtonPair = new Dictionary<CombatArt, CombatArtSelectButton[]>();
            var buttons = GetComponentsInChildren<CombatArtSelectButton>();
            PopulateCombatArtList(buttons);
            m_artRequirements = GetComponentsInChildren<CombatArtSelectRequirements>();
        }
    }
        #endregion

}
