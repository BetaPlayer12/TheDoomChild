using DChild.Gameplay.Environment.Interractables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/InteractableObject")]
    public class InteractableObject : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private string m_promptMessage = "Interact";
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private bool m_oneTimeInteraction;
        [SerializeField]
        private UnityEvent m_onInteraction;
        private bool m_canInteract;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public bool showPrompt => true;

        public string promptMessage => m_promptMessage;

        public void Interact(Character character)
        {
            if (m_canInteract)
            {
                m_onInteraction?.Invoke();
                if (m_oneTimeInteraction)
                {
                    m_canInteract = false;
                }
            }
        }

        [Button]
        public void Interact()
        {
            if (m_canInteract)
            {
                m_onInteraction?.Invoke();
                if (m_oneTimeInteraction)
                {
                    m_canInteract = false;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

        private void Awake()
        {
            m_canInteract = true;
        }
    }
}
