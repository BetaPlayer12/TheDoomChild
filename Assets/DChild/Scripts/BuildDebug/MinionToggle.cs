using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class MinionToggle : MonoBehaviour
    {
        public static MinionToggle minionToggleInstance;
        public event EventAction<EventActionArgs> OnToggle;
        public static bool toggleValue = true;

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

        private void Awake()
        {
            minionToggleInstance = this;
        }
    }
}
