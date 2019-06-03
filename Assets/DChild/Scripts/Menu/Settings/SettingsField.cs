using Holysoft.Event;
using Holysoft.UI;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class SettingsField : MonoBehaviour, IFocusLockUI
    {
        private IHighlightableUIEvents m_highlightableUI;
        private ISelectableUI m_selectableUI;
        private UIHighlight[] m_transistions;
        private bool m_lockedFocus;

        public event EventAction<FocusLockUIEventArgs> FocusLock;
        public event EventAction<FocusLockUIEventArgs> FocusUnlock;

        public void SetFocusLock(bool value) => m_lockedFocus = value;

        private void OnNormalize(object sender, SelectedUIEventArgs eventArgs)
        {
            if (m_lockedFocus)
                return;
            for (int i = 0; i < m_transistions.Length; i++)
            {
                m_transistions[i].Normalize();
            }
        }

        private void OnHighlight(object sender, SelectedUIEventArgs eventArgs)
        {
            if (m_lockedFocus)
                return;
            for (int i = 0; i < m_transistions.Length; i++)
            {
                m_transistions[i].Highlight();
            }
        }

        private void OnDeselected(object sender, SelectedUIEventArgs eventArgs)
        {
            m_lockedFocus = false;
            FocusUnlock?.Invoke(this, new FocusLockUIEventArgs(this));
        }

        private void OnSelected(object sender, SelectedUIEventArgs eventArgs)
        {
            m_lockedFocus = true;
            FocusLock?.Invoke(this, new FocusLockUIEventArgs(this));
        }


        private void OnEnable()
        {
            m_lockedFocus = false;
            m_highlightableUI = GetComponentInChildren<IHighlightableUIEvents>();
            m_highlightableUI.UIHighlight += OnHighlight;
            m_highlightableUI.UINormalize += OnNormalize;
            m_selectableUI = GetComponentInChildren<ISelectableUI>();
            m_selectableUI.UISelected += OnSelected;
            m_selectableUI.UIDeselected += OnDeselected;
            m_transistions = GetComponentsInChildren<UIHighlight>();
        }


        private void OnDisable()
        {
            m_highlightableUI.UIHighlight -= OnHighlight;
            m_highlightableUI.UINormalize -= OnNormalize;
            m_highlightableUI = null;
            m_selectableUI.UISelected -= OnSelected;
            m_selectableUI.UIDeselected -= OnDeselected;
            m_selectableUI = null;
        }

       
    }
}