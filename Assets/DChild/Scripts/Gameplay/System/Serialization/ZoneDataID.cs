using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "ZoneDataID", menuName = "DChild/Zone Data ID")]
    public class ZoneDataID : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private int m_ID;

        public int ID => m_ID;

        public ZoneDataID()
        {
#if UNITY_EDITOR
            var idKey = "latestZoneDataID";
            var latestZoneDataID = EditorPrefs.GetInt(idKey, 0);
            m_ID = latestZoneDataID + 1;
            EditorPrefs.SetInt(idKey, m_ID);
#endif
        }
    }
}