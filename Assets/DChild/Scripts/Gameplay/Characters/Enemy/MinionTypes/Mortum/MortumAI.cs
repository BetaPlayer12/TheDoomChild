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
using DChild;
using DChild.Gameplay.Characters.Enemies;
using System.Security.Cryptography;

namespace DChild.Gameplay.Characters.Enemies
{

    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Mortum")]
    public class MortumAI : CombatAIBrain<MortumAI.Info>
    {
        public class Info : BaseInfo
        {

            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            [SerializeField, FoldoutGroup("Kneeling State"), Min(0)]
            private float m_kneelDuration;
            [SerializeField, FoldoutGroup("Kneeling State")]
            private BasicAnimationInfo m_kneelingAnimation;
            [SerializeField, FoldoutGroup("Kneeling State")]
            private BasicAnimationInfo m_standingAnimation;

            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            [SerializeField,]
            private BasicAnimationInfo m_turnAnimation;

            [SerializeField, FoldoutGroup("Melee Attacks")]
            private BasicAnimationInfo m_backHandAttack;
            [SerializeField, FoldoutGroup("Melee Attacks")]
            private BasicAnimationInfo m_armBashAttack;
            [SerializeField, FoldoutGroup("Melee Attacks"), Min(0)]
            private float m_meleeAttackRange;

            [SerializeField, FoldoutGroup("Eye Laser Attack")]
            private SimpleAttackInfo m_eyeLaserAttack;
            [SerializeField, FoldoutGroup("Eye Laser Attack")]
            private float m_eyeLaserMaxDistance;
            [SerializeField, FoldoutGroup("Eye Laser Attack"), ValueDropdown("GetEvents")]
            private string m_eyeLaserOnEvent;
            [SerializeField, FoldoutGroup("Eye Laser Attack"), ValueDropdown("GetEvents")]
            private string m_eyeLaserOffEvent;
            [SerializeField, FoldoutGroup("Eye Laser Attack")]
            private float m_eyelaserMinRange;
            [SerializeField, FoldoutGroup("Eye Laser Attack")]
            private float m_eyelaserMaxRange;

            [SerializeField, Min(0)]
            private float m_patienceDuration;

            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            public float kneelDuration => m_kneelDuration;
            public BasicAnimationInfo kneelingAnimation => m_kneelingAnimation;
            public BasicAnimationInfo standingAnimation => m_standingAnimation;
            public MovementInfo patrol => m_patrol;
            public MovementInfo move => m_move;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            public BasicAnimationInfo backHandAttack => m_backHandAttack;
            public BasicAnimationInfo armBashAttack => m_armBashAttack;
            public float meleeAttackRange => m_meleeAttackRange;
            public SimpleAttackInfo eyeLaserAttack => m_eyeLaserAttack;
            public float eyeLaserMaxDistance => m_eyeLaserMaxDistance;
            public string eyeLaserOnEvent => m_eyeLaserOnEvent;
            public string eyeLaserOffEvent => m_eyeLaserOffEvent;
            public float eyelaserMinRange => m_eyelaserMinRange;
            public float eyelaserMaxRange => m_eyelaserMaxRange;

            public float patienceDuration => m_patienceDuration;

            public override void Initialize()
            {
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_kneelingAnimation.SetData(m_skeletonDataAsset);
                m_standingAnimation.SetData(m_skeletonDataAsset);
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_backHandAttack.SetData(m_skeletonDataAsset);
                m_armBashAttack.SetData(m_skeletonDataAsset);
                m_eyeLaserAttack.SetData(m_skeletonDataAsset);
            }
        }

        [System.Serializable]
        public class BoundingBoxConfiguration
        {

            [SerializeField]
            private GameObject m_instance;
            [SerializeField, Min(0)]
            private float m_spawnDelay;
            [SerializeField, Min(0)]
            private float m_duration;

            public GameObject instance => m_instance;
            public float spawnDelay => m_spawnDelay;
            public float duration => m_duration;
        }

        public enum State
        {
            Idle,
            Kneeling,
            Patrol,
            Chase,
            Attack,
            ReevaluateSituation,
            WaitForBehaviour,
            TEST
        }

        public enum Attack
        {
            BackHand,
            ArmBash,
            EyeLaser
        }

        [SerializeField]
        private Damageable m_bodyEntity;

        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private MortumEyeLaser m_eyelaser;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("BoundingBox")]
        private BoundingBoxConfiguration m_backHandBB;
        [SerializeField, TabGroup("BoundingBox")]
        private BoundingBoxConfiguration m_armBashBB;

