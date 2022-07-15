using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Menu.Inputs
{
    [System.Serializable]
    public class InputIconHandle : MonoBehaviour
    {
        private static GamepadIconData xboxIconData;
        private static GamepadIconData ps4IconData;
        private static InputControlsDetector inputControlsDetector;

        public static int inputIndex
        {
            get
            {
                return inputControlsDetector.isUsingGamepad ? 1 : 0;
            }
        }
        public static event EventAction<InputIconChangeEventArgs> UpdateInputIcons;

        public static GamepadIconData GetCurrentInputIcon()
        {
            if (inputControlsDetector.isUsingGamepad)
            {
                return xboxIconData;
            }
            else
            {
                return null;
            }
        }

        [SerializeField]
        private GamepadIconData m_xboxIconData;
        [SerializeField]
        private GamepadIconData m_ps4IconData;
        [SerializeField]
        private InputControlsDetector m_inputControlsDetector;


        public void Awake()
        {
            xboxIconData = m_xboxIconData;
            ps4IconData = m_ps4IconData;
            inputControlsDetector = m_inputControlsDetector;
            m_inputControlsDetector.InputControlChange += OnInputControlChange;
        }

        private void OnInputControlChange(object sender, EventActionArgs eventArgs)
        {
            using(Cache< InputIconChangeEventArgs> cacheEvent = Cache<InputIconChangeEventArgs>.Claim())
            {
                cacheEvent.Value.Set(GetCurrentInputIcon(), inputIndex);
                UpdateInputIcons?.Invoke(this,cacheEvent.Value);
                cacheEvent.Release();
            }
        }
    }
}