using DChild.Gameplay.Environment.Interractables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/InteractableObject")]
    public class InteractableObject : MonoBehaviour, IButtonToInteract
    {

        [SerializeField, HideInInspector]
        private Vector3 m_promptPosition;
        [SerializeField]
        private bool m_oneTimeInteraction;
        [SerializeField]
        private UnityEvent m_onInteraction;
        private bool m_canInteract;

        public Vector3 promptPosition => m_promptPosition;

        public bool showPrompt => true;

#if UNITY_EDITOR
        [SerializeField,PropertyOrder(-1)]
        private Transform m_promptLocation;

        private void OnValidate()
        {
            m_promptPosition = m_promptLocation.position;
        }
#endif
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

        private void Awake()
        {
            m_canInteract = true;
        }
    }
}
