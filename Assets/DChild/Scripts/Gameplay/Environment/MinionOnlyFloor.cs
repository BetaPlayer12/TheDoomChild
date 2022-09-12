using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionOnlyFloor : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_surface;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
        {
            Physics2D.IgnoreCollision(m_surface, collision, true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
        {
            Physics2D.IgnoreCollision(m_surface, collision.collider, true);
        }
    }
}
