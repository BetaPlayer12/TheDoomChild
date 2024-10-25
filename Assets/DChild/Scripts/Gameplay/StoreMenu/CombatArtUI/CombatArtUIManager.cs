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

        private Dictionary<CombatArt, CombatArtSelectButton[]> m_abilityButtonPair;
        private CombatArtSelectRequirements[] m_artRequirements;

        private CombatArtSelectButton m_currentSelectedButton;

        public void Initialize()
        {
            m_selectorHighlight.Initialize();
            m_unlockArtHandler.UnlockSuccessful += OnUnlockSuccessFull;
            m_unlockArtHandler.InitializeReferences(m_progressionReference, m_referenceList);
            m_unlockArtHandler.ResetUnlockProgress();
            SyncButtonStates();
        }

        public void SyncButtonStates()
        {
            InitializeButtonStates();
            ValidateButtonStates();
        }

        public void Select(CombatArtSelectButton button)
        {
            if (button == m_currentSelectedButton)
                return;

            m_currentSelectedButton = button;
            var combatArtData = m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock);
            m_uiDetail.Display(combatArtData, m_currentSelectedButton.unlockLevel);
            m_selectorHighlight.Highlight(button);

            if (CanAfford(m_progressionReference.skillPoints, combatArtData.GetCombatArtLevelData(m_currentSelectedButton.unlockLevel)))
            {
                m_unlockArtHandler.VerifyUnlockFunction(m_currentSelectedButton);
            }
            else
            {
                m_unlockArtHandler.DisableUnlockFunction();
            }
            m_unlockArtHandler.ResetUnlockProgress();

            bool CanAfford(CombatSkillPoints points, CombatArtLevelData combatArtLevelData) => points.points >= combatArtLevelData.cost;
        }

        public void StartUnlockSelectedCombatArt()
        {
            if (m_currentSelectedButton.currentState != CombatArtUnlockState.Unlockable)
                return;

            m_unlockArtHandler.StartUnlockProgress();
        }

        public void ResetUnlock() => m_unlockArtHandler.ResetUnlockProgress();

        private void OnUnlockSuccessFull()
        {
            m_unlockArtHandler.DisableUnlockFunction();

            var combatArtData = m_referenceList.GetCombatArtData(m_currentSelectedButton.skillUnlock);
            var combatArtLevelData = combatArtData.GetCombatArtLevelData(m_currentSelectedButton.unlockLevel);
            m_progressionReference.skillPoints.AddPoint(-combatArtLevelData.cost);
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
                    try
                    {
                        array = new CombatArtSelectButton[combatArtData.maxLevel];
                        array[button.unlockLevel - 1] = button;
                        m_abilityButtonPair.Add(button.skillUnlock, array);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError($"Combat Arts Reference File Doesn't Have {button.skillUnlock}");
                    }
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
            Debug.Log(m_artRequirements.Length);
        }

        private void Start()
        {
            Initialize();
        }
    }

}