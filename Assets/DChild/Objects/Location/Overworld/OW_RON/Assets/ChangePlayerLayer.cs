using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangePlayerLayer : MonoBehaviour
{
    [SerializeField]
    private int m_zeeSortingLayerBefore;
    [SerializeField]
    private int m_zeeSortingLayerAfter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            var zeeSorting = collision.gameObject.GetComponent<SpriteRenderer>();
            zeeSorting.sortingOrder = m_zeeSortingLayerAfter;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var zeeSorting = collision.gameObject.GetComponent<SpriteRenderer>();
        zeeSorting.sortingOrder = m_zeeSortingLayerBefore;
    }
}
