using DChild.Gameplay.Characters.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitLightningStrike : MonoBehaviour
{
    [SerializeField]
    private DemonLordAI m_demonLord;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.layer == 8)
        {
            m_demonLord.m_isPlayerHit = true;
            Debug.Log("MEEMMEMEMEMEME");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    private void Awake()
    {


       

    }
}
