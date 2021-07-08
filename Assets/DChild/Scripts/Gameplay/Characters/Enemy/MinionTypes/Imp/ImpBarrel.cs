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
    //[AddComponentMenu("DChild/Gameplay/Enemies/Minion/ImpBarrel")]
    public class ImpBarrel : CombatAIBrain<ImpBarrel.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_plantBombAnimation;
            public string plantBombAnimation => m_plantBombAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_bombFallingLoopAnimation;
            public string bombFallingLoopAnimation => m_bombFallingLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_bombExplosionAnimation;
            public string bombExplosionAnimation => m_bombExplosionAnimation;

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
            Explode,
            WaitBehaviourEnd,
        }

        //[SerializeField, TabGroup("Modules")]
        //private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private BaseColliderDamage m_explosionDamageable;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_explosionHitBox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_playerTrigger;
        [SerializeField, TabGroup("Reference")]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        //[SerializeField, TabGroup("Sensors")]
        //private RaySensor m_playerSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_explosionFX;
        //Patience Handler
        [SerializeField]
        private bool m_idled;

        private ImpAI m_imp;


        private float m_currentSpawnTime;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        //private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public void GetTarget(AITargetInfo target)
        {
            m_targetInfo = target;
        }

        public void SetImp(ImpAI imp)
        {
            m_imp = imp;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.tag != "Sensor" && colliderGameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (m_idled)
                {
                    m_idled = false;
                }
                else
                {
                    StopAllCoroutines();
                    m_skeletonAnimation.maskInteraction = SpriteMaskInteraction.None;
                    m_animation.SetEmptyAnimation(0, 0);
                    m_animation.SetAnimation(0, m_info.bombExplosionAnimation, false).TimeScale = 10;
                    StartCoroutine(ExplodeRoutine());
                }
            }
        }

        //Patience Handler

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_skeletonAnimation.maskInteraction = SpriteMaskInteraction.None;
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.bombExplosionAnimation, false).TimeScale = 10;
            StartCoroutine(ExplodeRoutine());
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator SpawnRoutine()
        {
            m_animation.SetAnimation(0, m_info.bombFallingLoopAnimation, true);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.plantBombAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.plantBombAnimation);
            //yield return new WaitUntil(() => !m_idled);
            m_skeletonAnimation.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            while (m_idled)
            {
                //if (m_playerSensor.isDetecting)
                //{
                //    m_idled = false;
                //}
                yield return null;
            }
            m_skeletonAnimation.maskInteraction = SpriteMaskInteraction.None;
            m_animation.SetAnimation(0, m_info.bombExplosionAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bombExplosionAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ExplodeRoutine()
        {
            m_playerTrigger.enabled = false;
            m_hitbox.Disable();
            m_character.physics.SetVelocity(Vector2.zero);
            m_character.physics.simulateGravity = false;
            m_explosionHitBox.enabled = true;
            m_explosionFX.Play();
            yield return new WaitForSeconds(.15f);
            m_explosionHitBox.enabled = false;
            yield return new WaitForSeconds(2f);
            Destroy(this.gameObject);
            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
            //m_turnHandle.TurnDone += OnTurnDone;
            m_explosionDamageable.DamageableDetected += Laugh;
            //m_deathHandle.SetAnimation(null);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }

        private void Laugh(TargetInfo arg1, Collider2D arg2)
        {
            m_imp.Laugh();
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_stateHandle.Wait(State.Explode);
                    StartCoroutine(SpawnRoutine());
                    break;
                case State.Explode:
                    m_stateHandle.Wait(State.WaitBehaviourEnd);
                    StartCoroutine(ExplodeRoutine());
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
        }

        protected override void OnBecomePassive()
        {

        }
    }
}
