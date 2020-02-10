﻿using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{

    public class Door : MonoBehaviour, ISerializableComponent
    {
        public struct SaveData : ISaveData
        {
            public SaveData(bool isOpen)
            {
                this.isOpen = isOpen;
            }

            public bool isOpen { get; }
        }

        [ShowInInspector, OnValueChanged("OnStateChange")]
        private bool m_isOpen;
        private Animator m_animator;

        public void Open()
        {
            m_isOpen = true;
            m_animator.SetTrigger("Open");
        }

        public void Close()
        {
            m_isOpen = false;
            m_animator.SetTrigger("Close");
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