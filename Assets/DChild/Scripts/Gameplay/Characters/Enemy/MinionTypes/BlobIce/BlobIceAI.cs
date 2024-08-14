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
            [SerializeField, MinValue(0)]
            private float m_cowerInFearDuration;
            public float cowerInFearDuration => m_cowerInFearDuration;
            [SerializeField, MinValue(0)]
            private float m_deathDuration;
            public float deathDuration => m_deathDuration;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_hitboxStartEvent;
            public string hitboxStartEvent => m_hitboxStartEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_retreat.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum IceBlobType
        {
            Ground,
            Ceiling,
            Wall
        }

        private enum State
        {
            Patrol,
            Turning,
            ReevaluateSituation,
            WaitBehaviourEnd,
            Detect,
            Retreat,
            Cower,
        }

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
        private IsolatedCharacterPhysics2D m_isolatedCharacterPhysics2D;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_iceTrailSensor;

        [SerializeField]
        private IceBlobType m_iceBlobType;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private float m_cowerInFearDuration;
        [ShowInInspector]
        private bool m_isCowering;
        [ShowInInspector]
        private bool m_isRetreating;

        private State m_turnState;

        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            base.SetTarget(damageable);
            if (m_stateHandle.currentState != State.Retreat)
            {
                if (IsFacingTarget())
                {
                    m_turnHandle.ForceTurnImmidiately();
                }

                m_stateHandle.SetState(State.Retreat);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
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
            //explode 
            //wait for explosion to dissipate
            gameObject.SetActive(false);
            yield return null;
        }

        #region Blob Setup
        private void SetUpGroundBlob()
        {
            transform.rotation = Quaternion.identity;

            m_isolatedCharacterPhysics2D.simulateGravity = true;
        }

        private void SetUpCeilingBlob()
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0f, 0f, 180f));

            m_isolatedCharacterPhysics2D.simulateGravity = false;

            //Set trail on
        }

        private void SetUpWallBlob()
        {
            transform.rotation = Quaternion.identity;           

            m_wallSensor.Cast();
            if (m_wallSensor.isDetecting)
            {
                transform.Rotate(new Vector3(0f, 0f, 90f));
            }
            else
            {
                transform.Rotate(new Vector3(0f, 0f, 270f));
            }

            m_isolatedCharacterPhysics2D.simulateGravity = false;

            //Set trail on
        }
        #endregion

        public override void ApplyData()
        {
            base.ApplyData();
        }

        protected override void Start()
        {
            base.Start();

            switch (m_iceBlobType)
            {
                case IceBlobType.Ground:
                    SetUpGroundBlob();
                    break;
                case IceBlobType.Ceiling:
                    SetUpCeilingBlob();
                    break;
                case IceBlobType.Wall:
                    SetUpWallBlob();
                    break;
            }

            m_cowerInFearDuration = m_info.cowerInFearDuration;
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_turnHandle.TurnDone += OnTurnDone;
            
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            if(m_isCowering)
            {
                if (m_targetInfo.isValid)
                {
                    if (IsTargetInRange(2f))
                    {
                        m_damageable.KillSelf();
                        m_isCowering = false;
                    }
                }               
            }

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
                    StartCoroutine(DetectRoutine());
                    break;
                case State.Retreat:
                    StartCoroutine(RetreatRoutine());
                    break;
                case State.Cower:
                    StartCoroutine(CowerRoutine());
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private IEnumerator CowerRoutine()
        {
            m_stateHandle.Wait(State.Patrol);

            m_isCowering = true;
            m_cowerInFearDuration = m_info.cowerInFearDuration;

            while(m_cowerInFearDuration > 0)
            {
                m_cowerInFearDuration -= Time.deltaTime;

                yield return null;
            }

            m_isCowering = false;

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_stateHandle.Wait(State.Retreat);

            if (IsFacingTarget())
            {
                m_turnHandle.ForceTurnImmidiately();
            }

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }


        private IEnumerator RetreatRoutine()
        {
            m_stateHandle.Wait(State.Cower);

            //If player is on right, turn left, vice versa
            //Set destination equal to wall sensor distance x, current y, vice versa if wall blob
            //Move while wall sensor is not detecting

            m_isRetreating = true;
            if (m_targetInfo.position.x > transform.position.x)
            {
                if (m_character.facing == HorizontalDirection.Right)
                {
                    m_turnHandle.ForceTurnImmidiately();
                }
            }
            else
            {
                if (m_character.facing == HorizontalDirection.Left)
                {
                    m_turnHandle.ForceTurnImmidiately();
                }
            }

            m_animation.SetAnimation(0, m_info.retreat.animation, true);

            while (!m_wallSensor.isDetecting && m_edgeSensor.isDetecting)
            {
                m_movement.MoveTowards(Vector2.right, m_info.retreat.speed);
                yield return null;
            }

            m_movement.Stop();
            m_isRetreating = false;

            m_animation.SetAnimation(0, m_info.idleAnimation, true);

            m_stateHandle.ApplyQueuedState();
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
