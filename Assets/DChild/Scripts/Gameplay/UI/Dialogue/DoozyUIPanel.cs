using DChild.Temp;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

namespace DChild.UI
{

    public class DoozyUIPanel : UIPanel
    {
        public override void Open()
        {
            if (panelState == PanelState.Open || panelState == PanelState.Opening) return;
            panelState = PanelState.Opening;
            onOpen.Invoke();

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
                // Deselect ours:
                if (UnityEngine.EventSystems.EventSystem.current != null && selectables.Contains(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject))
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        protected override void Start()
        {
            if (panelState == PanelState.Uninitialized)
            {
                switch (startState)
                {
                    case StartState.Open:
                        panelState = PanelState.Opening;
                        RefreshSelectablesList();
                        animatorMonitor.SetTrigger(showAnimationTrigger, OnVisible, false);
                        break;
                    case StartState.Closed:
                        panelState = PanelState.Closed;
                        break;
                    default:
                        if (gameObject.activeInHierarchy)
                        {
                            panelState = PanelState.Opening;
                            RefreshSelectablesList();
                            animatorMonitor.SetTrigger(showAnimationTrigger, OnVisible, false);
                        }
                        else
                        {
                            panelState = PanelState.Closed;
                        }
                        break;
                }
            }
        }
    }

}