using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldInputTransfer: MonoBehaviour
{
    private PlayerInput m_overworldInput;

    private void OnDestroy()
    {
        PlayerInput m_playerInput = GameplaySystem.playerManager.player.GetComponentInChildren<PlayerInput>(true);
        m_playerInput.enabled = true;
        Debug.Log("successfully switched over to underworld controls");
    }
}
