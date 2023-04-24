using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Holysoft.Pooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleProjectile : MonoBehaviour
{
    [SerializeField]
    private int maxBounces;
    [SerializeField]
    private float m_cooldown;
    [SerializeField]
    private Collider2D m_bodyCollider2D;
    [SerializeField]
    private Collider2D m_hurtbox;

    [SerializeField]
    private ParticleFX m_fx;

    private int numCollisions = 0;

    private SimpleAttackProjectile m_projectile;

    //private void Start()
    //{
    //    StopAllCoroutines();
    //    m_projectile.PoolRequest += PoolRequest;
    //    m_projectile.Impacted += Impacted;
    //    m_bodyCollider2D.enabled = true;
    //    StopAllCoroutines();
    //    StartCoroutine(CooldownRoutine());
    //}

    private void OnEnable()
    {
        StopAllCoroutines();
        m_bodyCollider2D.enabled = true;
        numCollisions = 0;
        StartCoroutine(CooldownRoutine());
    }

    private void Awake()
    {
        m_projectile = GetComponent<SimpleAttackProjectile>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        numCollisions++;
        m_fx.Play();

        if (numCollisions >= maxBounces)
        {
            m_bodyCollider2D.enabled = false;
            //m_hurtbox.enabled = true;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_projectile.ForceCollision();
        yield return null;
    }

}
