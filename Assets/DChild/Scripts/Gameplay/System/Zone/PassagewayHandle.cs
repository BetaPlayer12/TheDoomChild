using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Systems;
using System;
using UnityEngine;

[System.Serializable]
public class PassagewayHandle : ISwitchHandle
{
    [SerializeField]
    private TravelDirection m_entranceDirection;
    [SerializeField]
    private TravelDirection m_exitDirection;

    public float transitionDelay => 1;

    private void OnPassagewayEnter(Character character, TravelDirection travelDirection)
    {
        var controller = GameplaySystem.playerManager.OverrideCharacterControls();

        if (travelDirection == TravelDirection.Left || travelDirection == TravelDirection.Right)
        {
            controller.moveDirectionInput = GetDirectionInput(travelDirection);
        }
        if(travelDirection == TravelDirection.Top)
        {

        }
        if(travelDirection == TravelDirection.Bottom)
        {

        }
    }

    private void OnPassageWayPostEnter(Character character)
    {
        var controller = GameplaySystem.playerManager.OverrideCharacterControls();

        controller.moveDirectionInput = 0;
        character.physics.SetVelocity(Vector2.zero);
        character.physics.simulateGravity = false;
        var groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
        groundednessHandle.enabled = false;
    }

    private void OnPassagewayExit(Character character, TravelDirection exitDirection)
    {
        var controller = GameplaySystem.playerManager.OverrideCharacterControls();

        if (exitDirection == TravelDirection.Left)
        {
            controller.moveDirectionInput = GetDirectionInput(exitDirection);
        }
        if(exitDirection == TravelDirection.Right)
        {
            controller.moveDirectionInput = GetDirectionInput(exitDirection);
        }
        if (exitDirection == TravelDirection.Top)
        {

        }
        if (exitDirection == TravelDirection.Bottom)
        {

        }

        character.physics.simulateGravity = true;
        var groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
        groundednessHandle.enabled = true;
    }

    public int GetDirectionInput(TravelDirection direction)
    {
        switch (direction)
        {
            case TravelDirection.Left:
                return -1;
            case TravelDirection.Right:
                return 1;
            default:
                return 0;
        }
    }

    public void DoSceneTransition(Character character, TransitionType type)
    {
        if(type == TransitionType.Enter)
        {
            OnPassagewayEnter(character, m_entranceDirection);
        }
        else if(type == TransitionType.PostEnter)
        {
            OnPassageWayPostEnter(character);
        }
        else if(type == TransitionType.Exit)
        {
            OnPassagewayExit(character, m_exitDirection);
        }
    }
}
