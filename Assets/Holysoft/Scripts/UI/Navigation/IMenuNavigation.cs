using Holysoft.Event;
using Holysoft.UI;

namespace Holysoft.Menu
{
    public interface IMenuNavigation
    {
        UICanvas mainCanvas { get; }
        event UnityEventAction<UICanvas> CanvasOpen;
    }
}
