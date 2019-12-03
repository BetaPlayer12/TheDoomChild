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
    public class QueenBeeAI : CombatAIBrain<QueenBeeAI.Info>
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
            [SerializeField]
            private MovementInfo m_moveNoMinion = new MovementInfo();
            public MovementInfo moveNoMinion => m_moveNoMinion;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private SimpleAttackInfo m_minionAttack1 = new SimpleAttackInfo();
            public SimpleAttackInfo minionAttack1 => m_minionAttack1;
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
            private string m_idleNoMinionAnimation;
            public string idleNoMinionAnimation => m_idleNoMinionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2NoMinionAnimation;
            public string idle2NoMinionAnimation => m_idle2NoMinionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_recoverFromStuckAnimation;
            public string recoverFromStuckAnimation => m_recoverFromStuckAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_summonBeeAnimation;
            public string summonBeeAnimation => m_summonBeeAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro2Animation;
            public string intro2Animation => m_intro2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro3Animation;
            public string intro3Animation => m_intro3Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stuckAnimation;
            public string stuckAnimation => m_stuckAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchNoMinionAnimation;
            public string flinchNoMinionAnimation => m_flinchNoMinionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchStuckAnimation;
            public string flinchStuckAnimation => m_flinchStuckAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_moveNoMinion.SetData(m_skeletonDataAsset);
                attack.SetData(m_skeletonDataAsset);
                attack2.SetData(m_skeletonDataAsset);
                minionAttack1.SetData(m_skeletonDataAsset);

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
            ReevaluateSituation,
            WaitBehaviourEnd,
        }
        
        [SerializeField, TabGroup("Modules")]
        private SimpleTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private ProjectileLauncher m_stingerLauncher;
        private float m_currentPatience;
        private bool m_enablePatience;
        private float timeCounter;
        private bool m_chargeOnce;
        private bool m_chargeFacing;

        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {

                base.SetTarget(damageable, m_target);

                m_currentPatience = 0;
                m_enablePatience = false;

                m_stateHandle.SetState(State.Chasing);


            }
            else
            {
                if (!IsTargetInRange(m_info.targetDistanceTolerance))
                {
                    m_enablePatience = true;
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }






        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            timeCounter = 0;
            m_chargeOnce = false;
            m_chargeFacing = false;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);

        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.Patrol:

                    // if (!m_wallSensor.isDetecting && !m_floorSensor.isDetecting && !m_cielingSensor.isDetecting) //This means that as long as your sensors are detecting something it will patrol
                    // {
                    m_chargeOnce = false;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    timeCounter = 0;
                    // break;
                    // }
                    // else
                    // {
                    //    m_stateHandle.SetState(State.Turning);
                    //   Debug.Log("sensor test patrol");
                    //  }
                    break;

                case State.Turning:

                    m_stateHandle.Wait(State.ReevaluateSituation);

                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:

                    m_agent.Stop();
                    m_animation.EnableRootMotion(false, false);
                    m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    m_animation.SetAnimation(0, m_info.attack.animation, true);
                    m_stateHandle.Wait(State.WaitBehaviourEnd);
                    //m_Audiosource.clip = m_AttackClip;
                    //m_Audiosource.Play();
                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {




                        var target = m_targetInfo.position;
                        target.y -= 0.5f;
                        m_animation.DisableRootMotion();

                        if (IsTargetInRange(m_info.attack.range))
                        {
                            // m_agent.Stop();

                            m_stateHandle.SetState(State.Attacking);

                        }
                        else
                        {

                            if (m_character.physics.velocity != Vector2.zero)
                            {
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.patrol.animation, true);
                            }
                            m_agent.SetDestination(target);

                            m_agent.Move(m_info.move.speed);

                        }







                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);

                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
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


    }
}

