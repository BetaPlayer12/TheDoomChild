using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class ZoneDataList
    {
        [OdinSerialize]
        private Dictionary<int, IZoneSaveData> m_saveDatas;

        public void UpdateZoneData(ZoneDataID ID, IZoneSaveData data)
        {
            var IDvalue = ID.value;
            if (m_saveDatas.ContainsKey(ID.value))
            {
                m_saveDatas[IDvalue] = data;
            }
            else
            {
                m_saveDatas.Add(IDvalue, data);
            }
        }

        public IZoneSaveData GetZoneData(ZoneDataID ID)
        {
            var IDvalue = ID.value;
            return m_saveDatas.ContainsKey(IDvalue) ? m_saveDatas[IDvalue] : null;
        }
    }
}