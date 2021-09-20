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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/EyeFlying")]
    public class EyeFlyingAI : CombatAIBrain<EyeFlyingAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
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
            private string m_hardenAnimation;
            public string hardenAnimation => m_hardenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                //
#endif
            }
        }

        private enum State
        {
            Idle,
            Harden,
            Dead,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            [HideInInspector]
            _COUNT
        }

        //[SerializeField, TabGroup("Modules")]
        //private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;
        

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        //private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.SetState(State.Harden);
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
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

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        protected override void Awake()
        {
            base.Awake();
            //m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //m_animation.SetEmptyAnimation(0, 0);
                    break;

                case State.Harden:
                    {
                        m_hitbox.SetInvulnerability(Invulnerability.MAX);
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.hardenAnimation, true);
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Harden);
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

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}
