﻿
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild
{
    public class EnumElement<T> where T : Enum, IConvertible
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [SerializeField]
        [ReadOnly]
        protected T m_name;
        public T name { set => m_name = value; } 

        public EnumElement(T m_name)
        {
            this.m_name = m_name;
        }

        public void SetName(T name) => m_name = name;
#endif

        public EnumElement()
        {
        }
    }

    [System.Serializable]
    public class EnumElement<T, U> : EnumElement<T> where T : Enum, IConvertible
    {
        [SerializeField]
        private U m_value;

        public EnumElement()
        {
        }

#if UNITY_EDITOR
        public EnumElement(T name): base(name)
        {

        }
#endif

        public U value { get => m_value; }
    }

}