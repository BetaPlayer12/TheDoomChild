using DChild.Gameplay.Environment;
using Holysoft.Event;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class DetectedInteractableEventArgs : IEventActionArgs
    {
        private InteractableObject m_interactable;

        public InteractableObject interactable => m_interactable;

        public void Initialize(InteractableObject interactable) => m_interactable = interactable;
    }

    public class InteractableDetector : MonoBehaviour, IComplexCharacterModule
    {
        private Character m_character;
        private Vector2 m_prevCharacterPosition;

        private List<InteractableObject> m_objectsInRange;
        private InteractableObject m_closestObject;
        public event EventAction<DetectedInteractableEventArgs> InteractableDetected;

        public InteractableObject closestObject => m_closestObject;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_prevCharacterPosition = m_character.centerMass.transform.position;
        }

        private void CallInteractableDetectedEvent(InteractableObject interactable)
        {
            using (Cache<DetectedInteractableEventArgs> cacheEvent = Cache<DetectedInteractableEventArgs>.Claim())
            {
                cacheEvent.Value.Initialize(interactable);
                InteractableDetected?.Invoke(this, cacheEvent.Value);
                cacheEvent.Release();
            }
        }

        private void Awake()
        {
            m_objectsInRange = new List<InteractableObject>();
        }

        public void Update()
        {
            if (m_objectsInRange.Count > 1)
            {
                var currentPosition = (Vector2)m_character.centerMass.position;
                if (m_prevCharacterPosition != currentPosition)
                {
                    float closestDistance = Vector2.Distance(currentPosition, m_objectsInRange[0].transform.position);
                    var closestObject = m_objectsInRange[0];
                    for (int i = 1; i < m_objectsInRange.Count; i++)
                    {
                        var distance = Vector2.Distance(currentPosition, m_objectsInRange[i].transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestObject = m_objectsInRange[i];
                        }
                    }

                    if (m_closestObject != closestObject)
                    {
                        CallInteractableDetectedEvent(closestObject);
                    }
                    m_closestObject = closestObject;
                    m_prevCharacterPosition = currentPosition;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out InteractableObject interactableObject))
            {
                m_objectsInRange.Add(interactableObject);
                if (m_objectsInRange.Count == 1)
                {
                    m_closestObject = interactableObject;
                    CallInteractableDetectedEvent(interactableObject);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out InteractableObject interactableObject))
            {
                m_objectsInRange.Remove(interactableObject);
                if (m_objectsInRange.Count == 0)
                {
                    m_closestObject = null;
                    CallInteractableDetectedEvent(null);
                }
            }
        }


    }
}