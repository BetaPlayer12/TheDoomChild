using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Death : MonoBehaviour, IPlayerExternalModule, IEventModule
{
    private Player m_player;
    [SerializeField]
    private GameObject m_hitCollider;
    [SerializeField]
    private GameObject m_hitBox;
    [SerializeField]
    private SpineRootAnimation m_spineRoot;

    public void ConnectEvents()
    {

        m_player.OnDeath += DeathTrigger;
    }

    private void DeathTrigger(object sender, EventActionArgs eventArgs)
    {
        m_hitBox.SetActive(false);
        m_hitCollider.SetActive(false);
        m_spineRoot.SetAnimation(0, "Death_Instant_Right", true, 0);

    }

    public void Initialize(IPlayerModules player)
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
