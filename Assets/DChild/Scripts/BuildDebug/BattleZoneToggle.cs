using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class BattleZoneToggle : MonoBehaviour, IToggleDebugBehaviour
    {
        public static BattleZoneToggle instance { get; private set; }
        public static bool toggleValue = true;
        public event EventAction<EventActionArgs> OnToggle;

        public bool value => toggleValue;

        [Button]
        public void ToggleOn()
        {
            toggleValue = true;
            OnToggle?.Invoke(this, EventActionArgs.Empty);
        }

        [Button]
        public void ToggleOff()
        {
            toggleValue = false;
            OnToggle?.Invoke(this, EventActionArgs.Empty);
        }

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
