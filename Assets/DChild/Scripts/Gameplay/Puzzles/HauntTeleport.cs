﻿using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_deathCounter);
        }

        [SerializeField]
        private int m_deathCounter;
        [SerializeField]
        public int respawnDelay;
        [SerializeField]
        private Damageable m_entity;
        [SerializeField]
        private int m_totalDeaths;
        [SerializeField]
        private UnityEvent m_afterfinaldefeat;
        [SerializeField,VariablePopup(true)]
        private string m_deathCounterDatabaseVariable;
        [SerializeField]
        private Dictionary<int, Vector2> m_info = new Dictionary<int, Vector2>();

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
        public void Initialize()
        {
            m_deathCounter = 0;
            UpdateEntityPosition();

        }
        public void UseIndex(int index)
        {
            m_deathCounter = index;
            UpdateEntityPosition();
        }

        private void OnEntityDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_deathCounter++;
            DialogueLua.SetVariable(m_deathCounterDatabaseVariable, m_deathCounter);
            StartCoroutine(DelayUpdatePosition());
            
        }
        IEnumerator DelayUpdatePosition()
        {
            yield return new WaitForSeconds(respawnDelay);
            UpdateEntityPosition();

        }
        private void UpdateEntityPosition()
        {
            if (m_info.ContainsKey(m_deathCounter))
            {
                m_entity.gameObject.SetActive(true);
                m_entity.transform.position = m_info[m_deathCounter];
                m_entity.Heal(9999);
                m_entitybrain.ResetAI();
            }
            else
            {
                m_entity.gameObject.SetActive(false);
            }
            if(m_totalDeaths+1== m_deathCounter)
            {
                m_afterfinaldefeat?.Invoke();
            }
        }

        void Start()
        {
            
            m_entitybrain = m_entity.GetComponent<IResetableAIBrain>();
            m_entity.Destroyed += OnEntityDestroyed;
            m_deathCounter = DialogueLua.GetVariable(m_deathCounterDatabaseVariable).asInt;
            UpdateEntityPosition();
        }

        private void OnDestroy()
        {
            m_entity.Destroyed -= OnEntityDestroyed;
        }
    }
}