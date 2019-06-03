using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public struct NavigationButtonEventArgs : IEventActionArgs
    {
        public NavigationButtonEventArgs(int buttonID) : this()
        {
            this.buttonID = buttonID;
        }

        public int buttonID { get; }
    }

    [RequireComponent(typeof(Button))]
    public class NavigationButton : MonoBehaviour
    {
        [InfoBox("This Automatically connects to the button's Onclick No need to manually connect it")]
        [SerializeField]
        [ReadOnly]
        protected int m_buttonID;

        public event EventAction<NavigationButtonEventArgs> ButtonClick;

        public void SetButtonID(int id) => m_buttonID = id;
        private void Click() => ButtonClick?.Invoke(this, new NavigationButtonEventArgs(m_buttonID));

        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Click);
        }
    }
}
