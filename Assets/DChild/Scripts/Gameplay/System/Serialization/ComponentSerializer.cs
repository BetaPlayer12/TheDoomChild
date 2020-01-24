using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace DChild.Serialization
{
    public class ComponentSerializer : MonoBehaviour
    {
        [InfoBox("This GameObject must be ACTIVE", InfoMessageType = InfoMessageType.Warning)]
        [SerializeField, ReadOnly]
        private int m_id;
        private ISerializableComponent m_component;

        public int ID => m_id;

        public ISaveData SaveData() => m_component.Save();
        public void LoadData(ISaveData data) => m_component.Load(data);

        public void Initiatlize()
        {
            m_component = GetComponent<ISerializableComponent>();
        }

        private void OnValidate()
        {
            List<ComponentSerializer> list = new List<ComponentSerializer>(FindObjectsOfType<ComponentSerializer>());
            if (m_id == 0)
            {
                AssignNewID();
            }
            else
            {
                bool hasDuplicate = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (ID == list[i].ID && this != list[i])
                    {
                        hasDuplicate = true;
                    }
                }

                if (hasDuplicate)
                {
                    AssignNewID();
                }
            }

            void AssignNewID()
            {
                m_id = list.Max(x => x.ID) + 1;
            }
        }
    }
}