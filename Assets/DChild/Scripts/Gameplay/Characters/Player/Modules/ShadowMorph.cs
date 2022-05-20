﻿using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowMorph : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule, IResettableBehaviour
    {
        [SerializeField, MinValue(0)]
        private int m_sourceRequiredAmount;
        [SerializeField, MinValue(0)]
        private int m_sourceConsumptionRate;
        [SerializeField]
        private ParticleSystem m_shadowMorphFX;
        //HACK
        [SerializeField, SpineSkin(dataField = "m_skeletonData")]
        private string m_originalSkinName;
        [SerializeField, SpineSkin(dataField = "m_skeletonData")]
        private string m_shadowMorphSkinName;
        [SerializeField]
        private SkeletonAnimation m_skeletonData;
        [SerializeField]
        private GameObject m_playerShadow;

        private Damageable m_damageable;
        private ICappedStat m_source;
        private IShadowModeState m_state;
        private Animator m_animator;
        private int m_animationParameter;
        private float m_stackedConsumptionRate;
        private IPlayerModifer m_modifier;
        private SkeletonGhost m_skeletonGhost;
        private string SHADOW_MORPH_ANIMATION_STATE = "Shadow Morph Start";

        public event EventAction<EventActionArgs> ExecuteModule;
        public event EventAction<EventActionArgs> End;

        public bool IsInShadowMode() => m_state.isInShadowMode;

        public bool HaveEnoughSourceForExecution() => m_sourceRequiredAmount <= m_source.currentValue;

        public void ConsumeSource()
        {
            m_stackedConsumptionRate += (m_sourceConsumptionRate * GameplaySystem.time.deltaTime) * m_modifier.Get(PlayerModifier.ShadowMagic_Requirement);

            if (m_stackedConsumptionRate >= 1)
            {
                var integer = Mathf.FloorToInt(m_stackedConsumptionRate);
                m_stackedConsumptionRate -= integer;
                m_source.ReduceCurrentValue(integer);
            }
        }

        public void EndExecution()
        {
            m_skeletonData.Skeleton.SetSkin(m_shadowMorphSkinName);
            m_state.isInShadowMode = true;
            m_state.waitForBehaviour = false;
            GameplaySystem.world.SetShadowColliders(true);
            //End?.Invoke(this, EventActionArgs.Empty);
        }

        public void Execute()
        {
            Debug.Log("Shadow Morph");
            m_animator.Play(SHADOW_MORPH_ANIMATION_STATE);
            m_shadowMorphFX.Play();
            m_damageable.SetInvulnerability(Invulnerability.Level_2);
            //m_animator.SetBool(m_animationParameter, true);
            m_skeletonGhost.enabled = true;
            m_playerShadow.SetActive(false);
            ExecuteModule?.Invoke(this, EventActionArgs.Empty);
            m_state.waitForBehaviour = true;
        }

        public void Cancel()
        {
            GameplaySystem.world.SetShadowColliders(false);
            m_skeletonData.Skeleton.SetSkin(m_originalSkinName);
            m_damageable.SetInvulnerability(Invulnerability.None);
            m_state.isInShadowMode = false;
            m_animator.SetBool(m_animationParameter, false);
            m_stackedConsumptionRate = 0;
            m_shadowMorphFX.Stop(true);
            m_skeletonGhost.enabled = false;
            m_playerShadow.SetActive(true);
            End?.Invoke(this, EventActionArgs.Empty);
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_damageable = info.damageable;
            m_source = info.magic;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode);
            m_stackedConsumptionRate = 0;
            m_modifier = info.modifier;
            m_skeletonGhost = info.skeletonGhost;
        }

        public void Reset()
        {
            m_state.isInShadowMode = false;
            m_playerShadow.SetActive(true);
        }
    }
}
