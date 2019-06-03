using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(IUICanvas))]
    public abstract class UICanvasComponent : MonoBehaviour
    {
        private IUICanvas m_canvas;

        protected abstract void OnHide(object sender, EventActionArgs eventArgs);
        protected abstract void OnShow(object sender, EventActionArgs eventArgs);

        protected virtual void Awake()
        {
            m_canvas = GetComponent<IUICanvas>();
            m_canvas.CanvasHide += OnHide;
            m_canvas.CanvasShow += OnShow;
        }

        private void OnDestroy()
        {
            if (m_canvas != null)
            {
                m_canvas.CanvasHide -= OnHide;
                m_canvas.CanvasShow -= OnShow;
            }
        }

#if UNITY_EDITOR
        public void ForceEnable()
        {
            OnShow(this, EventActionArgs.Empty);
        }

        public void ForceDisable()
        {
            OnHide(this, EventActionArgs.Empty);
        }
#endif
    }
}