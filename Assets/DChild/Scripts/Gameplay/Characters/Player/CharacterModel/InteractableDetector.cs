using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class DetectedInteractableEventArgs : IEventActionArgs
    {
        private IButtonToInteract m_interactable;

        public IButtonToInteract interactable => m_interactable;

        public void Initialize(IButtonToInteract interactable) => m_interactable = interactable;
    }

    public class InteractableDetector : MonoBehaviour, IComplexCharacterModule
    {
        private Character m_character;
        private Vector2 m_prevCharacterPosition;

        private List<IButtonToInteract> m_objectsInRange;
        private IButtonToInteract m_closestObject;
        public event EventAction<DetectedInteractableEventArgs> InteractableDetected;

        public IButtonToInteract closestObject => m_closestObject;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_prevCharacterPosition = m_character.centerMass.transform.position;
        }

        private void CallInteractableDetectedEvent(IButtonToInteract interactable)
        {
            if(interactable != null)
            {
                if (interactable.showPrompt)
                {
                    using (Cache<DetectedInteractableEventArgs> cacheEvent = Cache<DetectedInteractableEventArgs>.Claim())
                    {
                        cacheEvent.Value.Initialize(interactable);
                        InteractableDetected?.Invoke(this, cacheEvent.Value);
                        cacheEvent.Release();
                    }
                }
            }
        }

        private void Awake()
        {
            m_objectsInRange = new List<IButtonToInteract>();
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
            if (collision.TryGetComponentInParent(out IButtonToInteract interactableObject))
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
            if (collision.TryGetComponentInParent(out IButtonToInteract interactableObject))
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