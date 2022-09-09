using Doozy.Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace DChild.Menu
{
    public class PressAnykeyHandle : MonoBehaviour
    {
        [SerializeField]
        private SignalSender m_signal;
        [SerializeField]
        private UnityEvent m_events;
        void Start()
        {
            InputSystem.onAnyButtonPress.CallOnce(OnAnyKeyDown);
        }

        private void OnAnyKeyDown(InputControl obj)
        {
            m_signal?.SendSignal();
            m_events?.Invoke();
        }
    }

}