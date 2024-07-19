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

//public class CobWebTrigger : MonoBehaviour
//{

//    [SerializeField]
//    private CorpseWeaverAI m_corpseWeaverAI;

//    public bool m_isPlayerCaught;
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if ( collision.gameObject.layer == 8)
//        {
//            m_isPlayerCaught = m_corpseWeaverAI.isPlayerDetected = true;
//            Debug.Log("Hit the player");
//        }
//        Debug.Log("Hit outside the if statement");
//    }






//    private void Start()
//    {
//    }
//}
public class CobWebTrigger : MonoBehaviour
{
    
    public EventAction<EventActionArgs> CobWebEnterEvent;
    [ReadOnly]
    private EventAction<EventActionArgs> m_cobWebExitEvent;

   
    public PlayerDamageable playerDamageable;
    public void CobwebEvent()
    {
        CobWebEnterEvent.Invoke(this, EventActionArgs.Empty);

    }


    //private void TestEvent(object sender, EventActionArgs eventAction)
    //{
    //    Debug.Log("Cobweb Activated");
    //}
    //private void TestEventExit(object sender, EventActionArgs eventAction)
    //{
    //    Debug.Log("Cobweb DeActivated");
    //}
    private void Start()
    {
        //m_cobWebEnterEvent += TestEvent;
        //m_cobWebExitEvent += TestEventExit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Hitbox")
            return;

        var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
        if (playerObject != null && collision.tag != "Sensor" && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
        {
            playerDamageable = collision.GetComponentInParent<PlayerDamageable>();
            CobWebEnterEvent.Invoke(this, EventActionArgs.Empty);

        }

    }
}
