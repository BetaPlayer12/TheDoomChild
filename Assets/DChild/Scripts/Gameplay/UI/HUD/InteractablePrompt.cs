using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using Doozy.Engine.UI;
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
        private UIView m_view;
        private Vector3 m_showStartPosition;
        private void OnInteractableDetected(object sender, DetectedInteractableEventArgs eventArgs)
        {
            GameEventMessage.SendEvent("Interaction Prompt Hide");
            if (eventArgs.interactable?.showPrompt ?? false)
            {
                var position = eventArgs.interactable.promptPosition;
                m_prompt.transform.position = position;
                var move = m_view.ShowBehavior.Animation.Move;
                move.From = position + m_showStartPosition;
                move.To = position;
                GameEventMessage.SendEvent("Interaction Prompt Show");
            }
        }

        private void Awake()
        {
            if (m_detector)
            {
                m_detector.InteractableDetected += OnInteractableDetected;
            }
            m_view = m_prompt?.GetComponent<UIView>();
            m_showStartPosition = m_view.ShowBehavior.Animation.Move.From;
        }
    }
}