using UnityEngine;

namespace Holysoft.UI
{

    public class SwitchHighlightData<T> : ScriptableObject
    {
        [SerializeField]
        private T m_deselected;
        [SerializeField]
        private T m_selected;

        public T deselected => m_deselected;
        public T selected => m_selected;
    }
}