using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ElectricArc : Obstacle
    {
        [SerializeField, LockAttackType(AttackType.Lightning)]
        private AttackDamage m_damage;
        [SerializeField]
        private Collider2D m_damageCollider;
        private IntervalTimer m_timer;

        protected override AttackDamage damage => m_damage;

        private void OnDeactivate(object sender, EventActionArgs eventArgs)
        {
            m_damageCollider.enabled = false;
        }

        private void OnActivate(object sender, EventActionArgs eventArgs)
        {
            m_damageCollider.enabled = true;
        }

        private void Awake()
        {
            m_timer = GetComponent<IntervalTimer>();
            m_timer.Activate += OnActivate;
            m_timer.Deactivate += OnDeactivate;
        }
    }

}