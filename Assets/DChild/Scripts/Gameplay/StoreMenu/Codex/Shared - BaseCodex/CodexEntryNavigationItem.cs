using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DChild.Menu.Codex
{
    [DisallowMultipleComponent]
    public class CodexEntryNavigationItem : MonoBehaviour, IMoveHandler, ISelectHandler
    {
        private CodexEntryNavigationController m_controller;
        private Selectable m_selectable;

        internal RectTransform rectTransform => transform as RectTransform;
        internal Selectable selectable => m_selectable ??= GetComponent<Selectable>();

        public void OnMove(AxisEventData eventData)
        {
            ResolveController();
            if (eventData.used || m_controller == null)
                return;

            if (m_controller.TryMove(this, eventData.moveDir))
                eventData.Use();
        }

        public void OnSelect(BaseEventData eventData)
        {
            ResolveController();
            m_controller?.RecordSelection(this);
        }

        internal void DisableBuiltInNavigation()
        {
            if (selectable == null)
                return;

            var navigation = selectable.navigation;
            navigation.mode = Navigation.Mode.None;
            selectable.navigation = navigation;
        }

        internal bool IsAvailable()
        {
            return isActiveAndEnabled && selectable != null && selectable.IsActive() && selectable.IsInteractable();
        }

        internal void Select() => selectable?.Select();

        private void ResolveController()
        {
            if (m_controller != null)
                return;

            m_controller = GetComponentInParent<CodexEntryNavigationController>(true);
            m_controller?.Register(this);
        }

        private void Awake()
        {
            m_selectable = GetComponent<Selectable>();
            ResolveController();
        }

        private void OnEnable() => ResolveController();
    }
}
