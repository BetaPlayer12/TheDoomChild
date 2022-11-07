using DChild.Temp;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.UI
{
    public class DoozyUISubtitlePanel : StandardUISubtitlePanel
    {
        private static List<UIButton> m_continueButtons = new List<UIButton>();
        private static bool isActive = true;

        public static AbstractTypewriterEffect currentTypeWriterEffect { get; private set; }

        public static void SetContinueButtonInteractibility(bool value)
        {
            isActive = value;
            for (int i = 0; i < m_continueButtons.Count; i++)
            {
                m_continueButtons[i].interactable = value;
            }
        }

        [SerializeField]
        private UIButton m_continueUIButton;
        private UIContainer m_container;

        public override void HideImmediate()
        {
            //GameEventMessage.SendEvent($"Dialogue End");
        }

        public void SetPanelStateToOpen()
        {
            panelState = PanelState.Open;
        }

        public void SetPanelStateToClosed()
        {
            panelState = PanelState.Closed;
        }

        public override void Open()
        {
            if (panelState == PanelState.Open || panelState == PanelState.Opening) return;
            panelState = PanelState.Opening;
            onOpen.Invoke();

            currentTypeWriterEffect = subtitleText.gameObject.GetComponent<AbstractTypewriterEffect>();
            //GameEventMessage.SendEvent($"Dialogue Start");

            // With quick panel changes, panel may not reach OnEnable/OnDisable before being reused.
            // Update panelStack here also to handle this case:
            PushToPanelStack();
        }

        public override void Close()
        {
            if (isOpen)
            {
                PopFromPanelStack();
                CancelInvoke();
                if (panelState == PanelState.Closed || panelState == PanelState.Closing) return;
                panelState = m_container.isHidden ? PanelState.Closed : PanelState.Closing;
                onClose.Invoke();

                // Deselect ours:
                if (UnityEngine.EventSystems.EventSystem.current != null && selectables.Contains(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                }
            }
            ClearText();
            hasFocus = false;
        }

        public override void HideContinueButton()
        {
            base.HideContinueButton();
            Tools.SetGameObjectActive(m_continueUIButton, false);
        }

        public override void ShowContinueButton()
        {
            base.ShowContinueButton();
            Tools.SetGameObjectActive(m_continueUIButton, true);
            if (m_continueUIButton != null && m_continueUIButton.onClickEvent.GetPersistentEventCount() == 0)
            {
                m_continueUIButton.onClickEvent.RemoveAllListeners();
                var fastForward = m_continueUIButton.GetComponent<StandardUIContinueButtonFastForward>();
                if (fastForward != null)
                {
                    m_continueUIButton.onClickEvent.AddListener(fastForward.OnFastForward);
                }
                else
                {
                    m_continueUIButton.onClickEvent.AddListener(OnContinue);
                }
            }
        }

        public override void Select(bool allowStealFocus = true)
        {
            base.Select(allowStealFocus);
            UITools.Select(m_continueUIButton, allowStealFocus);
        }

        protected override void SetUIElementsActive(bool value)
        {
            base.SetUIElementsActive(value);
            Tools.SetGameObjectActive(m_continueUIButton, false);
        }


        protected override void Awake()
        {
            if (m_continueUIButton)
            {
                m_continueButtons.Add(m_continueUIButton);
                m_continueUIButton.interactable = isActive;
            }
            m_container = GetComponent<UIContainer>();
            base.Awake();
        }

        private void OnDestroy()
        {
            m_continueButtons.Remove(m_continueUIButton);
        }
    }

}