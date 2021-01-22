using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerAvatar : MonoBehaviour
    {
        [System.Serializable]
        private class ModeShiftInfo
        {
            [SerializeField]
            private AnimationReferenceAsset m_startAnimation;
            [SerializeField]
            private AnimationReferenceAsset m_loopAnimation;
            [SerializeField]
            private AnimationReferenceAsset m_endAnimation;

            public AnimationReferenceAsset startAnimation => m_startAnimation;
            public AnimationReferenceAsset loopAnimation => m_loopAnimation;
            public AnimationReferenceAsset endAnimation => m_endAnimation;
        }

        [SerializeField]
        private AnimationReferenceAsset m_normalIdle;
        [SerializeField]
        private AnimationReferenceAsset m_flinch;

        [SerializeField]
        private ModeShiftInfo m_shadowMode;

        private ModeShiftInfo m_currentMode;
        private SkeletonGraphic m_animation;
        private CharacterState m_state;

        private void OnPlayerDamaged(object sender, Damageable.DamageEventArgs eventArgs)
        {
            ChangeToAnimation(m_flinch, m_normalIdle);
        }

        private void OnShadowMorphEnd(object sender, EventActionArgs eventArgs)
        {
            ChangeToAnimation(m_currentMode.endAnimation, m_normalIdle);
        }

        private void OnShadowMorphExecuted(object sender, EventActionArgs eventArgs)
        {
            ChangeToAnimation(m_shadowMode.startAnimation, m_shadowMode.loopAnimation);
            m_currentMode = m_shadowMode;
        }

        private void ChangeToAnimation(AnimationReferenceAsset start, AnimationReferenceAsset loop)
        {
            m_animation.AnimationState.SetAnimation(0, start, false);
            m_animation.AnimationState.AddAnimation(0, loop, true, 0);
        }

        private void Start()
        {
            m_animation = GetComponent<SkeletonGraphic>();

            var player = GameplaySystem.playerManager.player;
            m_state = player.state;
            player.damageableModule.DamageTaken += OnPlayerDamaged;

            var shadowMorph = player.GetComponent<ShadowMorph>();
            shadowMorph.ExecuteShadowMorph += OnShadowMorphExecuted;
            shadowMorph.EndShadowMorphExecution += OnShadowMorphEnd;

            m_animation.AnimationState.SetAnimation(0, m_normalIdle, true);
        }
    }
}