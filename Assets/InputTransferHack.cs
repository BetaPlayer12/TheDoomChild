using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTransferHack : MonoBehaviour
{

    [SerializeField]
    private PlayerInput m_overworldInput;

    private void Start()
    {
        PlayerInput m_playerInput = GameplaySystem.playerManager.player.GetComponentInChildren<PlayerInput>(true);
        m_playerInput.enabled = false;
        
        StartCoroutine(SwitchToOverworldControlsRoutine());
    }

    private IEnumerator SwitchToOverworldControlsRoutine()
    {
        m_overworldInput.enabled = false;
        yield return new WaitForSeconds(0.5f);
        m_overworldInput.enabled = true;
    }

}
