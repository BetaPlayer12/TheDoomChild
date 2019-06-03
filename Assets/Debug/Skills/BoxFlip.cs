using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFlip : MonoBehaviour, IPlayerExternalModule
{
    [SerializeField]
    private Transform m_body;
    [SerializeField]
    private Transform m_spawnPoints;

    private IFacing m_facing;

    public void Initialize(IPlayerModules player)
    {
        m_facing = player;
    }

    private void Update()
    {
        var newBodyPos = m_body.rotation;
        newBodyPos.y = (m_facing.currentFacingDirection == HorizontalDirection.Left) ? 180 : 0;

        var newSpawnPos = m_spawnPoints.rotation;
        newSpawnPos.y = (m_facing.currentFacingDirection == HorizontalDirection.Left) ? 180 : 0;

        m_body.rotation = newBodyPos;
        m_spawnPoints.rotation = newSpawnPos;
    }
}
