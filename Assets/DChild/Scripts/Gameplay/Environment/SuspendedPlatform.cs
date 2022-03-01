using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class SuspendedPlatform : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isSuspended;

            public SaveData(bool isSuspended)
            {
                m_isSuspended = isSuspended;
            }

            public bool isSuspended => m_isSuspended;

            ISaveData ISaveData.ProduceCopy() =>new SaveData(m_isSuspended);
        }

        [SerializeField, OnValueChanged("OnSuspensionChanged")]
        private bool m_isSuspended = true;

        [TabGroup("Main", "Suspended")]
        [SerializeField, HorizontalGroup("Main/Suspended/Group")]
        private Vector2 m_suspendedLocalPosition;
        [SerializeField, TabGroup("Main", "Suspended")]
        private UnityEvent m_onSuspend;

        [SerializeField, TabGroup("Main", "Transistition")]
        private UnityEvent m_toUnsuspend;

        [TabGroup("Main", "Unsuspended")]
        [SerializeField, HorizontalGroup("Main/Unsuspended/Group")]
        private Vector2 m_unsuspendedLocalPosition;
        [SerializeField, TabGroup("Main", "Unsuspended")]
        private UnityEvent m_onUnsuspend;

        private Rigidbody2D m_rigidbody;
        private RigidbodyConstraints2D m_constraints;

        public ISaveData Save() => new SaveData(m_isSuspended);

        public void Load(ISaveData data)
        {
            m_isSuspended = ((SaveData)data).isSuspended;
            SetAs(m_isSuspended);
        }
        public void Initialize()
        {
            m_isSuspended = true;
            SetAs(m_isSuspended);
        }
        public void Unsuspend()
        {
            if (m_isSuspended)
            {
                m_isSuspended = false;
                m_toUnsuspend?.Invoke();
                m_rigidbody.constraints = m_constraints;
                m_rigidbody.WakeUp();
            }
        }

        public void SetAs(bool isSuspended)
        {
            if (m_rigidbody == null)
            {
                m_rigidbody = GetComponent<Rigidbody2D>();
            }


            m_isSuspended = isSuspended;
            if (m_isSuspended)
            {
                transform.localPosition = m_suspendedLocalPosition;
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                m_onSuspend?.Invoke();
            }
            else
            {
                transform.localPosition = m_unsuspendedLocalPosition;
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                m_onUnsuspend?.Invoke();
            }
        }
        
       
        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_constraints = m_rigidbody.constraints;
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

#if UNITY_EDITOR
        [ResponsiveButtonGroup("Main/Suspended/Group/Button"), Button("Use Current")]
        private void UseCurrentForSuspended()
        {
            m_suspendedLocalPosition = transform.localPosition;
        }

        [ResponsiveButtonGroup("Main/Unsuspended/Group/Button"), Button("Use Current")]
        private void UseCurrentForUnuspended()
        {
            m_unsuspendedLocalPosition = transform.localPosition;
        }

        private void OnSuspensionChanged()
        {
            SetAs(m_isSuspended);
        }
#endif
    }
}