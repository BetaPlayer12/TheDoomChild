using Holysoft.Event;

namespace Holysoft.UI
{
    public interface IHighlightableUIEvents
    {
        event EventAction<SelectedUIEventArgs> UIHighlight;
        event EventAction<SelectedUIEventArgs> UINormalize;
    }
}