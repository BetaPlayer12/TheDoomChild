using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDeathHitDetector : MonoBehaviour
{
    public event EventAction<EventActionArgs> PlayerHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
        if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
        {
            if (collision.tag == "Hitbox")
            {
                PlayerHit?.Invoke(this, new EventActionArgs());
            }
        }
    }
}
