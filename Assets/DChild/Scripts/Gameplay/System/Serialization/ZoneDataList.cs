using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class ZoneDataList
    {
        [SerializeField, ShowInInspector]
        private Dictionary<ZoneDataID, IZoneSaveData> m_saveDatas;

        public void UpdateZoneData(ZoneDataID ID, IZoneSaveData data)
        {
            if (m_saveDatas.ContainsKey(ID))
            {
                m_saveDatas[ID] = data;
            }
            else
            {
                m_saveDatas.Add(ID, data);
            }
        }

        public IZoneSaveData GetZoneData(ZoneDataID ID) => m_saveDatas.ContainsKey(ID) ? m_saveDatas[ID] : null;
    }
}