using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Systems
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent m_gameEvent;

        [SerializeField]
        private UnityEvent m_response;

        private void OnEnable()
        {
            m_gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            m_gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            m_response.Invoke();
        }
    }
}

