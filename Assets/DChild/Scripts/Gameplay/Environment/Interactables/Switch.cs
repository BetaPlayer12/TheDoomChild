﻿/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Characters;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Interactable/Switch")]
    public class Switch : MonoBehaviour, IHitToInteract, ISerializableComponent
    {
        public enum Type
        {
            Toggle,
            OneTime,
            Trigger
        }

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isOpen)
            {
                this.m_isTriggered = isOpen;
            }

            [SerializeField]
            private bool m_isTriggered;

            public bool isTriggered => m_isTriggered;
        }

        [SerializeField, OnValueChanged("OnTypeChanged")]
        private Type m_type;
        [SerializeField]
        private Collider2D m_collider;
#if UNITY_EDITOR
        [SerializeField, ReadOnly]
#endif
        private bool m_isOn;

        [SerializeField]
        private UnityEvent m_startAsOnState;
        [SerializeField, HideIf("m_hideStartAsOffState")]
        private UnityEvent m_startAsOffState;
        [SerializeField]
        private UnityEvent m_onState;
        [SerializeField, HideIf("m_hideOffState")]
        private UnityEvent m_offState;

        public event EventAction<HitDirectionEventArgs> OnHit;

        public Vector2 position => transform.position;

        public ISaveData Save()
        {
            return new SaveData(m_isOn);
        }

        public void Load(ISaveData data)
        {
            m_isOn = ((SaveData)data).isTriggered;
            if (m_isOn)
            {
                m_startAsOnState?.Invoke();
                if (m_type == Type.OneTime)
                {
                    m_collider.enabled = false;
                }
            }
            else
            {
                m_startAsOffState?.Invoke();
                m_collider.enabled = true;
            }
        }

        public void Interact(HorizontalDirection direction)
        {
            OnHit?.Invoke(this, new HitDirectionEventArgs(direction));
            Interact();
        }

        public void SetAs(bool value)
        {
            m_isOn = value;
            if (m_isOn)
            {
                m_startAsOnState?.Invoke();
                switch (m_type)
                {
                    case Type.OneTime:
                        m_collider.enabled = false;
                        break;
                    case Type.Trigger:
                        m_isOn = false;
                        break;
                }
            }
            else
            {
                m_startAsOffState?.Invoke();
                if (m_type == Type.OneTime)
                {
                    m_collider.enabled = true;
                }
            }
        }

        public void Interact()
        {
            switch (m_type)
            {
                case Type.OneTime:
                    if (m_isOn == false)
                    {
                        m_onState?.Invoke();
                        m_isOn = true;
                        m_collider.enabled = false;
                    }
                    break;
                case Type.Toggle:
                    m_isOn = !m_isOn;
                    if (m_isOn)
                    {
                        m_onState?.Invoke();
                    }
                    else
                    {
                        m_offState?.Invoke();
                    }
                    break;
                case Type.Trigger:
                    m_isOn = false;
                    m_onState?.Invoke();
                    break;
            }
        }

        private void Awake()
        {
            switch (m_type)
            {
                case Type.OneTime:
                    m_offState = null;
                    break;
                case Type.Trigger:
                    m_offState = null;
                    m_startAsOffState = null;
                    break;
            }
        }

        private void Start()
        {
            SetAs(m_isOn);
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_hideStartAsOffState;
        [SerializeField, HideInInspector]
        private bool m_hideOffState;

        private struct GizmoInfo
        {
            public Vector3 position { get; }
            public Color color;

            public GizmoInfo(Vector3 position, Color color) : this()
            {
                this.position = position;
                this.color = color;
            }
        }

        private void OnTypeChanged()
        {
            switch (m_type)
            {
                case Type.OneTime:
                    m_hideStartAsOffState = false;
                    m_hideOffState = true;
                    m_offState.RemoveAllListeners();
                    break;
                case Type.Toggle:
                    m_hideStartAsOffState = false;
                    m_hideOffState = false;
                    break;
                case Type.Trigger:
                    m_hideStartAsOffState = true;
                    m_startAsOffState.RemoveAllListeners();
                    m_hideOffState = true;
                    m_offState.RemoveAllListeners();
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {

            Dictionary<GameObject, GizmoInfo> m_gizmosToDraw = new Dictionary<GameObject, GizmoInfo>();


            HandleGizmoValidation(m_gizmosToDraw, m_onState, new Color(0, 0.5595117f, 1f));
            HandleGizmoValidation(m_gizmosToDraw, m_offState, new Color(1, 0.7397324f, 0));

            foreach (var key in m_gizmosToDraw.Keys)
            {
                var info = m_gizmosToDraw[key];
                Gizmos.color = info.color;
                Gizmos.DrawLine(transform.position, info.position);
            }
        }

        private void HandleGizmoValidation(Dictionary<GameObject, GizmoInfo> list, UnityEvent unityEvent, Color gizmoColor)
        {
            GameObject cacheGO = null;
            for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
            {
                if (unityEvent.GetPersistentTarget(i) != null)
                {
                    cacheGO = GetGameObject(unityEvent.GetPersistentTarget(i));
                    if (list.ContainsKey(cacheGO))
                    {
                        var info = list[cacheGO];
                        info.color += gizmoColor;
                        list[cacheGO] = info;
                    }
                    else
                    {
                        list.Add(cacheGO, new GizmoInfo(RetrievePosition(cacheGO), gizmoColor));
                    }
                }
            }
        }

        private GameObject GetGameObject(Object instance)
        {
            if (instance is GameObject)
            {
                return ((GameObject)instance);
            }
            else
            {
                return ((Component)instance).gameObject;
            }
        }

        private Vector3 RetrievePosition(Object instance)
        {
            if (instance is GameObject)
            {
                return ((GameObject)instance).transform.position;
            }
            else
            {
                return ((Component)instance).transform.position;
            }
        }
#endif
    }
}