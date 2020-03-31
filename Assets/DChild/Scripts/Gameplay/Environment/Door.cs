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

        [ShowInInspector, OnValueChanged("OnStateChange")]
        private bool m_isOpen;
        [SerializeField]
        private bool m_multiDirectionParameterValue;
        [SerializeField,ShowIf("m_multiDirectionParameterValue")]
        private string m_multiDirectionParameter;
        private Animator m_animator;

        public void Open()
        {
            m_isOpen = true;
            if(m_multiDirectionParameter != string.Empty)
            {
                m_animator.SetBool(m_multiDirectionParameter, m_multiDirectionParameterValue);
            }
            m_animator.SetTrigger("Open");
        }

        public void Close()
        {
            m_isOpen = false;
            if (m_multiDirectionParameter != string.Empty)
            {
                m_animator.SetBool(m_multiDirectionParameter, m_multiDirectionParameterValue);
            }
            m_animator.SetTrigger("Close");
        }

        public void SetAsOpen(bool open)
        {
            if (m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }

            m_isOpen = open;
            if (m_multiDirectionParameter != string.Empty)
            {
                m_animator.SetBool(m_multiDirectionParameter, m_multiDirectionParameterValue);
            }
            m_animator.SetTrigger("Force");
            if (m_isOpen)
            {
               
                m_animator.SetTrigger("Open");
            }
            else
            {
                m_animator.SetTrigger("Close");
            }
        }

        public virtual void Load(ISaveData data)
        {
            if(m_animator == null)
            {
                m_animator = GetComponentInChildren<Animator>();
            }

            m_isOpen = ((SaveData)data).isOpen;
            if (m_isOpen)
            {
                if (m_multiDirectionParameter != string.Empty)
                {
                    m_animator.SetBool(m_multiDirectionParameter, m_multiDirectionParameterValue);
                }
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
        }

#if UNITY_EDITOR
        private void OnStateChange()
        {
            string open = "Open";
            string close = "Close";
            m_animator.ResetTrigger(open);
            m_animator.ResetTrigger(close);
            m_animator.SetTrigger(m_isOpen ? open : close);
        }
#endif
    }

}