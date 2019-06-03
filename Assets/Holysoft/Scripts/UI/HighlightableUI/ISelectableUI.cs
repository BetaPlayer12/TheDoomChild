using Holysoft.Event;

namespace Holysoft.UI
{
    public struct SelectedUIEventArgs : IEventActionArgs
    {
        public SelectedUIEventArgs(ISelectableUI selectableUI) : this()
        {
            this.selectableUI = selectableUI;
        }

        public ISelectableUI selectableUI { get; }
    }

    public interface ISelectableUI
    {
        event EventAction<SelectedUIEventArgs> UISelected;
        event EventAction<SelectedUIEventArgs> UIDeselected;
        void Select();
        void Deselect();
    }
}