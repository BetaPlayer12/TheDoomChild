#if UNITY_EDITOR
using DChild.Serialization;
using UnityEngine;

namespace DChildEditor
{
    [System.Serializable]
    public class ZoneDataPostProcessor : IScenePostProcessor
    {
        [SerializeField]
        private ZoneDataHandle m_zoneDataHandle;

        public ZoneDataPostProcessor(ZoneDataHandle zoneDataHandle)
        {
            m_zoneDataHandle = zoneDataHandle;
        }

        public void Execute()
        {
            m_zoneDataHandle.enabled = true;
            m_zoneDataHandle.gameObject.SetActive(true);
            m_zoneDataHandle.RemoveNullInComponentSerializers();
        }
    }

}
#endif