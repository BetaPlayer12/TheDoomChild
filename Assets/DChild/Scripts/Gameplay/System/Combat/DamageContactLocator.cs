using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageContactLocator : MonoBehaviour
{
    private Vector2 m_damageContactPoint;
    public Vector2 damageContactPoint => m_damageContactPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ColliderDamage>() != null)
            m_damageContactPoint = collision.transform.position;
    }
}
