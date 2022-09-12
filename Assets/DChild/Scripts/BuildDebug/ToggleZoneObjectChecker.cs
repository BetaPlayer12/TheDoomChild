using DChild.Gameplay;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleZoneObjectChecker : SerializedMonoBehaviour
    {
        

        [SerializeField]
        private Dictionary<ZoneObjectType, GameObject[]> m_zoneObjects = new Dictionary<ZoneObjectType, GameObject[]>();

        private void Toggle(object sender, EventActionArgs eventArgs)
        {
            UpdateZoneObjectInstanceState();
        }

        private void UpdateZoneObjectInstanceState()
        {
            var togglesCount = (int)ZoneObjectType._COUNT;
            for (int i = 0; i < togglesCount; i++)
            {
                var toggleType = (ZoneObjectType)i;

                if (ToggleZoneObject.GetToggleState(toggleType) == true)
                {
                    var list = m_zoneObjects[toggleType];
                    for (int x = 0; x < list.Length; x++)
                    {
                        list[x].SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("it worked");
                    var list = m_zoneObjects[toggleType];
                    for (int x = 0; x < list.Length; x++)
                    {
                        list[x].SetActive(false);
                    }
                }
            }

        }

        private void Start()
        {
            ToggleZoneObject.objectToggleInstance.OnToggle += Toggle;
            UpdateZoneObjectInstanceState();
        }

        private void OnDestroy()
        {

            ToggleZoneObject.objectToggleInstance.OnToggle -= Toggle;
        }
    }
}
