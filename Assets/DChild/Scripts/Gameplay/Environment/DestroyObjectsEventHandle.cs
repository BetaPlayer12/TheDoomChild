using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DestroyObjectsEventHandle : MonoBehaviour
    {
        [SerializeField]
        private Damageable[] m_toTrack;
        [SerializeField]
        private UnityEvent m_onAllObjectsDestroyed;

        private int m_destroyedObject;

        public void RestoreObjects()
        {
            for (int i = 0; i < m_toTrack.Length; i++)
            {
                m_toTrack[i].health.ResetValueToMax();
            }
        }

        private void OnObjectDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_destroyedObject++;
            if (m_destroyedObject == m_toTrack.Length)
            {
                m_onAllObjectsDestroyed?.Invoke();
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