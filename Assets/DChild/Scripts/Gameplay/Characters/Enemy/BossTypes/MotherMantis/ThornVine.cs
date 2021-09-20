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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/ThornVine")]
    public class ThornVine : CombatAIBrain<ThornVine.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_growAnimation;
            public string growAnimation => m_growAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_death2Animation;
            public string death2Animation => m_death2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                //
#endif
            }
        }

        private enum State
        {
            Grow,
            Idle,
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
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_spawnFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_deathFX;
        //Patience Handler


        private float m_currentSpawnTime;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        //private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public void GetTarget(AITargetInfo target)
        {
            m_targetInfo = target;
        }

        //Patience Handler

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            StartCoroutine(DeathFXRoutine());
            base.OnDestroyed(sender, eventArgs);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.Wait(State.Idle);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator GrowRoutine()
        {
            m_stateHandle.Wait(State.Idle);
            m_spawnFX.Play();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.growAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.growAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DeathFXRoutine()
        {
            yield return new WaitForSeconds(.25f);
            m_deathFX.Play();
            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
            var sizeMult = UnityEngine.Random.Range(60, 120) * .01f;
            transform.localScale = new Vector2(transform.localScale.x * sizeMult, transform.localScale.y * sizeMult);
            //m_turnHandle.TurnDone += OnTurnDone;
            var deathAnim = UnityEngine.Random.Range(0, 2) == 1 ? m_info.deathAnimation : m_info.death2Animation;
            m_deathHandle.SetAnimation(deathAnim);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Grow, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Grow:
                    StartCoroutine(GrowRoutine());
                    break;
                case State.Idle:
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Grow);
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}
