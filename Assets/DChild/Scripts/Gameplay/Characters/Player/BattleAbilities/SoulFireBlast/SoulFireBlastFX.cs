using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Holysoft.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFireBlastFX : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> m_effects;
    [SerializeField]
    private SimpleAttackProjectile m_projectile;

    private void Awake()
    {
        m_projectile.PoolRequest += PoolRequest;
    }

    private void PoolRequest(object sender, PoolItemEventArgs eventArgs)
    {
        for (int i = 0; i < m_effects.Count; i++)
        {
            if (m_effects[i] != null)
            {
                m_effects[i].transform.SetParent(null);
                var fx = m_effects[i].main;
                fx.loop = false;
            }
        }
    }
}
