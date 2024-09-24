using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using Holysoft.Event;
using DChild.Gameplay.Combat;
using Spine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ImpaledCharger_PusherAI : CombatAIBrain<ImpaledCharger_PusherAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, Min(0)]
            private float m_patience;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            [SerializeField]
            private MovementInfo m_chaseInfo;
            [SerializeField]
            private SimpleAttackInfo m_clawSlashAttack;


            public float patience => m_patience;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            public MovementInfo chaseInfo => m_chaseInfo;
            public SimpleAttackInfo clawSlashAttack => m_clawSlashAttack;

            public override void Initialize()
            {
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_chaseInfo.SetData(m_skeletonDataAsset);
                m_clawSlashAttack.SetData(m_skeletonDataAsset);
            }
        }

        private enum State
        {
            Idle,
            Chase,
            Attacking,
            WaitForBehaviour
        }

        public enum Mode
        {
            Alone,
            Manipulated
        }

        [SerializeField]
        private MovementHandle2D m_movementHandle;
        [SerializeField]
        private TurnHandle m_turn;
        [SerializeField]
        private FlinchHandler m_flinch;
        [SerializeField]
        private RaySensorCaster m_sensors;
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_edgeSensor;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private float m_patienceTimer;

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnTargetDisappeared()
        {
        }

        #region ImpaleCharger Only Functions
        public void BeAnnoyed()
        {
            StartCoroutine(DetectTargetRoutine());
        }

        public TrackEntry SetAnimation(int index, IAIAnimationInfo animationInfo, bool loop)
        {
            return AIBrainUtility.SetAnimation(m_animation, index, animationInfo, loop);
        }

        public void SetHitboxInvulnerability(Invulnerability level)
        {
            var hitbox = m_damageable.GetHitboxes();
            for (int i = 0; i < hitbox.Length; i++)
            {
                hitbox[i].SetInvulnerability(level);
            }
        }

        public void EnableHitbox(bool enabled)
        {
            var hitbox = m_damageable.GetHitboxes();
            for (int i = 0; i < hitbox.Length; i++)
            {
                if (enabled)
                {
                    hitbox[i].Enable();
                }
                else
                {
                    hitbox[i].Disable();
                }
            }
        }

        public void TurnTowards(Vector2 target)
        {
            if (IsFacing(target) == false)
            {
                m_turn.ForceTurnImmidiately();
            }
        }
        #endregion

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable == null)
            {
                if (m_targetInfo.doesTargetExist)
                {
                    m_patienceTimer = m_info.patience;
                }
            }
            else
            {
                base.SetTarget(damageable, m_target);
                m_patienceTimer = -1;
                if (m_stateHandle.currentState == State.Idle)
                {
                    StopAllCoroutines();
                    StartCoroutine(DetectTargetRoutine());
                }
            }
        }

        public void SetMode(Mode mode)
        {
            StopAllCoroutines();
            switch (mode)
            {
                case Mode.Alone:
                    enabled = true;
                    m_aggroBoundary.gameObject.SetActive(true);
                    m_sensors.enabled = true;
                    m_character.physics.Enable();
                    m_character.colliders.Enable();
                    if (m_targetInfo.doesTargetExist)
                    {
                        m_stateHandle.OverrideState(State.Chase);
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Idle);
                    }
                    break;
                case Mode.Manipulated:
                    enabled = false;
                    m_aggroBoundary.gameObject.SetActive(false);
                    m_sensors.enabled = false;
                    m_character.physics.Disable();
                    m_character.colliders.Disable();
                    break;
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
            if (m_targetInfo.doesTargetExist)
            {
                // m_stateHandle.OverrideState(State.Struggle);
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState == State.Idle)
            {
                m_stateHandle.Wait(State.Idle);
            }
            else
            {
                StopAllCoroutines();
                //m_stateHandle.Wait(State.Struggle);
            }
        }

        private IEnumerator DetectTargetRoutine()
        {
            m_stateHandle.Wait(State.Chase);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.detectAnimation, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ClawSlashAttack()
        {
            m_stateHandle.Wait(State.Chase);
            m_movementHandle.Stop();
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.clawSlashAttack, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_stateHandle.ApplyQueuedState();
        }

        private void HandlePatience()
        {
            if (m_patienceTimer > 0)
            {
                m_patienceTimer -= GameplaySystem.time.deltaTime;
                if (m_patienceTimer <= 0)
                {
                    if (m_stateHandle.currentState == State.WaitForBehaviour)
                    {
                        m_stateHandle.QueueState(State.Idle);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitForBehaviour);
            m_flinch.FlinchStart += OnFlinchStart;
            m_flinch.FlinchEnd += OnFlinchEnd;
        }

        private void Update()
        {
            HandlePatience();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    AIBrainUtility.SetAnimation(m_animation, 0, m_info.idleAnimation, true);
                    break;
                case State.Chase:
                    var distanceToTarget = Vector3.Distance(m_targetInfo.position, m_character.centerMass.position);
                    if (distanceToTarget <= m_info.clawSlashAttack.range)
                    {
                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        if (IsFacingTarget() == false)
                        {
                            m_turn.ForceTurnImmidiately();
                        }

                        var canMoveForward = m_wallSensor.isDetecting == false && m_edgeSensor.isDetecting == true;
                        if (canMoveForward)
                        {
                            AIBrainUtility.SetAnimation(m_animation, 0, m_info.chaseInfo, true);
                            m_movementHandle.MoveTowards((int)m_character.facing * Vector2.right, m_info.chaseInfo.speed);
                        }
                        else
                        {
                            AIBrainUtility.SetAnimation(m_animation, 0, m_info.idleAnimation, true);
                            m_movementHandle.Stop();
                        }
                    }
                    break;
                case State.Attacking:
                    StopAllCoroutines();
                    StartCoroutine(ClawSlashAttack());
                    break;
                case State.WaitForBehaviour:
                    break;
            }
        }
    }
}
