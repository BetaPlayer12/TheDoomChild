﻿
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using Spine.Unity.Examples;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Dash : MonoBehaviour, IResettableBehaviour, ICancellableBehaviour, IComplexCharacterModule, IDash
    {
        [SerializeField, HideLabel]
        private DashStatsInfo m_configuration;

        private float m_cooldownTimer;
        private float m_durationTimer;

        private IPlayerModifer m_modifier;
        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private IDashState m_state;
        private Animator m_animator;
        private int m_animationParameter;
        private SkeletonGhost m_skeletonGhost;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_modifier = info.modifier;
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_state.canDash = true;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsDashing);
            m_skeletonGhost = info.skeletonGhost;
        }

        public void SetConfiguration(DashStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void Cancel()
        {
            m_rigidbody.velocity = Vector2.zero;
            m_state.isDashing = false;
            m_animator.SetBool(m_animationParameter, false);
            //m_skeletonGhost.enabled = false;
        }

        public void HandleCooldown()
        {
            if (m_cooldownTimer <= 0)
            {
                Reset();
            }
            else
            {
                m_cooldownTimer -= GameplaySystem.time.deltaTime;
            }
        }

        public void ResetCooldownTimer() => m_cooldownTimer = m_configuration.cooldown * m_modifier.Get(PlayerModifier.Cooldown_Dash);

        public void HandleDurationTimer() => m_durationTimer -= GameplaySystem.time.deltaTime;

        public bool IsDashDurationOver() => m_durationTimer <= 0;

        public void ResetDurationTimer() => m_durationTimer = m_configuration.duration;

        public void Execute()
        {
            if (m_state.isDashing == false)
            {
                m_state.canDash = false;
                m_animator.SetBool(m_animationParameter, true);
                m_state.isDashing = true;
            }
            var direction = (float)m_character.facing;
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(new Vector2(direction * m_configuration.velocity * m_modifier.Get(PlayerModifier.Dash_Distance), 0), ForceMode2D.Impulse);
            //m_skeletonGhost.enabled = true;
        }

        public void Reset()
        {
            m_state.canDash = true;
        }
    }
}
