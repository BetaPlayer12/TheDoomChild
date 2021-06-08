using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DestroyObjectsEventHandle : MonoBehaviour
    {
        private enum TrackingType
        {
            AllDamageableDestroyed,
            OneOfDamageableDestroyed,
        }

        [SerializeField]
        private TrackingType m_trackingType;
        [SerializeField]
        private Damageable[] m_toTrack;
        [SerializeField]
        private UnityEvent m_onAllObjectsDestroyed;

        private int m_destroyedObject;
        private bool m_eventCalled;

        public void RestoreObjects()
        {
            for (int i = 0; i < m_toTrack.Length; i++)
            {
                m_toTrack[i].health.ResetValueToMax();
            }
            m_eventCalled = false;
        }

        private void OnObjectDestroyed(object sender, EventActionArgs eventArgs)
        {
            switch (m_trackingType)
            {
                case TrackingType.AllDamageableDestroyed:
                    m_destroyedObject++;
                    if (m_destroyedObject == m_toTrack.Length)
                    {
                        m_onAllObjectsDestroyed?.Invoke();
                    }
                    break;
                case TrackingType.OneOfDamageableDestroyed:
                    if (m_eventCalled == false)
                    {
                        m_onAllObjectsDestroyed?.Invoke();
                        m_eventCalled = true;
                    }
                    break;
            }
        }
        private void Start()
        {
            for (int i = 0; i < m_toTrack.Length; i++)
            {
                m_toTrack[i].Destroyed += OnObjectDestroyed;
            }
        }
    }
}