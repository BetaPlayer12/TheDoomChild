using Holysoft.Event;

namespace Holysoft.UI
{
    public interface IUICanvas
    {
        event EventAction<EventActionArgs> CanvasShow;
        event EventAction<EventActionArgs> CanvasHide;
    }
}