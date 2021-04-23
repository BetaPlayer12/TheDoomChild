using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using UnityEngine;
using PlayerNew;
using DChild.Gameplay.Characters.Players.State;
using System;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerDeath : MonoBehaviour, IComplexCharacterModule
    {
        private Animator m_animator;
        private IDeathState m_state;
        public Damageable m_source;
        private int m_deathParameter;

        public event EventAction<EventActionArgs> OnExecute;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.damageable;
            m_source.Destroyed += OnDeath;
            m_state = info.state;
            m_animator = info.animator;
            m_deathParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsDead);
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            OnExecute?.Invoke(this, EventActionArgs.Empty);
            m_source.SetHitboxActive(false);
            m_state.isDead = true;
            m_animator.SetBool(m_deathParameter, true);
            m_source.Healed += OnHealed;
        }

        private void OnHealed(object sender, EventActionArgs eventArgs)
        {
            m_source.SetHitboxActive(true);
            m_state.isDead = false;
            m_animator.SetBool(m_deathParameter, false);
            m_source.Healed -= OnHealed;
        }
    }
}