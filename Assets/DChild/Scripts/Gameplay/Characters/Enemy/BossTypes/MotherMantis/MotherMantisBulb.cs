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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/MotherMantisBulb")]
    public class MotherMantisBulb : CombatAIBrain<MotherMantisBulb.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            //

            [SerializeField]
            private float m_spawnTime;
            public float spawnTime => m_spawnTime;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_growAnimation;
            public string growAnimation => m_growAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_openAnimation;
            public string openAnimation => m_openAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleOpenAnimation;
            public string idleOpenAnimation => m_idleOpenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_flinchAnimation;
            //public string flinchAnimation => m_flinchAnimation;

            

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
        private ParticleFX m_spore;
        public ParticleFX spore => m_spore;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_anticipation;
        public ParticleFX anticipation => m_anticipation;
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
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //}

        private IEnumerator GrowRoutine()
        {
            m_stateHandle.Wait(State.Idle);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_spawnFX.Play();
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.growAnimation, false).TimeScale = 10;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.growAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(1f);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DeathRoutine()
        {
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_animation.SetAnimation(0, m_info.openAnimation, false).TimeScale=10;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.openAnimation);
            anticipation.Play();
            yield return new WaitForSeconds(.2f);
            m_animation.SetAnimation(0, m_info.idleOpenAnimation, false).TimeScale = 2;
            yield return new WaitForSeconds(.5f);
            spore.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleOpenAnimation);
            gameObject.SetActive(false);
            yield return null;
        }

       

        protected override void Awake()
        {
            base.Awake();
            //m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.openAnimation);
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

                    if (m_currentSpawnTime <= m_info.spawnTime)
                    {
                        m_hitbox.SetInvulnerability(Invulnerability.None);
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_currentSpawnTime += Time.deltaTime;
                    }
                    else
                    {
                        m_currentSpawnTime = 0;
                        m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                        StartCoroutine(DeathRoutine());

                    }
                    //m_animation.SetEmptyAnimation(0, 0);
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
