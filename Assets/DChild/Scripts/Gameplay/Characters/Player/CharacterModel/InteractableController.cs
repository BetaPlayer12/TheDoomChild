using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using PlayerNew;
using UnityEngine;

public class InteractableController : PlayerBehaviour
{
    [SerializeField]
    private InteractableDetector m_interactableDetector;

    private Character m_character;
    private bool m_isHeld;

    private void Start()
    {
        m_character = GetComponentInParent<Character>();
    }

    private void Update()
    {
        var interactButton = inputState.GetButtonValue(inputButtons[0]);

        if (interactButton)
        {
            if (m_isHeld == false)
            {
                Interact();
                m_isHeld = true;
            }
        }
        else
        {
            m_isHeld = false;
        }
    }

    private void Interact()
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
