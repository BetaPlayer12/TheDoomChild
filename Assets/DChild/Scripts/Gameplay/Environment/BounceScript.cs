using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using DChild.Gameplay;

public class BounceScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Zee"))
        {
            Rigidbody2D m_playerphysics = collision.GetComponentInParent<Rigidbody2D>();
            m_playerphysics.velocity = new Vector2(0, 10);
            m_playerphysics.AddForce(new Vector2(0, 50));
        }
        
    }

}
