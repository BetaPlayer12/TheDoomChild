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
using DChild.Gameplay.Pathfinding;
using DarkTonic.MasterAudio;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Collections;
using Holysoft.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BlobIce (Wall)")]
    public class BlobIceWallAI : CombatAIBrain<BlobIceWallAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_retreat = new MovementInfo();
            public MovementInfo retreat => m_retreat;

            //
            [SerializeField, BoxGroup("Patience"), MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField, BoxGroup("Patience")]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField, MinValue(0)]
            private float m_deathDuration;
            public float deathDuration => m_deathDuration;

            //Animations
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathLoopAnimation;
            public string deathLoopAnimation => m_deathLoopAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathEndAnimation;
            public string deathEndAnimation => m_deathEndAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;

            [SerializeField, BoxGroup("Blob Ice Cloud")]
            private GameObject m_blobIceCloud;
            public GameObject blobIceCloud => m_blobIceCloud;

            [SerializeField, BoxGroup("Blob Ice Trail")]
            private GameObject m_blobIceTrail;
            public GameObject blobIceTrail => m_blobIceTrail;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_retreat.SetData(m_skeletonDataAsset);
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

        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_iceTrailSensor;

        [SerializeField, TabGroup("Magister")]
        private Transform m_magister;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        [SerializeField]
        private Renderer m_renderer;

        private float m_currentPatience;
        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_enablePatience;
        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;

        private Coroutine m_patienceRoutine;

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
            {
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_stateHandle.Wait(State.ReevaluateSituation);
                StopAllCoroutines();
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
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
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_info.blobIceTrail, gameObject.scene);

            if (!m_iceTrailSensor.isDetecting)
            {
                instance.transform.position = m_iceTrailSensor.transform.position;
            }
        }

        private void CalculateRunPath()
        {
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -3 : 3), m_targetInfo.position.y + 10);
            while (Vector2.Distance(transform.position, WallPosition()) <= 5)
            {
                movePos = new Vector2(movePos.x + 0.1f, movePos.y);
                break;
            }
            m_agent.SetDestination(movePos);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            //m_animation.SetEmptyAnimation(0, 0);
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            m_movement.Stop();
            m_selfCollider.enabled = false;
            Debug.Log("P Blob destroyed");
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_hitbox.Disable();
            m_selfCollider.enabled = false;
            //m_animation.SetAnimation(0, m_info.deathAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //m_animation.SetAnimation(0, m_info.disassembledIdleAnimation, true);
            yield return new WaitForSeconds(m_info.deathDuration);
            //m_health.SetHealthPercentage(1f);
            //enabled = true;
            //m_animation.SetAnimation(0, m_info.recoverAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.recoverAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.OverrideState(State.Patrol);
            InstantiateBlobIceCloud(transform.position);
            gameObject.SetActive(false);
            yield return null;
        }

        #region Movement
        private void DynamicMovement(Vector2 target, float moveSpeed)
        {
            m_agent.SetDestination(target);

            if (/*m_wallSensor.allRaysDetecting ||*/ m_selfSensor.isDetecting)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                //m_bodyCollider.SetActive(true);
                m_agent.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                return;
            }

            if (IsFacing(m_agent.hasPath && TargetBlocked() && !m_groundSensor.allRaysDetecting && !m_roofSensor.allRaysDetecting ? m_agent.segmentDestination : target))
            {
                if (m_animation.animationState.GetCurrent(0).IsComplete)
                {
                    var chosenMoveAnim = UnityEngine.Random.Range(0, 50) > 10 ? m_info.idleAnimation : m_info.move.animation;
                    m_animation.SetAnimation(0, chosenMoveAnim, true);
                }

                if (!m_wallSensor.allRaysDetecting && (m_groundSensor.allRaysDetecting || m_roofSensor.allRaysDetecting))
                {
                    //if (m_executeMoveCoroutine != null)
                    //{
                    //    StopCoroutine(m_executeMoveCoroutine);
                    //    m_executeMoveCoroutine = null;
                    //}
                    //StartCoroutine(DespawnRoutine());
                    //Vector3.MoveTowards(transform.position, m_targetInfo.position, m_info.move.speed);
                    m_bodyCollider.enabled = false;
                    m_agent.Stop();
                    Vector3 dir = (target - (Vector2)m_rigidbody2D.transform.position).normalized;
                    m_rigidbody2D.MovePosition(m_rigidbody2D.transform.position + dir * moveSpeed * Time.fixedDeltaTime);

                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    return;
                }

                m_bodyCollider.enabled = true;
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                m_agent.SetDestination(target);
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_turnState = State.ReevaluateSituation;
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.move.speed, characterInfo);
                    InstantiateBlobIceTrail();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    m_agent.Stop();
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

        #endregion

        public override void ReturnToSpawnPoint()
        {
            throw new NotImplementedException();
        }

        protected override void OnForbidFromAttackTarget()
        {
            throw new NotImplementedException();
        }

        protected override void OnTargetDisappeared()
        {
            throw new NotImplementedException();
        }
    }
}


