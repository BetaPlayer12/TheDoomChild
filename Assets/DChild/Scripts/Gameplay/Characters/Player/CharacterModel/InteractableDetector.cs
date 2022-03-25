using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class DetectedInteractableEventArgs : IEventActionArgs
    {
        private IButtonToInteract m_interactable;
        private string m_message;
        public bool showInteractionButton;

        public DetectedInteractableEventArgs()
        {
            m_interactable = null;
            m_message = "Interact";
        }

        public IButtonToInteract interactable => m_interactable;

        public string message { get => m_message; set => m_message = value == null ? "Interact" : value; }

        public void Initialize(IButtonToInteract interactable) => m_interactable = interactable;
    }

    public class InteractableDetector : MonoBehaviour, IComplexCharacterModule
    {
        private Character m_character;
        private Vector2 m_prevCharacterPosition;

        [ShowInInspector]
        private IButtonToInteract m_closestObject;
        [ShowInInspector,HideInEditorMode]
        private List<IButtonToInteract> m_objectsInRange;
        public event EventAction<DetectedInteractableEventArgs> InteractableDetected;

        public IButtonToInteract closestObject => m_closestObject;
        public bool canBeInteracted { get; private set; }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_prevCharacterPosition = m_character.centerMass.transform.position;
        }
        public void remoteInitialization(Character character, Vector2 Charposition)
        {
            m_character = character;
            m_prevCharacterPosition = Charposition;
        }

        public void ClearAllInteractableReferences()
        {
            m_objectsInRange.Clear();
            m_closestObject = null;
            CallInteractableDetectedEvent(m_closestObject);
        }

        private void CallInteractableDetectedEvent(IButtonToInteract interactable)
        {
            if (interactable == null || interactable.showPrompt)
            {
                using (Cache<DetectedInteractableEventArgs> cacheEvent = Cache<DetectedInteractableEventArgs>.Claim())
                {
                    cacheEvent.Value.Initialize(interactable);
                    if (interactable != null)
                    {
                        string message = null;
                        if (interactable.GetType().IsCastableTo(typeof(IInteractionRequirement)))
                        {
                            var requirement = (IInteractionRequirement)interactable;
                            canBeInteracted = requirement.CanBeInteracted(m_character);
                            if (canBeInteracted)
                            {
                                cacheEvent.Value.showInteractionButton = true;
                                message = interactable.promptMessage;
                            }
                            else
                            {
                                cacheEvent.Value.showInteractionButton = false;
                                message = requirement.requirementMessage;
                            }
                        }
                        else
                        {
                            canBeInteracted = true;
                            cacheEvent.Value.showInteractionButton = true;
                            message = interactable.promptMessage;
                        }

                        cacheEvent.Value.message = message;
                    }
                    InteractableDetected?.Invoke(this, cacheEvent.Value);
                    cacheEvent.Release();
                }
            }
        }

        private void Awake()
        {
            m_objectsInRange = new List<IButtonToInteract>();
        }

        public void Update()
        {
            for (int i = m_objectsInRange.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (m_objectsInRange[i].transform.gameObject.activeInHierarchy == false)
                    {
                        RemoveIndexSafely(i);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is MissingReferenceException || ex is NullReferenceException)
                    {
                        RemoveIndexSafely(i);
                    }
                }
            }

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
            else if (m_objectsInRange.Count == 1)
            {
                if (m_closestObject != m_objectsInRange[0])
                {
                    m_closestObject = m_objectsInRange[0];
                }
            }
        }

        private void RemoveIndexSafely(int i)
        {
            m_objectsInRange.RemoveAt(i);

            if (m_objectsInRange.Count == 0)
            {
                CallInteractableDetectedEvent(null);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger)
            {
                if (collision.TryGetComponentInParent(out IButtonToInteract interactableObject))
                {
                    if (interactableObject.showPrompt)
                    {
                        m_objectsInRange.Add(interactableObject);
                        if (m_objectsInRange.Count == 1)
                        {
                            m_closestObject = interactableObject;
                            CallInteractableDetectedEvent(interactableObject);
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out IButtonToInteract interactableObject))
            {
                if (interactableObject.showPrompt)
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
}