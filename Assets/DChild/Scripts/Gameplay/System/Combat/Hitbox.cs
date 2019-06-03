﻿/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class Hitbox : MonoBehaviour
    {
        private ITarget m_damageable;
        [SerializeField, HideInInspector]
        private Collider2D[] m_collider2Ds;
        [SerializeField, HideInInspector]
        private int m_compositeColliderID;

        [SerializeField]
        private bool m_isInvulnerable;
        [SerializeField, HideIf("m_isInvulnerable"), Range(0, 0.99f)]
        private float m_damageReduction;

        public ITarget damageable => m_damageable;
        public BodyDefense defense => m_isInvulnerable ? new BodyDefense(m_isInvulnerable) : new BodyDefense(m_damageReduction);
        public int compositeColliderID => m_compositeColliderID;

        public void Enable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = true;
            }
        }

        public void Disable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = false;
            }
        }

        public void SetInvulnerability(bool value)
        {
            m_isInvulnerable = value;
        }

        private void Awake()
        {
            m_damageable = GetComponentInParent<ITarget>();
        }

        private void OnValidate()
        {
            var compositeCollider = GetComponent<CompositeCollider2D>();
            if (compositeCollider)
            {
                var colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
                colliders.Remove(compositeCollider);
                m_collider2Ds = colliders.ToArray();
                m_compositeColliderID = compositeCollider.GetInstanceID();
            }
            else
            {
                m_collider2Ds = new Collider2D[] { GetComponent<Collider2D>() };
                m_compositeColliderID = -1;
            }
        }
    }
}
