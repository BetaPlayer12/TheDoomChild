using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using System;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class InteractablePrompt : MonoBehaviour
    {
        [SerializeField]
        private InteractableDetector m_detector;
        [SerializeField]
        private RectTransform m_prompt;
        private void OnInteractableDetected(object sender, DetectedInteractableEventArgs eventArgs)
        {
            GameEventMessage.SendEvent("Interaction Prompt Hide");
            if (eventArgs.interactable)
            {
                m_prompt.transform.position = eventArgs.interactable.transform.position;
                GameEventMessage.SendEvent("Interaction Prompt Show");
            }
        }

        private void Awake()
        {
            if (m_detector)
            {
                m_detector.InteractableDetected += OnInteractableDetected;
            }
        }
    }
}