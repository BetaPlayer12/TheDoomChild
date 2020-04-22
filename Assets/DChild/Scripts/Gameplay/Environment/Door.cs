using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{

    public class Door : MonoBehaviour, ISerializableComponent
    {

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isOpen)
            {
                this.m_isOpen = isOpen;
            }

            [SerializeField]
            private bool m_isOpen;

            public bool isOpen => m_isOpen;
        }

        [SerializeField, OnValueChanged("OnStateChange")]
        private bool m_isOpen;
        [SerializeField]
        private bool m_multiDirectionParameterValue;
        [SerializeField, ShowIf("m_multiDirectionParameterValue")]
        private string m_multiDirectionParameter;
        private Animator m_animator;

        public void Open()
        {
            m_isOpen = true;
            m_animator.SetBool("Force", false);
            ApplyMultiParameter();
            m_animator.SetTrigger("Open");
        }

        public void Close()
        {
            m_isOpen = false;
            ApplyMultiParameter();
            m_animator.SetBool("Force", false);
            m_animator.SetTrigger("Close");
        }

        public void SetAsOpen(bool open)
        {
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }

            ApplyMultiParameter();
            if (m_isOpen != open)
            {
                m_animator.SetBool("Force", true);
                if (open)
                {

                    m_animator.SetTrigger("Open");
                }
                else
                {
                    m_animator.SetTrigger("Close");
                }
            }
            m_isOpen = open;
        }

        private void ApplyMultiParameter()
        {
            if (m_multiDirectionParameter != string.Empty)
            {
                m_animator.SetBool(m_multiDirectionParameter, m_multiDirectionParameterValue);
            }
        }

        public virtual void Load(ISaveData data)
        {
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }

            m_isOpen = ((SaveData)data).isOpen;
            if (m_isOpen)
            {
                ApplyMultiParameter();
                m_animator.SetTrigger("Force");
                m_animator.SetTrigger("Open");
            }
        }

        public ISaveData Save() => new SaveData(m_isOpen);

        private void Awake()
        {
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }
            if (m_isOpen)
            {
                SetAsOpen(true);
            }
        }

#if UNITY_EDITOR
        private void OnStateChange()
        {
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }
            string open = "Open";
            string close = "Close";
            m_animator.ResetTrigger(open);
            m_animator.ResetTrigger(close);
            ApplyMultiParameter();
            m_animator.SetTrigger(m_isOpen ? open : close);
        }
#endif
    }

}