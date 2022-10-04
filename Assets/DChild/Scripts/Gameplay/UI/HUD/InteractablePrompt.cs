using DChild.Gameplay.Characters.Players;
using DChild.Temp;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
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
        private UIContainerUIAnimator m_animator;
        private Vector3 m_showStartPosition;

        private Vector3 m_newPromptPosition;

        private void OnInteractableDetected(object sender, DetectedInteractableEventArgs eventArgs)
        {
            GameplaySystem.gamplayUIHandle.ShowInteractionPrompt(false);
            if (eventArgs.interactable?.showPrompt ?? false)
            {
                m_newPromptPosition = eventArgs.interactable.promptPosition;
                GameEventMessage.Send();

                var move = m_animator.showAnimation.Move;
                move.fromCustomValue = m_newPromptPosition + m_showStartPosition;
                move.toCustomValue = m_newPromptPosition;

                m_validPrompt.enabled = eventArgs.showInteractionButton;
                m_invalidPrompt.enabled = !eventArgs.showInteractionButton;
                m_promptMessage.text = eventArgs.message;
                m_requirementMessage.text = eventArgs.message;
                GameplaySystem.gamplayUIHandle.ShowInteractionPrompt(true); 
            }
        }

        public void MoveToNewPromptPosition()
        {
            m_prompt.transform.position = m_newPromptPosition;
        }

        private void Awake()
        {
            if (m_detector)
            {
                m_detector.InteractableDetected += OnInteractableDetected;
            }
            m_animator = m_prompt?.GetComponent<UIContainerUIAnimator>();
            GameEventMessage.Send();
            m_showStartPosition = m_animator.showAnimation.Move.fromCustomValue;
        }
    }
}