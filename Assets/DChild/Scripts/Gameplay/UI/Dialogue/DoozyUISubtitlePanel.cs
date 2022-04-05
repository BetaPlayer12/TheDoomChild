using Doozy.Engine;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DChild.UI
{
    public class DoozyUISubtitlePanel : StandardUISubtitlePanel
    {
        private static List<Button> m_continueButtons = new List<Button>();
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

        public override void HideImmediate()
        {
            //GameEventMessage.SendEvent($"Dialogue End");
        }

        public void SetPanelStateToOpen()
        {
            panelState = PanelState.Open;
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
                panelState = PanelState.Closing;
                onClose.Invoke();
                //GameEventMessage.SendEvent($"Dialogue End");

                // Deselect ours:
                if (UnityEngine.EventSystems.EventSystem.current != null && selectables.Contains(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                }
            }
            ClearText();
            hasFocus = false;
        }

        private void Awake()
        {
            m_continueButtons.Add(continueButton);
            continueButton.interactable = isActive;
        }

        private void OnDestroy()
        {
            m_continueButtons.Remove(continueButton);
        }
    }

}