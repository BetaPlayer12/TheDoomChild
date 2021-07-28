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
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/AlchemistBot")]
    public class AlchemistBotAI : CombatAIBrain<AlchemistBotAI.Info>, IResetableAIBrain, IAmbushingAI
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private float m_attackBodyLightningDuration;
            public float attackBodyLightningDuration => m_attackBodyLightningDuration;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [SerializeField]
            private float m_detectionTime;
            public float detectionTime => m_detectionTime;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallLoopAnimation;
            public string deathFallLoopAnimation => m_deathFallLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact1Animation;
            public string deathFallImpact1Animation => m_deathFallImpact1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact2Animation;
            public string deathFallImpact2Animation => m_deathFallImpact2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_awakenAnimation;
            public string awakenAnimation => m_awakenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_dormantAnimation;
            public string dormantAnimation => m_dormantAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_prepAnimation;
            public string prepAnimation => m_prepAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spawnBlobEvent;
            public string spawnBlobEvent => m_spawnBlobEvent;

            [SerializeField]
            private GameObject m_blobAcid;
            public GameObject blobAcid => m_blobAcid;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Detect,
            Patrol,
            Idle,
            Cooldown,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_attackBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_attackSideBB;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_playerSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_bodylightningFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_glowFX;
        [SerializeField, TabGroup("BB")]
        private Collider2D m_bodylightningBB;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_chosenAttack;

        [SerializeField, SpineBone, TabGroup("Bone")]
        private string m_boneName;
        [SerializeField, TabGroup("Bone")]
        private Bone m_bone;
        private Vector2 m_boneDefaultPos;

        [SerializeField]
        private bool m_willPatrol;

        private float m_currentCD;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private bool m_canAttack2;
        private Coroutine m_bodylightningCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //transform.localScale = new Vector3(m_chosenAttack == Attack.Attack2 ? -transform.localScale.x : transform.localScale.x, 1, 1);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
                m_enablePatience = false;
            }
            else
            {
                //if (!m_enablePatience)
                //{
                //    m_enablePatience = true;
                //    //Patience();
                //    StartCoroutine(PatienceRoutine());
                //}
                m_enablePatience = true;
                //StartCoroutine(PatienceRoutine());
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            StopAllCoroutines();
            m_stateHandle.Wait(State.Cooldown);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (!m_bodylightningBB.enabled)
            {
                m_bodylightningCoroutine = StartCoroutine(BodyLightningRoutine());
            }
            m_stateHandle.ApplyQueuedState();
        }

        private void SpawnBlob()
        {
            var blob = Instantiate(m_info.blobAcid, transform.position, Quaternion.identity);
            blob.GetComponent<BlobAcidAI>().SetTargetInfo(m_targetInfo);
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.right * transform.localScale.x, 1000, LayerMask.GetMask("Environment"));
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_targetInfo.Set(null, null);
                m_enablePatience = false;
                m_isDetecting = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range)
                                  /*, new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            if (!m_willPatrol)
            {
                m_animation.EnableRootMotion(true, true);
                m_animation.SetAnimation(0, m_info.awakenAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
                //m_animation.SetAnimation(0, m_info.prepAnimation, false).MixDuration = 0;

                m_animation.AddAnimation(0, m_info.prepAnimation, false, 0);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.prepAnimation);
                m_animation.DisableRootMotion();
            }
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(m_info.detectionTime);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            m_animation.EnableRootMotion(true, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            yield return new WaitForSeconds(1.6f);
            //m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathFallLoopAnimation, true);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            var animation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.deathFallImpact1Animation : m_info.deathFallImpact2Animation;
            m_bodyCollider.SetActive(true);
            m_animation.SetAnimation(0, animation, false);
            yield return null;
        }

        private IEnumerator AttackBBSize()
        {
            yield return new WaitForSeconds(.35f);
            if (m_chosenAttack == Attack.Attack1)
            {
                var distance = Vector2.Distance(m_projectilePoint.position, new Vector2(GroundPosition().x, GroundPosition().y * .95f));
                m_attackBB.enabled = true;
                m_attackBB.offset = new Vector2(0, (-distance * 2) * .15f);
                m_attackBB.size = new Vector2(.25f, distance);
            }
            else
            {
                m_attackSideBB.enabled = true;
            }
            yield return new WaitForSeconds(.75f);
            m_attackBB.enabled = false;
            m_attackSideBB.enabled = false;
            m_attackBB.offset = Vector2.zero;
            m_attackBB.size = new Vector2(.25f, .25f);
            yield return null;
        }

        private IEnumerator Attack2CooldownRoutine()
        {
            m_canAttack2 = false;
            yield return new WaitForSeconds(3);
            m_canAttack2 = true;
            yield return null;
        }

        private IEnumerator BodyLightningRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            m_bodylightningFX.Play();
            m_glowFX.Play();
            m_bodylightningBB.enabled = true;
            yield return new WaitForSeconds(m_info.attackBodyLightningDuration);
            m_bodylightningFX.Stop();
            m_glowFX.Stop();
            m_bodylightningBB.enabled = false;
            yield return null;
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

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
            m_hitbox.Disable();
            m_attackBB.enabled = false;
            m_attackSideBB.enabled = false;
            m_attackBB.offset = Vector2.zero;
            m_attackBB.size = new Vector2(.25f, .25f);
            m_bodylightningBB.enabled = false;
            m_bodylightningFX.Stop();
            m_glowFX.Stop();
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }

        private void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            //Debug.Log("FKING AIM");
            if (m_targetInfo.isValid)
            {
                var targetPos = new Vector2(GroundPosition().x, GroundPosition().y * .925f);
                var localPositon = transform.InverseTransformPoint(targetPos);
                localPositon = new Vector2(-localPositon.x, localPositon.y);
                m_bone.SetLocalPosition(localPositon);
            }
        }

        protected override void Start()
        {
            base.Start();

            if (!m_willPatrol)
            {
                m_hitbox.Disable();
            }
            //m_selfCollider.SetActive(false);
            m_spineEventListener.Subscribe(m_info.spawnBlobEvent, SpawnBlob);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathFallImpact1Animation);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_bone = m_animation.skeletonAnimation.Skeleton.FindBone(m_boneName);
            m_boneDefaultPos = m_bone.GetLocalPosition();
            m_animation.skeletonAnimation.UpdateLocal += SkeletonAnimation_UpdateLocal;

            m_canAttack2 = true;
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();

                    if (!IsFacingTarget() && m_willPatrol)
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.dormantAnimation, true);
                    m_agent.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    //m_animation.EnableRootMotion(true, true);
                    StartCoroutine(AttackBBSize());
                    switch (m_chosenAttack)
                    {
                        case Attack.Attack1:
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack2:
                            transform.localScale = new Vector3(m_targetInfo.position.x > transform.position.x ? -1 : 1, 1, 1);
                            StartCoroutine(Attack2CooldownRoutine());
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                        }
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        var targetPos = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 10);
                        m_attackDecider.DecideOnAttack();
                        if (m_attackDecider.hasDecidedOnAttack && IsInRange(targetPos, m_attackDecider.chosenAttack.range) /*IsTargetInRange(m_attackDecider.chosenAttack.range)*/)
                        {
                            m_chosenAttack = m_attackDecider.chosenAttack.attack;
                            m_animation.EnableRootMotion(true, true);
                            m_agent.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            if (!m_selfSensor.isDetecting)
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                                m_agent.SetDestination(targetPos);
                                m_agent.Move(m_info.move.speed);

                                //if (m_canAttack2 && m_playerSensor.isDetecting)
                                //{
                                //    m_chosenAttack = Attack.Attack2;
                                //    m_animation.EnableRootMotion(true, true);
                                //    m_agent.Stop();
                                //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                //    m_stateHandle.SetState(State.Attacking);
                                //}
                            }
                            else
                            {
                                m_agent.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_hitbox.Enable();
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }

        public void LaunchAmbush(Vector2 position)
        {
            enabled = true;
            m_stateHandle.OverrideState(State.Detect);
        }

        public void PrepareAmbush(Vector2 position)
        {
            enabled = false;
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Idle);
        }
    }
}

