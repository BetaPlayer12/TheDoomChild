using System;
using Holysoft.Event;

namespace Holysoft.Menu
{
    public interface IWindowTransistionEvent
    {
        event EventAction<WindowTransistionEventArgs> TransistionStart;
        event EventAction<WindowTransistionEventArgs> TransistionEnd;
    }
}