using UnityEngine;
using Sirenix.OdinInspector;
namespace DChild.Serialization
{

    public class ComponentSerializer : MonoBehaviour
    {
        [InfoBox("This GameObject must be ACTIVE", InfoMessageType = InfoMessageType.Warning)]
        [SerializeField]
        private int m_id;
        private ISerializableComponent m_component;

        public int ID => m_id;

        public ISaveData SaveData() => m_component.Save();
        public void LoadData(ISaveData data) => m_component.Load(data);

        private void Awake()
        {
            m_component = GetComponent<ISerializableComponent>();
        }
    }
}