using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Serialization;
using Sirenix.Serialization;
using static DChild.Serialization.ZoneDataHandle;
using DChild;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif
using System;

namespace DChildDebug.Serialization
{
    [CreateAssetMenu(fileName = "ZoneDataFile", menuName = "DChild/Debug/ZoneDataFile")]
    public class ZoneDataFile : SerializedScriptableObject
    {
        [SerializeField, ReadOnly]
        private SerializeID m_ID = new SerializeID(true);
        [OdinSerialize, ReadOnly, HideReferenceObjectPicker, HideLabel]
        private ZoneData m_zoneData = new ZoneData();

#if UNITY_EDITOR
        public void Set(SerializeID ID, ZoneData zoneData)
        {
            if (m_ID != ID)
            {
                m_ID = new SerializeID(ID, false);
                var connection = DChildDatabase.GetSerializeIDConnection();
                connection.Initialize();
                var context = connection.GetContext(ID);
                string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                FileUtility.RenameAsset(this, assetPath, context.Replace(" ", string.Empty));
                connection.Close();
                AssetDatabase.SaveAssets();
            }
            m_zoneData = zoneData;
        }
#endif
    }

}