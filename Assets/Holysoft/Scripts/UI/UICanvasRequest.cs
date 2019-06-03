using Holysoft.Event;

namespace Holysoft.UI
{
    public interface UICanvasRequest
    {
        EventAction<bool, EventActionArgs> RequestCanvasShow { set; }
        EventAction<bool, EventActionArgs> RequestCanvasHide { set; }
    }
}