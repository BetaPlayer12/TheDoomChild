using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Holysoft.Collections;
using DChild.Gameplay.Environment;
using UnityEditor;
using DChild.Serialization;
using Sirenix.Serialization;

namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "ZoneSlotFile", menuName = "DChild/Zone Slot File")]
    [System.Serializable]
    public class ZoneSlot : SerializedScriptableObject
    {
        [SerializeField]
        private SerializeID m_id;
        [OdinSerialize]
        private ZoneDataHandle.ZoneData m_zoneDatas;

        

        public SerializeID id => m_id;
        public ZoneDataHandle.ZoneData zoneDatas => m_zoneDatas;

        [Button]
        public void Reset()
        {
            m_zoneDatas = new ZoneDataHandle.ZoneData();
        }

        public ZoneSlot(SerializeID m_id)
        {
            this.m_id = m_id;
            m_zoneDatas = new ZoneDataHandle.ZoneData();
        }

        public void UpdateZoneSlot(ZoneDataHandle.ZoneData m_zoneSaves)
        {
            m_zoneDatas = m_zoneSaves;
        }
        public void UpdateZoneID(SerializeID m_zoneID)
        {
            m_id = m_zoneID;
        }

    }
}
