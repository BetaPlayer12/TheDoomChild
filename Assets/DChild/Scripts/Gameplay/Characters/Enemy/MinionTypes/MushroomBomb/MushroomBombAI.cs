using System;
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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/MushroomBomb")]
    public class MushroomBombAI : CombatAIBrain<MushroomBombAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private float m_chargeTime;
            public float chargeTime => m_chargeTime;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_explodeAnimation;
            public string explodeAnimation => m_explodeAnimation;
            //

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeCharging;
            public string smokeCharging => m_smokeCharging;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            Dead,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Pound,
            Punch,
            OraOra,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_aggroSensor;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_explosionRadius;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_smokeChargeFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_poisonExplodeFX;

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        protected override void Start()
        {
            base.Start();

            //m_spineEventListener.Subscribe(m_info.smokeCharging, m_smokeChargeFX.Play);
            //GameplaySystem.SetBossHealth(m_character);
        }

        //private void SmokeCharging()
        //{
        //    Debug.Log("Scream Attack");
        //}

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_stateHandle.currentState != State.Dead)
            {
                if (damageable != null)
                {
                    base.SetTarget(damageable, m_target);
                    m_stateHandle.SetState(State.Chasing);
                    m_currentPatience = 0;
                    m_enablePatience = false;
                    //StopCoroutine(PatienceRoutine());
                }
                else
                {
                    m_enablePatience = true;
                    //StartCoroutine(PatienceRoutine());
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_targetInfo.Set(null, null);
                m_enablePatience = false;
                m_stateHandle.SetState(State.Idle);
            }
        }

        private IEnumerator ChargeRoutine()
        {
            m_stateHandle.Wait(State.Dead);
            m_aggroSensor.SetActive(false);
            m_smokeChargeFX.Play();
            m_animation.SetAnimation(0, m_info.attack.animation, true);
            yield return new WaitForSeconds(m_info.chargeTime);
            //m_hitbox.gameObject.SetActive(false);
            m_explosionRadius.SetActive(true);
            m_animation.SetAnimation(0, m_info.explodeAnimation, false);
            m_hitbox.SetInvulnerability(true);
            m_smokeChargeFX.Stop();
            m_poisonExplodeFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.explodeAnimation);
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
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
                    //Debug.Log("Patrolling");
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        //Debug.Log("Going to Patrol Pos");
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        //Debug.Log("Patrol Turn");
                        m_movement.Stop();
                        m_turnState = State.ReevaluateSituation;
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    //m_animation.EnableRootMotion(true, false);
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    StartCoroutine(ChargeRoutine());
                    break;
                case State.Dead:
                    gameObject.SetActive(false);
                    m_explosionRadius.SetActive(false);
                    m_targetInfo.Set(null, null);
                    m_enablePatience = false;
                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            if (IsTargetInRange(m_info.attack.range))
                            {
                                m_movement.Stop();
                                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_movement.Stop();
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
            m_currentPatience = 0;
            m_enablePatience = false;
        }
    }
}
