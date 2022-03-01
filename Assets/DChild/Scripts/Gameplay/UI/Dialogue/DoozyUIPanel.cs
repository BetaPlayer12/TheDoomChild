using Doozy.Engine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DChild.UI
{
    public class DoozyUIPanel : UIPanel
    {
        public override void Open()
        {
            if (panelState == PanelState.Open || panelState == PanelState.Opening) return;
            panelState = PanelState.Opening;
            onOpen.Invoke();

            //GameEventMessage.SendEvent($"Dialogue Open");

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

                //GameEventMessage.SendEvent($"Dialogue Close");

                onClose.Invoke();
                // Deselect ours:
                if (UnityEngine.EventSystems.EventSystem.current != null && selectables.Contains(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
    }

}