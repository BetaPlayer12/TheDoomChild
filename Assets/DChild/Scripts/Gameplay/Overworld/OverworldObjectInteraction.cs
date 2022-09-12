using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldObjectInteraction : MonoBehaviour
{
    [SerializeField]
    private InteractableDetector m_interactableDetector;

   
    public void Interact()
    {
        if (m_interactableDetector != null)
        {
            if (m_interactableDetector.closestObject != null && m_interactableDetector.canBeInteracted)
            {
                m_interactableDetector.closestObject.Interact(GameplaySystem.playerManager.player.character);
                GameEventMessage.SendEvent("Object Interacted");
            }
        }
    }
}
