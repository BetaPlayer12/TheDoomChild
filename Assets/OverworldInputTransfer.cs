using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldInputTransfer: MonoBehaviour
{
    private PlayerInput m_overworldInput;

    private void OnDisable()
    {
        m_overworldInput = GetComponentInParent<PlayerInput>(true);
        Debug.Log($"overworld input: {m_overworldInput}");
        m_overworldInput.enabled = false;

        StartCoroutine(SwitchToUnderworldControlsRoutine());
    }

    private static IEnumerator SwitchToUnderworldControlsRoutine()
    {
        PlayerInput m_playerInput = GameplaySystem.playerManager.player.GetComponentInChildren<PlayerInput>(true);
        yield return new WaitForSeconds(0.5f);
        m_playerInput.enabled = true;
        Debug.Log("successfully switched over to under controls");


    }
}
