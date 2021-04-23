using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using PlayerNew;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ObjectInteraction : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private InteractableDetector m_interactableDetector;

        private Character m_character;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
        }

        public void Interact()
        {
            if (m_interactableDetector != null)
            {
                if (m_interactableDetector.closestObject != null && m_interactableDetector.canBeInteracted)
                {
                    m_interactableDetector.closestObject.Interact(m_character);
                    GameEventMessage.SendEvent("Object Interacted");
                }
            }
        }
    }
}