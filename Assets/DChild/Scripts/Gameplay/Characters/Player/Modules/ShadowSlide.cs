using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity.Examples;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowSlide : MonoBehaviour, ISlide, IComplexCharacterModule
    {
        [SerializeField]
        private Slide m_slide;
        [SerializeField, HideLabel]
        private ShadowSlideStatsInfo m_configuration;
        [SerializeField]
        private ParticleSystem m_shadowFX;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField]
        private CharacterState m_state;

        private ICappedStat m_source;
        private IPlayerModifer m_modifier;
        private Damageable m_damageable;
        private Animator m_animator;
        private bool m_wasUsed;
        private int m_animationParameter;
        private SkeletonGhost m_skeletonGhost;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        protected int sourceRequiredAmount => Mathf.FloorToInt(m_configuration.baseSourceRequiredAmount * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement));

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

        public void SetConfiguration(ShadowSlideStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void Cancel()
        {
            m_slide.Cancel();
            GameplaySystem.world.SetShadowColliders(false);
            m_damageable.SetInvulnerability(Invulnerability.None);
            m_wasUsed = false;
            m_shadowFX?.Stop(true);
            m_animator.SetBool(m_animationParameter, false);
            m_skeletonGhost.enabled = false;
            m_state.waitForBehaviour = false;

            End?.Invoke(this, EventActionArgs.Empty);
        }

        public bool HaveEnoughSourceForExecution() => sourceRequiredAmount <= m_source.currentValue;

        public void ConsumeSource() => m_source.ReduceCurrentValue(sourceRequiredAmount);

        public void HandleCooldown() => m_slide.HandleCooldown();

        public void ResetCooldownTimer() => m_slide.ResetCooldownTimer();

        public void HandleDurationTimer() => m_slide.HandleDurationTimer();

        public bool IsSlideDurationOver() => m_slide.IsSlideDurationOver();

        public void ResetDurationTimer() => m_slide.ResetDurationTimer();

        public void Execute()
        {
            if (m_wasUsed == false)
            {
                GameplaySystem.world.SetShadowColliders(true);
                m_damageable.SetInvulnerability(Invulnerability.Level_2);
                m_wasUsed = true;
                m_shadowFX?.Play(true);
                m_animator.SetBool(m_animationParameter, true);
                //m_skeletonGhost.enabled = true;
                ExecuteModule?.Invoke(this, EventActionArgs.Empty);
            }

            m_slide.Execute();
        }

        public bool HasGroundToSlideOn()
        {
            m_groundSensor.Cast();
            return m_groundSensor.allRaysDetecting;
        }

        public void Reset()
        {
            m_state.waitForBehaviour = false;
            m_slide.Reset();
        }

        public void EndTransitionStart()
        {
            m_state.waitForBehaviour = true;
        }

        public void EndTransitionEnd()
        {
            m_state.waitForBehaviour = false;
        }
    }
}
