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
    public class EyebatAI : CombatAIBrain<EyebatAI.Info>
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
            private SimpleAttackInfo m_attackMove = new SimpleAttackInfo();
            public SimpleAttackInfo attackMove => m_attackMove;
            [SerializeField]
            private float m_attackRiseSpeed;
            public float attackRiseSpeed => m_attackRiseSpeed;
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
            private string m_alertAnimation;
            public string alertAnimation => m_alertAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_damageAnimation;
            public string damageAnimation => m_damageAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swoopLoopAnimation;
            public string swoopLoopAnimation => m_swoopLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swoopMissAnimation;
            public string swoopMissAnimation => m_swoopMissAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swoopStartAnimation;
            public string swoopStartAnimation => m_swoopStartAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                attack.SetData(m_skeletonDataAsset);
                attackMove.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Patrol,
            Turning,
            Alert,
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

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private ProjectileLauncher m_stingerLauncher;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isAlerted;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                if (m_isAlerted)
                {
                    m_stateHandle.SetState(State.Chasing);
                }
                else
                {
                    m_stateHandle.SetState(State.Alert);
                    m_isAlerted = true;
                }
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
            m_animation.SetAnimation(0, m_info.damageAnimation, false);
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
                m_isAlerted = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

        private IEnumerator AlertRoutine()
        {
            m_animation.SetAnimation(0, m_info.alertAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.alertAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            //m_attackHandle.ExecuteAttack(m_info.attackMove.animation);
            m_animation.SetAnimation(0, m_info.attackMove.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackMove.animation);
            m_agent.Stop();
            if (!IsFacingTarget())
            {
                m_turnHandle.Execute();
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_animation.EnableRootMotion(true, false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_info.attackRiseSpeed), ForceMode2D.Impulse);
            yield return new WaitForSeconds(5);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Idle")
            {
                return m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
            }
            else
            {
                return true;
            }
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);

        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Alert:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_agent.Stop();
                    StartCoroutine(AlertRoutine());
                    break;
                case State.Patrol:
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:

                    m_stateHandle.Wait(State.ReevaluateSituation);

                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:

                    m_stateHandle.Wait(State.ReevaluateSituation);

                    m_agent.Stop();
                    //m_animation.EnableRootMotion(true, true);
                    //m_attackHandle.ExecuteAttack(m_info.attackMove.animation);
                    //m_animation.SetAnimation(0, m_info.attackMove.animation, true);
                    m_stateHandle.Wait(State.WaitBehaviourEnd);
                    StartCoroutine(AttackRoutine());
                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        var target = m_targetInfo.position;
                        target.y += 3f;

                        if (IsTargetInRange(m_info.attackMove.range))
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            if (Wait())
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                                m_agent.SetDestination(target);

                                m_agent.Move(m_info.move.speed);
                            }
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

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
        }

    }
}

