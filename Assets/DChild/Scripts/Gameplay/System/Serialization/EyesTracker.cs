using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class EyesTracker : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public class SaveData : ISaveData
        {
            [SerializeField, HideReferenceObjectPicker]
            public Dictionary<SerializeID, bool> m_entities = new Dictionary<SerializeID, bool>(new SerializeID.EqualityComparer());
            [SerializeField, MinValue(0)]
            public int m_entitiesKilledCount;

            ISaveData ISaveData.ProduceCopy()
            {
                    var copy = new SaveData();
                    var entities = new Dictionary<SerializeID, bool>(m_entities);
                    copy.m_entities = entities;
                    copy.m_entitiesKilledCount = m_entitiesKilledCount;
                    return copy;
            }
        }

        [System.Serializable]
        public class TrackerInfo
        {
            [SerializeField]
            private SerializeID m_id;
            [SerializeField]
            private Damageable m_entity;
            [HideInInspector]
            public bool m_isListeningToEntity;

            public SerializeID id => m_id;
            public Damageable entity => m_entity;
        }

        [SerializeField, HideReferenceObjectPicker]
        public TrackerInfo[] m_entities;
        private SaveData m_data;

        public ISaveData Save() => m_data;

        public void Load(ISaveData data)
        {
            m_data = (SaveData)data;
            for (int i = 0; i < m_entities.Length; i++)
            {
                var info = m_entities[i];
                if (m_data.m_entities.ContainsKey(info.id))
                {
                    if (m_data.m_entities[info.id])
                    {
                        if (info.m_isListeningToEntity == true)
                        {
                            info.entity.Destroyed -= OnEntityDeath;
                            info.entity.gameObject.SetActive(false);
                            info.m_isListeningToEntity = false;
                        }
                    }
                    else
                    {
                        if (info.m_isListeningToEntity == false)
                        {
                            info.entity.Destroyed += OnEntityDeath;
                            info.entity.gameObject.SetActive(true);
                            info.m_isListeningToEntity = true;
                        }
                    }
                }
                else
                {
                    info.entity.gameObject.SetActive(true);
                    if (info.m_isListeningToEntity == false)
                    {
                        info.entity.Destroyed += OnEntityDeath;
                        info.m_isListeningToEntity = true;
                    }
                }
            }
        }

        private void OnEntityDeath(object sender, EventActionArgs eventArgs)
        {
            var entity = (Damageable)sender;
            SerializeID id = new SerializeID();
            for (int i = 0; i < m_entities.Length; i++)
            {
                var info = m_entities[i];
                if (info.entity == entity)
                {
                    id = info.id;
                    info.entity.Destroyed -= OnEntityDeath;
                    info.m_isListeningToEntity = false;
                    break;
                }
            }

            if (m_data.m_entities.ContainsKey(id))
            {
                m_data.m_entities[id] = true;
            }
            else
            {
                m_data.m_entities.Add(id, true);
            }

            m_data.m_entitiesKilledCount++;
        }

        private void Awake()
        {
            if (m_data == null)
            {
                m_data = new SaveData();
                for (int i = 0; i < m_entities.Length; i++)
                {
                    m_entities[i].entity.Destroyed += OnEntityDeath;
                    m_entities[i].m_isListeningToEntity = true;
                }
            }
        }


    }
}
