using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Menu
{
    public struct WindowTransistionEventArgs : IEventActionArgs
    {
        public WindowTransistionEventArgs(UICanvas toClose, UICanvas toOpen) : this()
        {
            this.toClose = toClose;
            this.toOpen = toOpen;
        }

        public UICanvas toClose { get; private set; }
        public UICanvas toOpen { get; private set; }

    }

    public abstract class WindowTransistion : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        protected bool m_manual;
#endif
        [SerializeField]
        [ShowIf("m_manual")]
        protected UICanvas m_toClose;
        [SerializeField]
        [ShowIf("m_manual")]
        protected UICanvas m_toOpen;

        public event EventAction<WindowTransistionEventArgs> TransistionStart;
        public abstract void StartTransistion();

        public virtual void SetCanvases(UICanvas toClose, UICanvas toOpen)
        {
            m_toClose = toClose;
            m_toOpen = toOpen;
        }

        protected void CallTransistionStart()
        {
            TransistionStart?.Invoke(this, new WindowTransistionEventArgs(m_toClose,m_toOpen));
        }
    }
}