        [SerializeField]
        private ParticleSystem m_eyeLaserMuzzleFX;


        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        private StateHandle<State> m_state;
        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        private RandomAttackDecider<Attack> m_attackDecider;

        private float m_patienceTimer;

        public override void ReturnToSpawnPoint()
        {

        }

        protected override void OnTargetDisappeared()
        {

        }

        private void OnBodyDead(object sender, EventActionArgs eventArgs)
        {
            if (m_damageable.isAlive)
            {
                StopAllCoroutines();
                m_eyelaser.Disable();
                StartCoroutine(KneelingRoutine());
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_eyelaser.Disable();
            base.OnDestroyed(sender, eventArgs);
        }



        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                if (m_targetInfo.doesTargetExist == false)
                {
                    StopAllCoroutines();
                    StartCoroutine(DetectTargetRoutine());
                }
                base.SetTarget(damageable, m_target);
                m_patienceTimer = -1;
            }
            else
            {
                m_patienceTimer = m_info.patienceDuration;
            }
        }

        private IEnumerator DetectTargetRoutine()
        {
            m_state.Wait(State.Chase);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.detectAnimation, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_state.ApplyQueuedState();
        }

        private IEnumerator KneelingRoutine()
        {
            m_state.OverrideState(State.Kneeling);
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.kneelingAnimation, false);
            yield return new WaitForSeconds(m_info.kneelDuration);
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.standingAnimation, false);
            AIBrainUtility.AddAnimation(m_animation, 0, m_info.idleAnimation, true, 0);
            GameplaySystem.combatManager.Heal(m_bodyEntity, 9999999);
            m_state.SetState(State.ReevaluateSituation);
        }

        private IEnumerator BackHandAttackRoutine()
        {
            m_state.Wait(State.ReevaluateSituation);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.backHandAttack, false);
            yield return WaitForBoundingBox(m_backHandBB);
            yield return new WaitForSpineAnimationComplete(track);
            AIBrainUtility.AddAnimation(m_animation, 0, m_info.idleAnimation, true, 0);

            m_attackDecider.hasDecidedOnAttack = false;
            m_state.ApplyQueuedState();
        }

        private IEnumerator ArmBashAttackRoutine()
        {
            m_state.Wait(State.ReevaluateSituation);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.armBashAttack, false);
            yield return WaitForBoundingBox(m_armBashBB);
            yield return new WaitForSpineAnimationComplete(track);
            AIBrainUtility.AddAnimation(m_animation, 0, m_info.idleAnimation, true, 0);

            m_attackDecider.hasDecidedOnAttack = false;
            m_state.ApplyQueuedState();
        }

        private IEnumerator WaitForBoundingBox(BoundingBoxConfiguration configuration)
        {
            yield return new WaitForSeconds(configuration.spawnDelay);
            configuration.instance.SetActive(true);
            yield return new WaitForSeconds(configuration.duration);
            configuration.instance.SetActive(false);
        }

        private IEnumerator EyeLaserAttackRoutine()
        {
            m_state.Wait(State.ReevaluateSituation);
            m_flinchHandle.m_enableModule = false;
            m_animation.EnableRootMotion(true, false);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.eyeLaserAttack, false);
            m_eyeLaserMuzzleFX.Play();
            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_info.eyeLaserOnEvent);
            m_eyelaser.SetMaxDistance(m_info.eyeLaserMaxDistance);
            m_eyelaser.Enable();
            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_info.eyeLaserOffEvent);
            m_eyelaser.Disable();
            yield return new WaitForSpineAnimationComplete(track);
            var idleTrack = AIBrainUtility.AddAnimation(m_animation, 0, m_info.idleAnimation, true, 0);
            yield return new WaitForSpineAnimationComplete(idleTrack);
            m_animation.DisableRootMotion();

            m_flinchHandle.m_enableModule = true;
            m_attackDecider.hasDecidedOnAttack = false;
            m_state.ApplyQueuedState();
        }

        private IEnumerator TurnRoutine(State afterTurnState)
        {
            m_state.Wait(afterTurnState);
            m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
            m_turnHandle.TurnDone += OnTurnDone;
            bool isTurnDone = false;
            while (isTurnDone == false)
                yield return null;


            m_state.ApplyQueuedState();

            void OnTurnDone(object sender, FacingEventArgs eventArgs)
            {
                isTurnDone = true;
            }
        }

        private void HandlePatience()
        {
            if (m_patienceTimer > 0)
            {
                m_patienceTimer -= GameplaySystem.time.deltaTime;
                if (m_patienceTimer <= 0)
                {
                    m_patienceTimer = 0;
                }
            }

            //Safeguard for when patience runs out in the middle of a behaviour
            if (m_patienceTimer == 0)
            {
                if (m_state.currentState != State.WaitForBehaviour)
                {
                    base.SetTarget(null, null);
                    m_state.SetState(State.Patrol);
                    m_patienceTimer = -1;
                }
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            switch (m_state.currentState)
            {
                case State.Patrol:
                    StartCoroutine(TurnRoutine(State.Patrol));
                    break;
                default:
                    StartCoroutine(TurnRoutine(State.ReevaluateSituation));
                    break;
            }
        }

        private float GetDistanceToTarget()
        {
            return Mathf.Abs(m_targetInfo.position.x - m_centerMass.position.x);
        }

        protected override void Awake()
        {
            base.Awake();
            m_state = new StateHandle<State>(State.Patrol, State.WaitForBehaviour);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.BackHand, m_info.meleeAttackRange),
                                    new AttackInfo<Attack>(Attack.ArmBash, m_info.meleeAttackRange));

            m_eyelaser.Disable();

            m_backHandBB.instance.SetActive(false);
            m_armBashBB.instance.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            m_bodyEntity.Destroyed += OnBodyDead;
            m_patrolHandle.TurnRequest += OnTurnRequest;
        }

        private void Update()
        {
            HandlePatience();
            switch (m_state.currentState)
            {
                case State.Idle:
                    AIBrainUtility.SetAnimation(m_animation, 0, m_info.idleAnimation, true);
                    break;
                case State.Kneeling:
                    //Will Be Handled By A Coroutine
                    break;
                case State.Patrol:
                    m_animation.EnableRootMotion(false, false);
                    AIBrainUtility.SetAnimation(m_animation, 0, m_info.patrol, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    break;
                case State.Chase:

                    if (IsFacingTarget())
                    {
                        var distanceToTarget = GetDistanceToTarget();
                        if (distanceToTarget <= m_info.meleeAttackRange)
                        {
                            m_state.SetState(State.Attack);
                            return;
                        }
                        else if (distanceToTarget >= m_info.eyelaserMinRange && distanceToTarget <= m_info.eyeLaserMaxDistance)
                        {
                            m_state.SetState(State.Attack);
                            m_attackDecider.DecideOnAttack(Attack.EyeLaser);
                        }
                        else
                        {
                            m_animation.EnableRootMotion(false, false);
                            AIBrainUtility.SetAnimation(m_animation, 0, m_info.move, true);
                            m_movement.MoveTowards(DirectionToTarget(), m_info.move.speed);
                        }
                    }
                    else
                    {
                        StartCoroutine(TurnRoutine(State.Chase));
                    }
                    break;
                case State.Attack:
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }

                    StopAllCoroutines();
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.BackHand:
                            StartCoroutine(BackHandAttackRoutine());
                            break;
                        case Attack.ArmBash:
                            StartCoroutine(ArmBashAttackRoutine());
                            break;
                        case Attack.EyeLaser:
                            StartCoroutine(EyeLaserAttackRoutine());
                            break;
                    }

                    break;
                case State.ReevaluateSituation:
                    if (m_targetInfo.doesTargetExist)
                    {
                        m_state.SetState(State.Chase);
                    }
                    else
                    {
                        m_state.SetState(State.Patrol);
                    }
                    break;
                case State.WaitForBehaviour:
                    break;
                case State.TEST:
                    ForceDoAttack(m_forcedAttack);
                    break;
            }
        }

        protected override void LateUpdate()
        {
            //    base.LateUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            if (m_info != null)
            {
                var origin = (Vector2)m_centerMass.position;
                var direction = m_character.facing == HorizontalDirection.Right ? Vector2.right : Vector2.left;

                var meleeRange = origin + (direction * m_info.meleeAttackRange);
                Gizmos.DrawLine(origin, meleeRange);

                var eyeLazerMin = origin + (direction * m_info.eyelaserMinRange);
                var eyeLazerMax = origin + (direction * m_info.eyelaserMinRange);
                Gizmos.DrawLine(eyeLazerMin, eyeLazerMax);
            }
        }

        private Attack m_forcedAttack;
        [Button]
        private void ForceDoAttack(Attack attack)
        {
            StopAllCoroutines();
            m_forcedAttack = attack;
            switch (attack)
            {
                case Attack.BackHand:
                    StartCoroutine(BackHandAttackRoutine());
                    break;
                case Attack.ArmBash:
                    StartCoroutine(ArmBashAttackRoutine());
                    break;
                case Attack.EyeLaser:
                    StartCoroutine(EyeLaserAttackRoutine());
                    break;
            }
            m_state.QueueState(State.TEST);
        }
    }
}

