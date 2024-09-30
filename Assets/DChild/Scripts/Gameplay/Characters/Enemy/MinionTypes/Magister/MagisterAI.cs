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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Magister")]
    public class MagisterAI : CombatAIBrain<MagisterAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_moveWithoutBook = new MovementInfo();
            public MovementInfo moveWithoutBook => m_moveWithoutBook;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_moveWithBook = new MovementInfo();
            public MovementInfo moveWithBook => m_moveWithBook;

            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attackCastingSpell = new SimpleAttackInfo();
            public SimpleAttackInfo attackCastingSpell => m_attackCastingSpell;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attackConjureBooks = new SimpleAttackInfo();
            public SimpleAttackInfo attackConjureBooks => m_attackConjureBooks;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_fleeDistance;
            public float fleeDistance => m_fleeDistance;


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleWithoutBookAnimation;
            public BasicAnimationInfo idleWithoutBookAnimation => m_idleWithoutBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleWithBookAnimation;
            public BasicAnimationInfo idleWithBookAnimation => m_idleWithBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchWithoutBookAnimation;
            public BasicAnimationInfo flinchWithoutBookAnimation => m_flinchWithoutBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchWithBookAnimation;
            public BasicAnimationInfo flinchWithBookAnimation => m_flinchWithBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportFromSideAnimation;
            public BasicAnimationInfo teleportFromSideAnimation => m_teleportFromSideAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportFromBelowAnimation;
            public BasicAnimationInfo teleportFromBelowAnimation => m_teleportFromBelowAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportDisappearAnimation;
            public BasicAnimationInfo teleportDisappearAnimation => m_teleportDisappearAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnWithoutBookAnimation;
            public BasicAnimationInfo turnWithoutBookAnimation => m_turnWithoutBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnWithBookAnimation;
            public BasicAnimationInfo turnWithBookAnimation => m_turnWithBookAnimation;
            [SerializeField]
            private BasicAnimationInfo m_bookSummonAnimation;
            public BasicAnimationInfo bookSummonAnimation => m_bookSummonAnimation;
            [SerializeField]
            private BasicAnimationInfo m_healAnimation;
            public BasicAnimationInfo healAnimation => m_healAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_summonTotemEvent;
            public string summonTotemEvent => m_summonTotemEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveWithoutBook.SetData(m_skeletonDataAsset);
                m_moveWithBook.SetData(m_skeletonDataAsset);
                m_attackCastingSpell.SetData(m_skeletonDataAsset);
                m_attackConjureBooks.SetData(m_skeletonDataAsset);

                m_idleWithoutBookAnimation.SetData(m_skeletonDataAsset);
                m_idleWithBookAnimation.SetData(m_skeletonDataAsset);
                m_flinchWithoutBookAnimation.SetData(m_skeletonDataAsset);
                m_flinchWithBookAnimation.SetData(m_skeletonDataAsset);
                m_teleportFromSideAnimation.SetData(m_skeletonDataAsset);
                m_teleportFromBelowAnimation.SetData(m_skeletonDataAsset);
                m_teleportDisappearAnimation.SetData(m_skeletonDataAsset);
                m_turnWithoutBookAnimation.SetData(m_skeletonDataAsset);
                m_turnWithBookAnimation.SetData(m_skeletonDataAsset);
                m_bookSummonAnimation.SetData(m_skeletonDataAsset);
                m_healAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Patrol,
            Standby,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            Fleeing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum BookState
        {
            WithoutBook,
            WithBook
        }

        private enum Attack
        {
            Attack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private int m_HealValue;
        [SerializeField, TabGroup("Reference")]
        private float m_summonCooldown;
        [SerializeField, TabGroup("Reference")]
        private ParticleSystem m_TeleportVFX;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_shadow;
        //[SerializeField, TabGroup("Reference")]
        //private float m_TeleportCooldown;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        private bool m_HasHealed;
        private float m_healthHealThreshold;
        private float m_summonCooldownDuration=0f;
        //private float m_teleportCooldownDuration = 0f;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_backSensor;
        [SerializeField, TabGroup("FlinchHandles")]
        private FlinchHandler m_flinchHandleWithoutBook;
        [SerializeField, TabGroup("FlinchHandles")]
        private FlinchHandler m_flinchHandleWithBook;
        [SerializeField, TabGroup("Totem")]
        private TomeOfSpellsIceAI m_tome;
        [SerializeField, TabGroup("Totem")]
        private TomeOfSpellsFlameAI m_tomeFire;
        [SerializeField, TabGroup("Totem")]
        private TomeOfSpellsStormAI m_tomeStorm;

        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        private BookState m_bookState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomIdleRoutine;
        private Coroutine m_randomTurnRoutine;
        private Coroutine m_teleportRoutine;

        private string m_currentIdleAnimation;
        private string m_currentMoveAnimation;
        private string m_currentTurnAnimation;
        private string m_currentFlinchAnimation;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null /*&& !ShotBlocked()*/)
            {
                base.SetTarget(damageable);
                //if (m_stateHandle.currentState != State.Chasing 
                //    && m_stateHandle.currentState != State.RunAway 
                //    && m_stateHandle.currentState != State.Turning 
                //    && m_stateHandle.currentState != State.WaitBehaviourEnd)
                //{
                //}
                m_selfCollider.enabled = false;

                if (!m_isDetecting)
                {
                    if (m_randomIdleRoutine != null)
                    {
                        StopCoroutine(m_randomIdleRoutine);
                        m_randomIdleRoutine = null;
                    }
                    if (m_randomTurnRoutine != null)
                    {
                        StopCoroutine(m_randomTurnRoutine);
                        m_randomTurnRoutine = null;
                    }
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //m_randomIdleRoutine = null;
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        //Patience Handler
        private void Patience()
        {
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }
            if (TargetBlocked())
            {
                if (IsFacingTarget())
                {
                    if (m_sneerRoutine == null)
                    {
                        m_sneerRoutine = StartCoroutine(SneerRoutine());
                    }
                    //else if ()
                    //{
                    //}
                }
                else
                {
                    if (m_sneerRoutine != null)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    //m_enablePatience = false;
                    m_turnState = State.WaitBehaviourEnd;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                        m_stateHandle.SetState(State.Turning);
                }
            }
            else if (!TargetBlocked())
            {
                if (m_sneerRoutine != null)
                {
                    if (m_patienceRoutine != null)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }
                    StopCoroutine(m_sneerRoutine);
                    m_sneerRoutine = null;
                    m_enablePatience = false;
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                }
            }
        }
        private IEnumerator PatienceRoutine()
        {
            //if (m_enablePatience)
            //{
            //    while (m_currentPatience < m_info.patience)
            //    {
            //        m_currentPatience += m_character.isolatedObject.deltaTime;
            //        yield return null;
            //    }
            //}
            yield return new WaitForSeconds(m_info.patience);
            m_selfCollider.enabled = false;
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.SetState(State.Patrol);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_currentIdleAnimation && m_teleportRoutine == null)
            {
                StopAllCoroutines();
                m_selfCollider.enabled = true;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_currentFlinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentFlinchAnimation);
            m_animation.SetAnimation(0, m_currentFlinchAnimation, true);
            if (m_teleportRoutine == null && !m_wallSensor.allRaysDetecting && !m_backSensor.allRaysDetecting&& m_targetInfo!=null)
            {
                //m_stateHandle.Wait(State.ReevaluateSituation);
                m_teleportRoutine = StartCoroutine(TeleportRoutine());
            }else
            {
                m_stateHandle.ApplyQueuedState();
            }
            yield return null;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attackCastingSpell.range)
                                  , new AttackInfo<Attack>(Attack.Attack, m_info.attackConjureBooks.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator RandomIdleRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }
        }

        private IEnumerator RandomTurnRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                m_turnState = State.Idle;
                m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        private void ApplyBookState(BookState currentBookState)
        {
            switch (currentBookState)
            {
                case BookState.WithoutBook:
                    m_bookState = BookState.WithoutBook;
                    m_currentIdleAnimation = m_info.idleWithoutBookAnimation.animation;
                    m_currentMoveAnimation = m_info.moveWithoutBook.animation;
                    m_currentMoveSpeed = UnityEngine.Random.Range(m_info.moveWithoutBook.speed * .75f, m_info.moveWithoutBook.speed * 1.25f);
                    m_currentTurnAnimation = m_info.turnWithoutBookAnimation.animation;
                    m_currentFlinchAnimation = m_info.flinchWithoutBookAnimation.animation;
                    break;
                case BookState.WithBook:
                    m_bookState = BookState.WithBook;
                    m_currentIdleAnimation = m_info.idleWithBookAnimation.animation;
                    m_currentMoveAnimation = m_info.moveWithBook.animation;
                    m_currentMoveSpeed = UnityEngine.Random.Range(m_info.moveWithBook.speed * .75f, m_info.moveWithBook.speed * 1.25f);
                    m_currentTurnAnimation = m_info.turnWithBookAnimation.animation;
                    m_currentFlinchAnimation = m_info.flinchWithBookAnimation.animation;
                    break;
            }
            m_flinchHandleWithoutBook.gameObject.SetActive(m_bookState == BookState.WithoutBook ? true : false);
            m_flinchHandleWithBook.gameObject.SetActive(m_bookState == BookState.WithBook ? true : false);
        }

        private void SummonTotem()
        {
            if (m_bookState == BookState.WithBook)
            {
                ApplyBookState(BookState.WithoutBook);
                int count = 0;
                do
                {
                    GameObject bookSpawned = null;
                    int x = UnityEngine.Random.Range(1, 4);
                    Debug.Log(x+" ??????????????????????????????");
                    switch (x)
                    {
                        case 1:
                            if (!m_tome.gameObject.activeSelf)
                            {
                                m_tome.SummonTome(m_targetInfo);
                                bookSpawned = m_tome.gameObject;
                            }
                            else
                            {
                                if (!m_tomeFire.gameObject.activeSelf)
                                {
                                    m_tomeFire.SummonTome(m_targetInfo);
                                    bookSpawned = m_tomeFire.gameObject;
                                }
                                else
                                {
                                    m_tomeStorm.SummonTome(m_targetInfo);
                                    bookSpawned = m_tomeStorm.gameObject;
                                }
                            }
                            break;

                        case 2:
                            if (!m_tomeFire.gameObject.activeSelf)
                            {
                                m_tomeFire.SummonTome(m_targetInfo);
                                bookSpawned = m_tomeFire.gameObject;
                            }
                            else
                            {
                                if (!m_tomeStorm.gameObject.activeSelf)
                                {
                                    m_tomeStorm.SummonTome(m_targetInfo);
                                    bookSpawned = m_tomeStorm.gameObject;
                                }
                                else
                                {
                                    m_tome.SummonTome(m_targetInfo);
                                    bookSpawned = m_tome.gameObject;
                                }
                            }
                            break;

                        case 3:
                            if (!m_tomeStorm.gameObject.activeSelf)
                            {
                                m_tomeStorm.SummonTome(m_targetInfo);
                                bookSpawned = m_tomeStorm.gameObject;
                            }
                            else
                            {
                                if (!m_tome.gameObject.activeSelf)
                                {
                                    m_tome.SummonTome(m_targetInfo);
                                    bookSpawned = m_tome.gameObject;
                                }
                                else
                                {
                                    m_tomeFire.SummonTome(m_targetInfo);
                                    bookSpawned = m_tomeFire.gameObject;
                                }
                            }
                            break;
                    }
                    count++;
                    bookSpawned.transform.position += (count == 1 ? new Vector3(10f, 0f) : new Vector3(-10f, 0f));
                } while (count < 2);

                
                
            }
        }

        private Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }

        private void Move()
        {
            m_animation.EnableRootMotion(false, false);
            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
            {
                m_selfCollider.enabled = false;
                m_animation.SetAnimation(0, m_currentMoveAnimation, true).TimeScale = 2f;
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                    m_movement.Stop();

                m_selfCollider.enabled = true;
                if (m_animation.animationState.GetCurrent(0).IsComplete)
                {
                    m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                }
            }
        }

        private void TeleportFleeing()
        {
            

            if (IsTargetInRange(m_info.targetDistanceTolerance) && !m_wallSensor.allRaysDetecting && !m_backSensor.allRaysDetecting)
            {
                if (IsTargetInRange(m_info.fleeDistance))
                {
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_teleportRoutine = StartCoroutine(TeleportRoutine());
                }
                else
                {
                    if (IsFacingTarget())
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                            m_movement.Stop();

                        m_selfCollider.enabled = true;
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                }

            }
            else
            {
                if (IsFacingTarget())
                {
                    Move();
                }
                else
                {
                    if (!m_backSensor.allRaysDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                }
            }
        }

        private IEnumerator TeleportRoutine()
        {
            m_TeleportVFX.Play();
            m_shadow.SetActive(false);
            m_animation.SetAnimation(0, m_info.teleportDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportDisappearAnimation);
            m_hitbox.Disable();
            m_movement.Stop();
            m_character.physics.simulateGravity = false;
            m_bodyCollider.enabled = false;
            m_legCollider.enabled = false;
            yield return new WaitForSeconds(2f);
            //yield return new WaitUntil(() => Vector2.Distance(m_targetInfo.position, transform.position) > m_info.targetDistanceTolerance);
            //int randomnum = UnityEngine.Random.Range(0, 2);
            int randomnum;
            Vector3 LastPosition = transform.position;
            if (m_targetInfo.position.x < transform.position.x)
            {
                randomnum = 1;
            }else
            {
                randomnum = 0;
            }
            transform.position = new Vector2(m_targetInfo.position.x + (randomnum==1 ? 50f : -50f), GroundPosition(transform.position).y);
            //transform.position = new Vector2(m_targetInfo.position.x + (randomnum == 1 ? 25f : -25f), GroundPosition(m_targetInfo.position).y);
            yield return new WaitForSeconds(0.6f);
            /*
            int check = 0;
            yield return new WaitForSeconds(0.6f);
            while (!m_groundSensor.isDetecting&&check<10)
            {
                check++;
                Debug.Log(check+" "+transform.position);
                transform.position -= new Vector3(0f, 1.5f);
            }
            */
            while (m_wallSensor.isDetecting && m_backSensor.isDetecting)
            {
                transform.position = Vector3.MoveTowards(transform.position, LastPosition, m_currentMoveSpeed);
                yield return null;
            }
            yield return new WaitForSeconds(0.6f);
            if (!m_groundSensor.isDetecting)
            {
                if (randomnum == 0)
                {
                    transform.position += new Vector3(75f, 0);
                    transform.position = new Vector3(transform.position.x, GroundPosition(transform.position).y);
                }
                else
                {
                    transform.position -= new Vector3(75f, 0);
                    transform.position = new Vector3(transform.position.x, GroundPosition(transform.position).y);
                }
            }
            m_animation.SetAnimation(0, m_info.teleportFromBelowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportFromBelowAnimation);
            //m_TeleportVFX.Play();
            m_shadow.SetActive(true);
            m_hitbox.Enable();
            m_character.physics.simulateGravity = true;
            m_bodyCollider.enabled = true;
            m_legCollider.enabled = true;
            m_teleportRoutine = null;
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Healing()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_HasHealed = true;
            m_animation.SetAnimation(0, m_info.healAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnimation);
            GameplaySystem.combatManager.Heal(m_damageable, m_HealValue);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SummonBook()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0,m_info.bookSummonAnimation,false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bookSummonAnimation);
            ApplyBookState(BookState.WithBook);
            m_summonCooldownDuration = m_summonCooldown;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();

            ApplyBookState(BookState.WithoutBook);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            //m_aggroCollider.enabled = m_willPatrol ? true : false;

            if (!m_willPatrol)
            {
                m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
                m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
            }

            m_spineEventListener.Subscribe(m_info.summonTotemEvent, SummonTotem);
            m_startPoint = transform.position;
            m_healthHealThreshold = (m_statsData.maxHealth * 0.2f);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandleWithoutBook.FlinchStart += OnFlinchStart;
            m_flinchHandleWithBook.FlinchStart += OnFlinchStart;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                        m_movement.Stop();

                    

                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_bookState == BookState.WithoutBook && m_summonCooldownDuration <= 0)
                        {
                            StartCoroutine(SummonBook());
                            break;
                        }
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }
                    break;

                case State.Idle:
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_currentMoveAnimation, true).TimeScale = 2f;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_currentMoveSpeed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                        }
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_currentTurnAnimation, m_currentIdleAnimation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    m_selfCollider.enabled = false;
                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_attackHandle.ExecuteAttack(m_info.attackCastingSpell.animation, m_currentIdleAnimation);
                            //m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_currentFullCD)
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
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_currentIdleAnimation)
                                    m_movement.Stop();

                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                Move();
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_currentTurnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.Fleeing:

                    if (m_summonCooldownDuration > 0)
                    {
                        m_summonCooldownDuration -= Time.deltaTime;
                    }

                    if (m_damageable.health.currentValue < m_healthHealThreshold && !m_HasHealed)
                    {
                        StartCoroutine(Healing());
                        break;
                    }

                    TeleportFleeing();
                    
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        switch (m_bookState)
                        {
                            case BookState.WithoutBook:
                                if(m_summonCooldownDuration<=0 &&!m_tome.gameObject.activeSelf && !m_tomeFire.gameObject.activeSelf && !m_tomeStorm.gameObject.activeSelf)
                                {
                                    StartCoroutine(SummonBook());
                                }
                                m_stateHandle.SetState(State.Fleeing);
                                break;
                            case BookState.WithBook:
                                m_stateHandle.SetState(State.Chasing);
                                break;
                        }
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }

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
                    if (m_summonCooldownDuration > 0)
                    {
                        m_summonCooldownDuration -= Time.deltaTime;
                    }
                    return;
            }

            if (m_enablePatience && m_stateHandle.currentState != State.Standby)
            {
                //Patience();
                if (TargetBlocked())
                {
                    m_stateHandle.OverrideState(State.Standby);
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
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
