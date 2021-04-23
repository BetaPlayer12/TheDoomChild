using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Combat
{
    public class EntitiesKilledEvent : MonoBehaviour
    {
        [SerializeField]
        private int m_entitiesLeftCallEvent;
        [SerializeField, TabGroup("Entities")]
        private Damageable[] m_entities;
        [SerializeField, TabGroup("Event")]
        private UnityEvent m_event;

        private int m_currentCount;

        private void Awake()
        {
            for (int i = 0; i < m_entities.Length; i++)
            {
                m_entities[i].Destroyed += OnEntityDestroyed;
            }

            m_currentCount = m_entities.Length;
        }

        private void OnEntityDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_currentCount--;
            if (m_currentCount == m_entitiesLeftCallEvent)
            {
                m_event?.Invoke();
            }
        }
    }
}