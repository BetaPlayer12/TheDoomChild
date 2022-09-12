using DChild.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI.Map
{
    [RequireComponent(typeof(DynamicSerializableComponent))]
    public class MapRevealator : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public class SaveData : ISaveData
        {
            [SerializeField]
            private Dictionary<SerializeID, bool> m_isRevealedPair;

            public SaveData()
            {
                m_isRevealedPair = new Dictionary<SerializeID, bool>();
            }

            public SaveData(SerializeID ID, bool isRevealed)
            {
                m_isRevealedPair = new Dictionary<SerializeID, bool>();
                m_isRevealedPair.Add(new SerializeID(ID, false), isRevealed);
            }

            ISaveData ISaveData.ProduceCopy()
            {
                    var copy = new SaveData();
                    foreach (var key in m_isRevealedPair.Keys)
                    {
                        copy.SetData(key, m_isRevealedPair[key]);
                    }
                    return copy;
            }

            public bool IsRevealed(SerializeID ID) => m_isRevealedPair.ContainsKey(ID) ? m_isRevealedPair[ID] : false;

            public void SetData(SerializeID ID, bool isRevealed)
            {
                if (m_isRevealedPair.ContainsKey(ID))
                {
                    m_isRevealedPair[ID] = isRevealed;
                }
                else
                {
                    m_isRevealedPair.Add(new SerializeID(ID, false), isRevealed);
                }
            }
        }

        [SerializeField]
        private SerializeID m_mapID = new SerializeID(true);

        private bool m_isRevealed;

        private static SaveData m_saveData;
        private static bool m_isLoaded;

        public void Load(ISaveData data)
        {
            if (m_isLoaded == false)
            {
                m_saveData = ((SaveData)data);
                m_isLoaded = true;
            }
            m_isRevealed = m_saveData.IsRevealed(m_mapID);
            GetComponent<Collider2D>().enabled = !m_isRevealed;
        }

        public ISaveData Save() => m_saveData;
        public void Initialize()
        {
            m_isRevealed = false;
            GetComponent<Collider2D>().enabled = true;
        }
        private void Start()
        {
            if (m_saveData == null)
            {
                m_saveData = new SaveData(m_mapID, m_isRevealed);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                m_isRevealed = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}