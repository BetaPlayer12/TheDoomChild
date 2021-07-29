using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HomunculusFemaleAI : CombatAIBrain<HomunculusFemaleAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            private const string ATTACKNAME = "LightningSphere";

            [SerializeField, Range(0, 100), BoxGroup("Idle During Patrol")]
            private int m_chanceToIdleDuringPatrol;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Idle During Patrol")]
            private string m_idleAnimation;
            [SerializeField, MinValue(1), BoxGroup("Idle During Patrol")]
            private RangeFloat m_idleDuringPatrolDuration;
            [SerializeField, MinValue(1), BoxGroup("Idle During Patrol")]
            private RangeFloat m_idleDuringPatrolCooldown;

            [SerializeField]
            private MovementInfo m_walkInfo = new MovementInfo();
            [SerializeField]
            private MovementInfo m_lightningArmorWalkInfo = new MovementInfo();

            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lightningCoreBurstAnimation;
            [SerializeField, BoxGroup(ATTACKNAME)]
            private SimpleAttackInfo m_lightningSphereAttackInfo = new SimpleAttackInfo();
            [SerializeField, MinValue(1), BoxGroup(ATTACKNAME)]
            private int m_maxLightningSphereUse = 1;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup(ATTACKNAME)]
            private string m_postLightningSphereIdleAnimation;
            [SerializeField, MinValue(1), BoxGroup(ATTACKNAME)]
            private float m_postLightningSphereIdleDuration;



            public int chanceToIdleDuringPatrol => m_chanceToIdleDuringPatrol;
            public string idleAnimation => m_idleAnimation;
            public string lightningCoreBurstAnimation => m_lightningCoreBurstAnimation;
            public SimpleAttackInfo lightningSphereAttackInfo => m_lightningSphereAttackInfo;
            public int maxLightningSphereUse => m_maxLightningSphereUse;
            public string postLightningSphereIdleAnimation => m_postLightningSphereIdleAnimation;
            public float postLightningSphereIdleDuration => m_postLightningSphereIdleDuration;


            public MovementInfo walkInfo => m_walkInfo;
            public MovementInfo lightningArmorWalkInfo => m_lightningArmorWalkInfo;
            public float GetRandomIdleDuringPatrolCooldown() => m_idleDuringPatrolCooldown.GenerateRandomValue();
            public float GetRandomIdleDuringPatrolDuration() => m_idleDuringPatrolDuration.GenerateRandomValue();

            public override void Initialize()
            {
                m_walkInfo.SetData(m_skeletonDataAsset);
                m_lightningArmorWalkInfo.SetData(m_skeletonDataAsset);
                m_lightningSphereAttackInfo.SetData(m_skeletonDataAsset);
            }
        }

        private enum State
        {
            Idle,
            PatrolIdle,
            Patrol,
            Flinch,
            Attack,
            Chase,
            ReevaluateSituation,
            WaitForBehaviour
        }

        [SerializeField]
        private WayPointPatrol m_patrolHandle;
        [SerializeField]
        private MovementHandle2D m_moveHandle;
        [SerializeField]
        private TransformTurnHandle m_turnHandle;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private bool m_canIdleDuringPatrol;
        private float m_idleDuringPatrolCooldownTimer;
        private float m_idleDuringPatrolDurationTimer;
        private bool m_isInRageMode;
        private int m_availableLightningSphereUse;

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_target == null && m_targetInfo.doesTargetExist)
            {

            }
            else
            {
                base.SetTarget(damageable, m_target);
                if (m_target != null)
                {
                    if (m_isInRageMode == false)
                    {
                        m_isInRageMode = true;
                        StartCoroutine(LightningCoreBurstRoutine());
                    }
                }
            }
        }

        protected override void OnBecomePassive()
        {

        }

        protected override void OnTargetDisappeared()
        {

        }


        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {

        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Patrol)
            {
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
            m_turnHandle.Execute();
        }

        private void HandleChanceToIdle(float chance, float deltaTime)
        {
            if (m_canIdleDuringPatrol)
            {
                if (UnityEngine.Random.Range(0, 100) >= chance)
                {
                    m_stateHandle.SetState(State.PatrolIdle);
                    m_idleDuringPatrolDurationTimer = m_info.GetRandomIdleDuringPatrolDuration();
                }
            }
            else
            {
                m_idleDuringPatrolCooldownTimer -= deltaTime;
                if (m_idleDuringPatrolCooldownTimer <= 0)
                {
                    m_canIdleDuringPatrol = true;
                }
            }
        }

        private IEnumerator LightningCoreBurstRoutine()
        {
            m_isInRageMode = true;
            m_moveHandle.Stop();
            ResetIdleChanceData();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.lightningCoreBurstAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningCoreBurstAnimation);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator LightningSphereRoutine()
        {
            m_availableLightningSphereUse--;
            m_moveHandle.Stop();
            m_stateHandle.Wait(State.Idle);
            m_animation.SetAnimation(0, m_info.lightningSphereAttackInfo.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningSphereAttackInfo.animation);

            if (m_availableLightningSphereUse <= 0)
            {
                m_isInRageMode = false;
            }
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator IdleRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_isInRageMode)
            {
                m_animation.SetAnimation(0, m_info.postLightningSphereIdleAnimation, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(m_info.postLightningSphereIdleDuration);

            if (m_isInRageMode == false)
            {
                m_isInRageMode = true;
                StartCoroutine(LightningCoreBurstRoutine());
            }
            m_stateHandle.ApplyQueuedState();
        }

        private void ResetIdleChanceData()
        {
            m_canIdleDuringPatrol = true;
            m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
            m_idleDuringPatrolDurationTimer = m_info.GetRandomIdleDuringPatrolDuration();
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.Initialize();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitForBehaviour);
            m_turnHandle.TurnDone += OnTurnDone;
            m_canIdleDuringPatrol = true;
            m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
            m_availableLightningSphereUse = m_info.maxLightningSphereUse;
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    var patrolCharacterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_moveHandle, m_info.walkInfo.speed, patrolCharacterInfo);
                    m_animation.SetAnimation(0, m_info.walkInfo.animation, true);
                    HandleChanceToIdle(m_info.chanceToIdleDuringPatrol, GameplaySystem.time.deltaTime);
                    break;
                case State.PatrolIdle:
                    m_moveHandle.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_idleDuringPatrolDurationTimer -= GameplaySystem.time.deltaTime;
                    if (m_idleDuringPatrolDurationTimer <= 0)
                    {
                        m_stateHandle.SetState(State.Patrol);
                        m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
                        m_canIdleDuringPatrol = false;
                    }
                    break;
                case State.Flinch:
                    //uf animation is finish 
                    StartCoroutine(LightningSphereRoutine());
                    break;
                case State.Idle:
                    StopAllCoroutines();
                    StartCoroutine(IdleRoutine());
                    break;
                case State.Attack:
                    StopAllCoroutines();
                    StartCoroutine(LightningSphereRoutine());
                    break;

                case State.Chase:
                    var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                    if (toTarget.magnitude > m_info.lightningSphereAttackInfo.range)
                    {
                        if (Mathf.Sign(toTarget.x) == (int)m_character.facing)
                        {
                            m_moveHandle.MoveTowards(toTarget.normalized, m_info.lightningArmorWalkInfo.speed);
                            m_animation.SetAnimation(0, m_info.lightningArmorWalkInfo.animation, true);
                        }
                        else
                        {
                            m_turnHandle.Execute();
                        }
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Attack);
                    }
                    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.doesTargetExist)
                    {
                        m_stateHandle.SetState(State.Chase);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitForBehaviour:
                    break;
            }
        }
    }

}