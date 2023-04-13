﻿using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity.Examples;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowDash : MonoBehaviour, IDash, IComplexCharacterModule
    {
        [SerializeField]
        private Dash m_dash;
        [SerializeField, HideLabel]
        private ShadowDashStatsInfo m_configuration;
        [SerializeField]
        private ParticleSystem m_shadowFX;
        [SerializeField]
        private Hitbox m_hitbox;

        private ICappedStat m_source;
        private IPlayerModifer m_modifier;
        private Damageable m_damageable;
        private Animator m_animator;
        private bool m_wasUsed;
        private int m_animationParameter;
        private SkeletonGhost m_skeletonGhost;

        //[ShowInInspector, ReadOnly, HideInEditorMode]
        //protected int sourceRequiredAmount => Mathf.FloorToInt(m_baseSourceRequiredAmount * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement));

        public event EventAction<EventActionArgs> ExecuteModule;
        public event EventAction<EventActionArgs> End;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.magic;
            m_modifier = info.modifier;
            m_damageable = info.damageable;
            m_animator = info.animator;
            m_skeletonGhost = info.skeletonGhost;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode);
        }

        public void SetConfiguration(ShadowDashStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void Cancel()
        {
            m_dash.Cancel();
            GameplaySystem.world.SetShadowColliders(false);
            m_damageable.SetInvulnerability(Invulnerability.None);
            m_hitbox.Enable();
            m_wasUsed = false;

            if (m_shadowFX != null)
            {
                m_shadowFX?.Stop(true);
            }

            m_animator.SetBool(m_animationParameter, false);
            //m_skeletonGhost.enabled = false;

            End?.Invoke(this, EventActionArgs.Empty);
        }

        public bool HaveEnoughSourceForExecution() => GetSourceRequiredAmount() <= m_source.currentValue;

        public void ConsumeSource() => m_source.ReduceCurrentValue(GetSourceRequiredAmount());

        public void HandleCooldown() => m_dash.HandleCooldown();

        public void ResetCooldownTimer() => m_dash.ResetCooldownTimer();

        public void HandleDurationTimer() => m_dash.HandleDurationTimer();

        public bool IsDashDurationOver() => m_dash.IsDashDurationOver();

        public void ResetDurationTimer() => m_dash.ResetDurationTimer();

        public int GetSourceRequiredAmount()
        {
            return Mathf.FloorToInt(m_configuration.baseSourceRequiredAmount * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement));
        }

        public void Execute()
        {
            if (m_wasUsed == false)
            {
                GameplaySystem.world.SetShadowColliders(true);
                m_damageable.SetInvulnerability(Invulnerability.Level_2);
                m_hitbox.Disable();
                m_wasUsed = true;

                if (m_shadowFX != null)
                {
                    m_shadowFX?.Play(true);
                }

                m_animator.SetBool(m_animationParameter, true);
                //m_skeletonGhost.enabled = true;
                ExecuteModule?.Invoke(this, EventActionArgs.Empty);
            }
            m_dash.Execute();
        }

        public void Reset()
        {
            m_dash.Reset();
        }
    }
}
