using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class PoleGuy : MonoBehaviour
    {
        private const string ANIMATION_IDLE = "Idle";
        private const string ANIMATION_FLINCH = "Flinch";
        private const string ANIMATION_DEATH = "Death";
        private const string ANIMATION_DEATHIDLE = "Death_idle";

        private SpineAnimation m_animation;

        private void Start()
        {
            var damageable = GetComponent<Damageable>();
            damageable.DamageTaken += OnDamageTaken;
            damageable.Destroyed += OnDestroyed;

            m_animation = GetComponentInChildren<SpineAnimation>();
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, ANIMATION_DEATH, false);
            m_animation.AddAnimation(0, ANIMATION_DEATHIDLE, true, 0);
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, ANIMATION_FLINCH, false);
            m_animation.AddAnimation(0, ANIMATION_IDLE, true, 0);
        }

        public void Reset()
        {
            m_animation.SetAnimation(0, ANIMATION_IDLE, true);
        }
    }
}