using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Doozy.Engine;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class InteractablePrompt : MonoBehaviour
    {
        [SerializeField]
        private InteractableDetector m_detector;
        [SerializeField]
        private RectTransform m_prompt;
        [SerializeField, TabGroup("Valid Prompt")]
        private Canvas m_validPrompt;
        [SerializeField, TabGroup("Valid Prompt")]
        private TextMeshProUGUI m_promptMessage;
        [SerializeField, TabGroup("Invalid Prompt")]
        private Canvas m_invalidPrompt;
        [SerializeField, TabGroup("Invalid Prompt")]
        private TextMeshProUGUI m_requirementMessage;
        private UIView m_view;
        private Vector3 m_showStartPosition;
        
        private void OnInteractableDetected(object sender, DetectedInteractableEventArgs eventArgs)
        {
            GameplaySystem.gamplayUIHandle.ShowInteractionPrompt(false);
            if (eventArgs.interactable?.showPrompt ?? false)
            {
                var position = eventArgs.interactable.promptPosition;
                m_prompt.transform.position = position;
                var move = m_view.ShowBehavior.Animation.Move;
                move.From = position + m_showStartPosition;
                move.To = position;
                m_validPrompt.enabled = eventArgs.showInteractionButton;
                m_invalidPrompt.enabled = !eventArgs.showInteractionButton;
                m_promptMessage.text = eventArgs.message;
                m_requirementMessage.text = eventArgs.message;
                GameplaySystem.gamplayUIHandle.ShowInteractionPrompt(true); 
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