using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;

public class CobWebTrigger : MonoBehaviour
{
    
    public EventAction<EventActionArgs> CobWebEnterEvent;
    public EventAction<EventActionArgs> Onhit;
    public PlayerDamageable playerDamageable;
    
 
    public void CobwebEvent()
    {
        CobWebEnterEvent.Invoke(this, EventActionArgs.Empty);

    }
    public void DamageTaken()
    {
        Onhit.Invoke(this, EventActionArgs.Empty);
    }
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag != "Hitbox")
        //    return;

        var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
        if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
        {
            playerDamageable = collision.GetComponentInParent<PlayerDamageable>();
            DamageTaken();
            Debug.Log("hit??");
          
        }

    }
}
