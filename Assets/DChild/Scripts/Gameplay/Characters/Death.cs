using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using Doozy.Engine;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Death : MonoBehaviour, IEventModule
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private GameObject m_hitCollider;
    [SerializeField]
    private GameObject m_hitBox;
    [SerializeField]
    private SpineRootAnimation m_spineRoot;
    [SerializeField]
    private GameObject m_movementController;
    [SerializeField]
    private CharacterPhysics2D m_physics;

    //private IFacing m_characterFacing;

    public void ConnectEvents()
    {
        m_player.OnDeath += DeathTrigger;
        Debug.Log("check events");
    }

    private void DeathTrigger(object sender, EventActionArgs eventArgs)
    {
        //Debug.Log("DEAD");
        //m_physics.SetVelocity(0, 0);
        //m_characterFacing = m_player;
        //m_movementController.SetActive(false);
        //m_hitBox.SetActive(false);
        //m_hitCollider.SetActive(false);
        //if (m_characterFacing.currentFacingDirection == HorizontalDirection.Right)
        //{
        //    m_spineRoot.SetAnimation(0, "Death_Instant_Right", false, 0);
        //}
        //if (m_characterFacing.currentFacingDirection == HorizontalDirection.Left)
        //{
        //    m_spineRoot.SetAnimation(0, "Death_Instant_Left", false, 0);
        //}

    }

}
