﻿using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Talimbaga")]
    public class TalimbagaAI : CombatAIBrain<TalimbagaAI.Info>, IResetableAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField, MinValue(0)]
            private float m_burrowDuration;
            public float burrowDuration => m_burrowDuration;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_burrowAnimation;
            public string burrowAnimation => m_burrowAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_unburrowAnimation;
            public string unburrowAnimation => m_unburrowAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Patrol,
            Standby,
            Turning,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Shadow m_shadow;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_shadowObject;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        private float m_currentTimeScale;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private State m_turnState;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.enabled = false;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }


        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                m_movement.Stop();

            //enabled = false;
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_flinchHandle.gameObject.SetActive(false);
            //m_animation.SetEmptyAnimation(0, 0);
            //m_animation.SetEmptyAnimation(1, 0);
            //m_animation.SetEmptyAnimation(2, 0);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_selfCollider.enabled = false;
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.Wait(State.Patrol);
            StartCoroutine(FlinchRoutine());
        }

        private IEnumerator FlinchRoutine()
        {
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_shadow.enabled = false;
            m_shadowObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.burrowAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.burrowAnimation);
            transform.position = m_startPoint;
            yield return new WaitForSeconds(m_info.burrowDuration);
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false).MixDuration = 0;
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_shadow.enabled = true;
            m_shadowObject.SetActive(true);
            m_hitbox.Enable();
            //m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_turnHandle.TurnDone += OnTurnDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.Patrol;
                        m_animation.SetAnimation(0, m_info.move.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.move.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }


        public void HandleKnockback(float resumeAIDelay)
        {
            StopAllCoroutines();
            m_stateHandle.Wait(State.Patrol);
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                m_movement.Stop();

            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.Patrol);
            yield return null;
        }
    }
}