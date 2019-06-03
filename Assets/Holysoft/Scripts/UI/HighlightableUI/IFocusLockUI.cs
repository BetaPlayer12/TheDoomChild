using Holysoft.Event;

namespace Holysoft.UI
{
    public struct FocusLockUIEventArgs : IEventActionArgs
    {
        public FocusLockUIEventArgs(IFocusLockUI focusLockUI) : this()
        {
            this.focusLockUI = focusLockUI;
        }

        public IFocusLockUI focusLockUI { get; }
    }

    public interface IFocusLockUI
    {
        event EventAction<FocusLockUIEventArgs> FocusLock;
        event EventAction<FocusLockUIEventArgs> FocusUnlock;
        void SetFocusLock(bool value);
    }
}