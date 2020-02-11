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
        private bool m_oneTimeInteraction;
        [SerializeField]
        private UnityEvent m_onInteraction;
        private bool m_canInteract;

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
