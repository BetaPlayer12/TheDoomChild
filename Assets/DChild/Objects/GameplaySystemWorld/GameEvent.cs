using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField]
        private List<GameEventListener> m_listeners = new List<GameEventListener>();

        public void Raise()
        {
            for(int i = m_listeners.Count; i >= 0 ; i--)
            {
                m_listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if(!m_listeners.Contains(listener))
            {
                m_listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (m_listeners.Contains(listener))
            {
                m_listeners.Remove(listener);
            }
        }
    }
}

