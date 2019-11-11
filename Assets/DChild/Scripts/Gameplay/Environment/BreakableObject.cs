﻿using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Breakable Object")]
    public class BreakableObject : MonoBehaviour, ISerializableComponent
    {
        public struct SaveData : ISaveData
        {
            public SaveData(bool isDestroyed) : this()
            {
                this.isDestroyed = isDestroyed;
            }

            public bool isDestroyed { get; }
        }

        [SerializeField]
        private Damageable m_object;
        [ShowInInspector, OnValueChanged("SetObjectState")]
        private bool m_isDestroyed;

        [SerializeField, TabGroup("On Destroy")]
        private UnityEvent m_onDestroy;
        [SerializeField, TabGroup("On Already Destroyed")]
        private UnityEvent m_onAlreadyDestroyed;
        [SerializeField, TabGroup("On Fix")]
        private UnityEvent m_onFix;

        private Vector2 m_forceDirection;
        private float m_force;
        private Debris m_instantiatedDebris;

        public void SetObjectState(bool isDestroyed)
        {
            m_isDestroyed = isDestroyed;
            if (m_isDestroyed == true)
            {
                m_onDestroy?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
            }
        }

        public void InstantiateDebris(GameObject debris)
        {
            var instance = Instantiate(debris, m_object.position, Quaternion.identity);
            m_instantiatedDebris = instance.GetComponent<Debris>();
            m_instantiatedDebris.SetInitialForceReference(m_forceDirection, m_force);
        }

        public void DestroyInstantiatedDebris()
        {
            if(m_instantiatedDebris != null)
            {
                Destroy(m_instantiatedDebris.gameObject);
            }
        }

        public void RecordForceReceived(Vector2 forceDirection, float force)
        {
            m_forceDirection = forceDirection;
            m_force = force;
        }

        public ISaveData Save() => new SaveData(m_isDestroyed);

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            if (saveData.isDestroyed)
            {
                m_onAlreadyDestroyed?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
            }
        }

        private void OnDestroyObject(object sender, EventActionArgs eventArgs)
        {
            m_onDestroy?.Invoke();
        }

        // Start is called before the first frame update
        private void Awake()
        {
            m_object.Destroyed += OnDestroyObject;
            if (m_isDestroyed == true)
            {
                m_onDestroy?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
            }
        }
    }
}
