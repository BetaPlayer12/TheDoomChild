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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BlobIce")]
    public class BlobIceAI : CombatAIBrain<BlobIceAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_retreat = new MovementInfo();
            public MovementInfo retreat => m_retreat;

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

            [SerializeField, BoxGroup("Blob Ice Cloud")]
            private GameObject m_blobIceCloud;
            public GameObject blobIceCloud => m_blobIceCloud;

            [SerializeField, BoxGroup("Blob Ice Trail")]
            private GameObject m_blobIceTrail;
            public GameObject blobIceTrail => m_blobIceTrail;



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
            ReevaluateSituation,
            WaitBehaviourEnd,
            Detect,
            Retreat,
        }

        private enum Attack
        {
            Attack,
            [HideInInspector]
            _COUNT
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
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_iceTrailSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private State m_turnState;

        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomIdleRoutine;
        private Coroutine m_randomTurnRoutine;

        [SerializeField]
        private Renderer m_renderer;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            base.SetTarget(damageable);
            if (m_stateHandle.currentState != State.Retreat)
            {
                m_stateHandle.SetState(State.Detect);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            GameplaySystem.minionManager.Unregister(GetComponent<ICombatAIBrain>());
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
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_hitbox.Disable();
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //m_animation.SetAnimation(0, m_info.disassembledIdleAnimation, true);
            yield return new WaitForSeconds(m_info.deathDuration);
            m_health.SetHealthPercentage(1f);
            enabled = true;
            m_animation.SetAnimation(0, m_info.recoverAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.recoverAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.Patrol);
            InstantiateBlobIceCloud(transform.position);
            gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator RetreatRoutine()
        {
            if (!m_wallSensor.isDetecting)
            {
                if (m_character.facing == HorizontalDirection.Right)
                {
                    transform.Translate(Vector2.right * m_info.retreat.speed * GameplaySystem.time.deltaTime);
                }
                else if (m_character.facing == HorizontalDirection.Left)
                {
                    transform.Translate(Vector2.left * m_info.retreat.speed * GameplaySystem.time.deltaTime);
                }

                InstantiateBlobIceTrail();
            }
            else
            {
                m_movement.Stop();

                if (m_targetInfo.doesTargetExist)
                {
                    StartCoroutine(DeathRoutine());
                }
            }
            

            if (!m_renderer.isVisible)
            {
                gameObject.SetActive(false);
            }

            
            yield return null;
        }

        private void InstantiateBlobIceCloud(Vector2 spawnPosition)
        {
            var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_info.blobIceCloud, gameObject.scene);
            instance.transform.position = spawnPosition;
            //var component = instance.GetComponent<ParticleFX>();
            //component.ResetState();
        }

        private void InstantiateBlobIceTrail()
        {
            var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_info.blobIceTrail, gameObject.scene);

            if (!m_iceTrailSensor.isDetecting)
            {
                instance.transform.position = m_iceTrailSensor.transform.position;
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }

        protected override void Start()
        {
            base.Start();
            //m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);

            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            GameplaySystem.minionManager.Register(GetComponent<ICombatAIBrain>());
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
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.move.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.move.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
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

                case State.Detect:
                    if (IsFacingTarget())
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);                       
                    }
                    else
                    {
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Retreat:
                    StartCoroutine(RetreatRoutine());
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private IEnumerator DetectRoutine()
        {
            m_stateHandle.SetState(State.Retreat);
            yield return null;
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
