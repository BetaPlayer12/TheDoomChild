using DChild.Gameplay.Items;
using Holysoft.Event;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Inventories
{
    public abstract class QuickItemSelections : SerializedMonoBehaviour
    {
        public abstract int itemCount { get; }

        public event EventAction<EventActionArgs> SelectionUpdate;
        public event EventAction<EventActionArgs> SelectionDetailsUpdate;

        public bool HasItems() => itemCount > 0;
        public abstract bool IsInSelections(ItemData data);
        public abstract void UpdateSelection();
        public abstract IStoredItem GetItem(int index);
        public abstract int FindIndexOf(IStoredItem item);

        protected void InvokeSelectionUpdate() => SelectionUpdate?.Invoke(this, EventActionArgs.Empty);
        protected void InvokeSelectionDetailsUpdate() => SelectionDetailsUpdate?.Invoke(this, EventActionArgs.Empty);
    }
}
