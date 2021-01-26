using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class UIHighlight : MonoBehaviour, IUIHighlight
    {
        public event EventAction<EventActionArgs> HighlightEnd;
        public event EventAction<EventActionArgs> NormalizeEnd;

        [Button,HideInEditorMode]
        public abstract void Highlight();

        [Button, HideInEditorMode]
        public abstract void Normalize();

        [Button, HideInEditorMode]
        public abstract void UseHighlightState();

        [Button, HideInEditorMode]
        public abstract void UseNormalizeState();
    }
}