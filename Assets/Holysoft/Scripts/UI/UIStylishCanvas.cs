using Holysoft.Event;
using Sirenix.OdinInspector;

namespace Holysoft.UI
{
    public abstract class UIStylishCanvas : UICanvas, UICanvasRequest
    {
        public EventAction<bool, EventActionArgs> RequestCanvasShow { private get; set; }
        public EventAction<bool, EventActionArgs> RequestCanvasHide { private get; set; }

        protected override bool canHide => base.canHide && (RequestCanvasHide?.Invoke(this, EventActionArgs.Empty) ?? true);
        protected override bool canShow => base.canShow && (RequestCanvasShow?.Invoke(this, EventActionArgs.Empty) ?? true);

#if UNITY_EDITOR
        [PropertySpace]
        [Button("Request Hide")]
        protected void RequestHide()
        {
            RequestCanvasHide?.Invoke(this, EventActionArgs.Empty);
        }

        [Button("Request Show")]
        protected void RequestShow()
        {
            RequestCanvasShow?.Invoke(this, EventActionArgs.Empty);
        }
#endif
    }
}