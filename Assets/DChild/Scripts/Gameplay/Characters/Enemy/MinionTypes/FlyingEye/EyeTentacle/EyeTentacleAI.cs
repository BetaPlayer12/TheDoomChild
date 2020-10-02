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
    public class EyeTentacleAI : CombatAIBrain<EyeTentacleAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_groundedIdleAnimation;
            public string groundedIdleAnimation => m_groundedIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_floatingIdleAnimation;
            public string floatingIdleAnimation => m_floatingIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR

#endif
            }
        }

        private enum State
        {
            Idle,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.SetState(State.Idle);
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Awake()
        {
            base.Awake();
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_groundSensor.isDetecting ? m_info.groundedIdleAnimation : m_info.floatingIdleAnimation, true);

                    break;

                case State.ReevaluateSituation:

                    m_stateHandle.OverrideState(State.Idle);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
            if (m_groundSensor.isDetecting)
            {
                Debug.Log("GROUNDED DEDDED");
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}

