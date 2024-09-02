﻿using System;
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
using Language.Lua;

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

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [SerializeField]
            private float m_suicideDelay;
            public float suicideDelay => m_suicideDelay;

            [SerializeField]
            private float m_minCoolDripTimer;
            public float minCoolDripTimer => m_minCoolDripTimer;

            [SerializeField]
            private float m_maxCoolDripTimer;
            public float maxCoolDripTimer => m_maxCoolDripTimer;

            [SerializeField]
            private float m_coolDripDropOffset;
            public float coolDripDropOffset => m_coolDripDropOffset;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_cowerAnimation;
            public BasicAnimationInfo cowerAnimation => m_cowerAnimation;
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
                m_cowerAnimation.SetData(m_skeletonDataAsset);
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

        private enum RetreatAxis
        {
            Horizontal,
            Vertical,
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
        [SerializeField, TabGroup("Reference")]
        private SkeletonAnimation m_skelAnimation;
        [SerializeField, TabGroup("Reference")]
        private Attacker m_bodyCollisionAttacker;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_groundMovement;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_wallMovement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private WayPointPatrol m_wayPointPatrol;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private IsolatedCharacterPhysics2D m_isolatedCharacterPhysics2D;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_rightWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_leftWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_rightEdgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_leftEdgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_escapePathSensor;

        [SerializeField, TabGroup("VFX Objects", "Ice Cloud")]
        private ParticleSystem m_detonationIceCloud;
        [SerializeField, TabGroup("VFX Objects", "Ice Cloud")]
        private Collider2D m_detonationDamageCollider;
        [SerializeField, TabGroup("VFX Objects", "Ice Cloud")]
        private Collider2D m_iceCloudStatusInflictionCollider;
        [SerializeField, TabGroup("VFX Objects", "Ice Trail")]
        private GameObject m_iceTrailObject;
        [SerializeField, TabGroup("VFX Objects", "Ice Drip")]
        private GameObject m_iceDrip;
        [SerializeField, TabGroup("VFX Objects", "Ice Drip")]
        private float m_coolDripTimer;
        [SerializeField, TabGroup("VFX Objects", "Death")]
        private ParticleSystem m_deathExplosionEffect;

        [InfoBox("For Wall Blobs on Right, the waypoints for patrol needs to be diagonal for the facing and patrol to work properly like this: / \n" +
            "For Wall Blobs on Left, the waypoints for patrol needs to be diagonal for the facing and patrol to work properly like this: " + @"\")]
        [ShowInInspector]
        private MovementHandle2D m_currentMovementHandle;
        [SerializeField]
        private IceBlobType m_iceBlobType;
        [ShowInInspector]
        private RetreatAxis m_retreatDirection;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [SerializeField]
        private Vector2 m_startingPosition;
        [ShowInInspector]
        private Collider2D m_aggroCollider;
        [ShowInInspector]
        private float m_cowerInFearDuration;
        [ShowInInspector]
        private bool m_isCowering;
        [ShowInInspector]
        private bool m_isRetreating;
        [ShowInInspector]
        private bool m_willDropCoolDrip;
        [ShowInInspector]
        private bool m_isRightWallBlob;

        private State m_turnState;

        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private bool m_isDetecting;

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (m_stateHandle.currentState != State.Retreat && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
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
            m_currentMovementHandle.Stop();
            m_selfCollider.enabled = false;
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_hitbox.Disable();
            m_selfCollider.enabled = false;
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_currentMovementHandle.Stop();
            m_bodyCollisionAttacker.SetDamageModifier(0);
            m_iceTrailObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //explode
            m_detonationDamageCollider.enabled = true;
            yield return new WaitForSeconds(0.5f);
            m_detonationDamageCollider.enabled = false;
            m_detonationIceCloud.Play();
            m_iceCloudStatusInflictionCollider.enabled = true;
            float cloudDuration = m_detonationIceCloud.main.duration;
            yield return new WaitForSeconds(cloudDuration);
            gameObject.SetActive(false);
            //wait for explosion to dissipate
            yield return null;
        }

        #region Blob Setup
        private void SetUpGroundBlob()
        {
            transform.rotation = Quaternion.identity;

            m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
            m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);

            m_iceTrailObject.SetActive(false);
            m_isolatedCharacterPhysics2D.simulateGravity = true;
            m_currentMovementHandle = m_groundMovement;
            m_retreatDirection = RetreatAxis.Horizontal;
            m_willDropCoolDrip = false;
        }

        private void SetUpCeilingBlob()
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0f, 0f, 180f));

            m_iceTrailObject.SetActive(true);
            m_isolatedCharacterPhysics2D.simulateGravity = false;
            m_currentMovementHandle = m_groundMovement;
            m_skelAnimation.initialFlipX = true;
            m_iceTrailObject.transform.localScale = new Vector3(-1, 1, 1);

            //Correct Sensor positions after flip
            m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.99f, 0);
            m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.99f, 0);
            m_escapePathSensor.transform.localPosition = new Vector3(-1, -0.70f, 0);

            m_escapePathSensor.GetComponent<RaySensorFaceRotator>().transform.localRotation = Quaternion.identity;

            m_leftEdgeSensor.transform.localPosition = new Vector3(1.5f, -3.5f, 0);
            m_rightEdgeSensor.transform.localPosition = new Vector3(-1.5f, -3.5f, 0);

            m_retreatDirection = RetreatAxis.Horizontal;
            m_willDropCoolDrip = true;
            //Set trail on
        }

        private void SetUpWallBlob()
        {
            transform.rotation = Quaternion.identity;

            m_rightWallSensor.Cast();
            if (m_rightWallSensor.isDetecting) //wall on right
            {
                transform.Rotate(new Vector3(0f, 0f, 90f));
                //Correct Sensor positions after flip
                m_rightWallSensor.transform.localPosition = new Vector3(-0.75f, -0.99f, 0);
                m_leftWallSensor.transform.localPosition = new Vector3(0.75f, -0.99f, 0);
                m_retreatDirection = RetreatAxis.Vertical;
                m_isRightWallBlob = true;
            }
            else //wall on left
            {
                transform.Rotate(new Vector3(0f, 0f, 270f));
                //Correct Sensor positions after flip
                m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.99f, 0);
                m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.99f, 0);
                m_retreatDirection = RetreatAxis.Vertical;
                m_isRightWallBlob = false;
            }

            m_iceTrailObject.SetActive(true);
            m_isolatedCharacterPhysics2D.simulateGravity = false;
            m_currentMovementHandle = m_wallMovement;
            m_willDropCoolDrip = false;
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
            
            //Set VFX stuff off if not needed, if needed setup in setup methods
            m_detonationDamageCollider.enabled = false;
            m_iceCloudStatusInflictionCollider.enabled = false;

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
            m_startingPosition = transform.position;
            m_aggroCollider = m_aggroBoundary.GetComponent<Collider2D>();
        }

        protected override void Awake()
        {
            base.Awake();
            
            if(m_iceBlobType != IceBlobType.Wall)
            {
                m_patrolHandle.TurnRequest += OnTurnRequest;
                m_turnHandle.TurnDone += OnTurnDone;
            }
            
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            if (m_willDropCoolDrip)
            {
                m_coolDripTimer -= Time.deltaTime;
                if(m_coolDripTimer <= 0)
                {
                    SpawnIceDrip();
                }
            }

            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_currentMovementHandle, m_info.move.speed, characterInfo);
                    if(m_iceBlobType == IceBlobType.Wall)
                    {
                        WallBlobManualTurn();
                    }
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
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
                
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private IEnumerator CowerRoutine()
        {
            m_stateHandle.Wait(State.Patrol);

            m_isRetreating = false;
            m_isCowering = true;
            m_cowerInFearDuration = m_info.cowerInFearDuration;
            m_iceTrailObject.SetActive(false);
            m_aggroCollider.enabled = true;

            while (m_isCowering)
            {
                m_cowerInFearDuration -= Time.deltaTime;          

                if (m_cowerInFearDuration <= 0)
                {
                    m_isCowering = false;
                }

                if (m_targetInfo.isValid)
                {
                    yield return new WaitForSeconds(m_info.suicideDelay);
                    m_damageable.KillSelf();
                }

                yield return null;
            }

            Debug.Log("Done Cowering");
            m_isDetecting = false;

            if(m_iceBlobType == IceBlobType.Wall || m_iceBlobType == IceBlobType.Ceiling)
            {
                m_iceTrailObject.SetActive(true);
            }

            m_cowerInFearDuration = m_info.cowerInFearDuration;

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

            AdjustSensorPositions();
            m_isRetreating = true;
            Vector2 retreatDirection = SetRetreatDirection();
            m_targetInfo.Set(null);
            m_aggroCollider.enabled = false;

            m_iceTrailObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.retreat.animation, true);

            while (!IsWallOrEdgeDetected())
            {
                m_currentMovementHandle.MoveTowards(retreatDirection, m_info.retreat.speed);
                yield return null;
            }

            m_turnHandle.ForceTurnImmidiately();
            m_currentMovementHandle.Stop();
            m_isRetreating = false;

            m_animation.SetAnimation(0, m_info.cowerAnimation, true);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void WallBlobManualTurn()
        {
            //Set up and down point by comparing waypoints to starting point
            //If current destination is equal to up point, face right
            //If current destination is equal to down point, face left
            //reverse if blob on left wall
            var moveInfo = m_wayPointPatrol.GetInfo(transform.position);
            var wayPoints = m_wayPointPatrol.GetWaypoints();
            Vector2 upPatrolPoint = wayPoints[0];
            Vector2 downPatrolPoint = wayPoints[1];
            var signDirection = Mathf.Sign(moveInfo.moveDirection.y);

            HorizontalDirection relativeDirection = HorizontalDirection.Right;
            if (m_isRightWallBlob)
            {
                relativeDirection = signDirection == 1 ? HorizontalDirection.Right : HorizontalDirection.Left;
            }
            else
            {
                relativeDirection = signDirection == 1 ? HorizontalDirection.Left : HorizontalDirection.Right;
            }

            if (relativeDirection != m_character.facing)
            {
                //Turn
                m_turnHandle.ForceTurnImmidiately();
            }
        }

        private void AdjustSensorPositions()
        {
            switch(m_iceBlobType)
            {
                case IceBlobType.Ground:
                    if(m_character.facing == HorizontalDirection.Right)
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    else
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    break;
                case IceBlobType.Ceiling:
                    if (m_character.facing == HorizontalDirection.Right)
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    else
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    break;
                case IceBlobType.Wall:
                    if (m_character.facing == HorizontalDirection.Right)
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    else
                    {
                        m_rightWallSensor.transform.localPosition = new Vector3(0.75f, -0.9f, 0);
                        m_leftWallSensor.transform.localPosition = new Vector3(-0.75f, -0.9f, 0);
                    }
                    break;
            }
        }

        private Vector2 SetRetreatDirection()
        {
            Vector2 direction = Vector2.zero;
            switch(m_retreatDirection)
            {
                case RetreatAxis.Horizontal:
                    if(m_targetInfo.position.x > transform.position.x) //PLayer is right of blob
                    {
                        if(m_character.facing == HorizontalDirection.Right)
                        {
                            m_turnHandle.ForceTurnImmidiately();
                        }

                        m_escapePathSensor.Cast();

                        if (!m_escapePathSensor.m_castHit)
                        {
                        direction = Vector2.left;
                            
                        }
                    }
                    else //Player on left
                    {
                        if (m_character.facing == HorizontalDirection.Left)
                        {
                            m_turnHandle.ForceTurnImmidiately();
                        }

                        m_escapePathSensor.Cast();

                        if (!m_escapePathSensor.m_castHit)
                        {
                        direction = Vector2.right;
                            
                        }
                    }
                    break;
                case RetreatAxis.Vertical:
                    if (m_targetInfo.position.y > transform.position.y)
                    {
                        direction = Vector2.down;
                    }
                    else
                    {
                        direction = Vector2.up;
                    }
                    break;
            }
            return direction;
        }

        private bool IsWallOrEdgeDetected()
        {
            bool result = false;
            if(m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting /*|| !m_leftEdgeSensor.isDetecting*/ || !m_rightEdgeSensor.isDetecting)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private void SpawnIceDrip()
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_iceDrip, gameObject.scene);
            var spawnPos = new Vector2(transform.position.x, transform.position.y - m_info.coolDripDropOffset);
            instance.SpawnAt(spawnPos, Quaternion.identity);

            var RandomTime = UnityEngine.Random.Range(m_info.minCoolDripTimer, m_info.maxCoolDripTimer);
            m_coolDripTimer = RandomTime;
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.Patrol);
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
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
