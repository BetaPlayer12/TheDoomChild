using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleZoneObject : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private ZoneObjectType m_type;
        
        public static ToggleZoneObject objectToggleInstance;
        public event EventAction<EventActionArgs> OnToggle;
        public static bool toggleValue = true;
        private static Dictionary<ZoneObjectType, bool> m_zoneState = new Dictionary<ZoneObjectType, bool>();
        private static bool m_dictionaryInitialized=true;
        public bool value => toggleValue;

        public static bool GetToggleState(ZoneObjectType zoneObjectType)
        {

            return m_zoneState[zoneObjectType];
        }

        [Button]
        public void ToggleOn()
        {
                m_zoneState[m_type]= true;
            OnToggle?.Invoke(this, EventActionArgs.Empty);
        }

        [Button]
        public void ToggleOff()
        {
            m_zoneState[m_type] = false;
            OnToggle?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            objectToggleInstance = this;
            var togglesCount = (int)ZoneObjectType._COUNT;
            for (int i = 0; i < togglesCount; i++)
            {
                m_zoneState.Add((ZoneObjectType)i, true);
            }


        }
    }
}
