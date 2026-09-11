using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Systems;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DChild.UI
{
    public class BlacksmithUI : MonoBehaviour
    {
        [SerializeField] private UIButton m_yesButton;
        [SerializeField] private TextMeshProUGUI m_subHeaderLabel;

        [SerializeField] private List<BlacksmithRequirementUI> m_requirementsUI;
        public List<BlacksmithRequirementUI> requirementsUI => m_requirementsUI;

        public EventAction<EventActionArgs> OnUpgradeConfirmed;

        public void SetSubHeaderLabel(WeaponLevel nextLevel) => m_subHeaderLabel.text = $"Weapon Level: {nextLevel - 1} -> {nextLevel}";
        public void OnConfirmationSuccess() => OnUpgradeConfirmed?.Invoke(this, EventActionArgs.Empty);
        public void ForceSetUIControls() => GameplaySystem.gamplayUIHandle.OverrideCurrentUIState(GameplayUIState.InteractableUI);
        public void SelectYesWhenShown() => StartCoroutine(SelectYesAfterDelay());
        private IEnumerator SelectYesAfterDelay()
        {
            yield return new WaitForSecondsRealtime(0.225f);

            if (EventSystem.current == null ||
                m_yesButton == null ||
                !m_yesButton.gameObject.activeInHierarchy)
            {
                yield break;
            }

            EventSystem.current.SetSelectedGameObject(null);
            yield return null;
            m_yesButton.Select();
        }
    }

}
