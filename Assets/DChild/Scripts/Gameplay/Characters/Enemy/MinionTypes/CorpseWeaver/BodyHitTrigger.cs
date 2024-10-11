using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitTrigger : MonoBehaviour
{

    public EventAction<EventActionArgs> OnhitEvent;
    public Damageable playerDamageable;

    public void DamageTaken()
    {
        OnhitEvent.Invoke(this, EventActionArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag != "Hitbox")
        //    return;

        var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
        if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
        {
            playerDamageable = collision.GetComponentInParent<Damageable>();
            DamageTaken();
            Debug.Log("hit?");

        }
        Debug.Log("hit??");
    }

}
