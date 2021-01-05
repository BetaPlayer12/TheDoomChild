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
    [AddComponentMenu("DChild/Gameplay/Combat/Hitbox")]
    public class Hitbox : MonoBehaviour
    {
        private IDamageable m_damageable;
        [SerializeField, DisableInPlayMode, HideInEditorMode]
        private Collider2D[] m_collider2Ds;

        [SerializeField]
        private Invulnerability m_invulnerabilityLevel;
        [SerializeField, HideIf("@m_invulnerabilityLevel == Invulnerability.MAX"), Range(0, 0.99f)]
        private float m_damageReduction;

        public IDamageable damageable => m_damageable;
        public BodyDefense defense => new BodyDefense(m_invulnerabilityLevel, m_damageReduction);
        public Invulnerability invulnerabilityLevel => m_invulnerabilityLevel;

        public void Enable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = true;
            }
        }

        [Button]
        public void Disable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = false;
            }
        }

        public virtual bool CanBeDamageBy(params Collider2D[] colliders) => true;

        public void SetInvulnerability(Invulnerability value)
        {
            m_invulnerabilityLevel = value;
        }

        private void Awake()
        {
            m_damageable = GetComponentInParent<IDamageable>();
        }

        private void OnValidate()
        {
            var compositeCollider = GetComponent<CompositeCollider2D>();
            if (compositeCollider)
            {
                var colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
                colliders.Remove(compositeCollider);
                m_collider2Ds = colliders.ToArray();
            }
            else
            {
                m_collider2Ds = GetComponentsInChildren<Collider2D>();
            }
        }
    }
}
