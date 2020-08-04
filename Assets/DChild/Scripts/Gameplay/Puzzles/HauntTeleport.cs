using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Puzzles
{
    public class HauntTeleport : SerializedMonoBehaviour, ISerializableComponent
    {
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private int m_deathCounter;

            public SaveData(int m_deathCounter)
            {
                this.m_deathCounter = m_deathCounter;
            }

            public int deathCounter => m_deathCounter;

            ISaveData ISaveData.ProduceCopy => new SaveData(m_deathCounter);
        }

        [SerializeField]
        private int m_deathCounter;
        [SerializeField]
        private Damageable m_entity;
        [SerializeField]
        private Dictionary<int, Transform> m_dictionary;

        private IResetableAIBrain m_entitybrain;

        public ISaveData Save()
        {
            return new SaveData(m_deathCounter);
        }

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_deathCounter=saveData.deathCounter;
            UpdateEntityPosition();
        }

        private void OnEntityDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_deathCounter++;
            UpdateEntityPosition();
        }

        private void UpdateEntityPosition()
        {
            if (m_dictionary.ContainsKey(m_deathCounter))
            {
                m_entity.gameObject.SetActive(true);
                m_entity.transform.position = m_dictionary[m_deathCounter].position;
                m_entitybrain.ResetAI();
            }
            else
            {
                m_entity.gameObject.SetActive(false);
            }
        }

        void Start()
        {
            m_entitybrain = m_entity.GetComponent<IResetableAIBrain>();
            m_entity.Destroyed += OnEntityDestroyed;
        }

        private void OnDestroy()
        {
            m_entity.Destroyed -= OnEntityDestroyed;
        }

      
    }
}