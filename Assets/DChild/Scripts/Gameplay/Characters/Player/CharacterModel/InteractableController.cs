
using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using PlayerNew;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : PlayerBehaviour
{
    [SerializeField]
    private InteractableDetector m_interactableDetector;

    private Character m_character;

    private void Start()
    {
        m_character = GetComponentInParent<Character>();
    }

    private void Update()
    {
        var interactButton = inputState.GetButtonValue(inputButtons[0]);

        if (interactButton)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (m_interactableDetector != null)
        {
            if (m_interactableDetector.closestObject != null)
            {
                m_interactableDetector.closestObject.Interact(m_character);
                GameEventMessage.SendEvent("Object Interacted");
            }
        }
    }
}
