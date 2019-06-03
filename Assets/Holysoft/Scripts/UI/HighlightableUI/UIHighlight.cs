using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class UIHighlight : MonoBehaviour, IUIHighlight
    {
        public event EventAction<EventActionArgs> HighlightEnd;
        public event EventAction<EventActionArgs> NormalizeEnd;

        public abstract void Highlight();
        public abstract void Normalize();
        public abstract void UseHighlightState();
        public abstract void UseNormalizeState();
    }
}