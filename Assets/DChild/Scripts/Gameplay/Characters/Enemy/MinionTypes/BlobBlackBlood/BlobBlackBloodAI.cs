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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BlobBlackBlood")]
    public class BlobBlackBloodAI : CombatAIBrain<BlobBlackBloodAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField, MinValue(0)]
            private float m_deathDuration;
            public float deathDuration => m_deathDuration;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_recoverAnimation;
            public string recoverAnimation => m_recoverAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_hitboxStartEvent;
            public string hitboxStartEvent => m_hitboxStartEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrol,
            Turning,
            Sleep,
            WakeUp,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        //[SerializeField, TabGroup("Reference")]
        //private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
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

        private float m_currentMoveSpeed;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        [ShowInInspector]
        private float m_sleepTimer;

        private State m_turnState;

        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomIdleRoutine;
        private Coroutine m_randomTurnRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            //m_animation.SetEmptyAnimation(0, 0);
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            m_movement.Stop();
            m_selfCollider.enabled = false;
            m_stateHandle.OverrideState(State.Sleep);
            StartCoroutine(SleepRoutine());  
        }

        private IEnumerator SleepRoutine()
        {
            enabled = true;
            m_hitbox.Disable();
            m_selfCollider.enabled = false;
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //m_animation.SetAnimation(0, m_info.disassembledIdleAnimation, true);
            yield return new WaitForSeconds(m_info.deathDuration);
            //m_animation.SetAnimation(0, m_info.recoverAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.recoverAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.OverrideState(State.Patrol);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            //gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator WakeUpRoutine()
        {
            m_health.SetHealthPercentage(1f);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            m_stateHandle.SetState(State.Patrol);
            yield return null;
        }

        //private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        //{
        //    if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
        //    {
        //        StopAllCoroutines();
        //        m_selfCollider.enabled = true;
        //        //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
        //        m_stateHandle.Wait(State.ReevaluateSituation);
        //        //StartCoroutine(FlinchRoutine());
        //    }
        //}

        public override void ApplyData()
        {
            base.ApplyData();
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);

            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_turnHandle.TurnDone += OnTurnDone;
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        //m_animation.EnableRootMotion(true, false);
                        //m_animation.SetAnimation(0, m_info.move.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.move.speed, characterInfo);
                    }
                    //else
                    //{
                    //    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    //    {
                    //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //    }
                    //}
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    //m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    m_stateHandle.SetState(State.Patrol);

                    if (m_patienceRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }

                    if (m_sneerRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    break;
                case State.Sleep:
                    m_sleepTimer -= GameplaySystem.time.deltaTime;

                    if(m_sleepTimer < 0)
                    {
                        m_stateHandle.SetState(State.WakeUp);
                    }
                    break;
                case State.WakeUp:
                    StartCoroutine(WakeUpRoutine());
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Patrol);
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
    }
}
