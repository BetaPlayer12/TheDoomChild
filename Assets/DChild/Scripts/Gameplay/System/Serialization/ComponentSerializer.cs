using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace DChild.Serialization
{
    public class ComponentSerializer : MonoBehaviour
    {
        [SerializeField, DisableInPlayMode]
        private SerializeID m_id = new SerializeID(true);
        private ISerializableComponent m_component;

        [InfoBox("This GameObject must be ACTIVE", InfoMessageType = InfoMessageType.Warning, VisibleIf = "@gameObject.activeSelf")]
        public SerializeID ID => m_id;

        public ISaveData SaveData() => m_component.Save();
        public void LoadData(ISaveData data) => m_component.Load(data);

        public void Initiatlize()
        {
            m_component = GetComponent<ISerializableComponent>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (m_id.value == SerializeID.defaultValue)
                {
                    Debug.LogError($"{gameObject.name} Component Serializer ID is not on the Database");
                }
            }
        }
#endif
    }
}