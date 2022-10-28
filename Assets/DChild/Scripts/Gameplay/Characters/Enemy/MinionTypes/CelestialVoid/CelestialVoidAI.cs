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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/CelestialVoid")]
    public class CelestialVoidAI : CombatAIBrain<CelestialVoidAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_counterFlinchAnimation;
            public string counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fadeInAnimation;
            public string fadeInAnimation => m_fadeInAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fadeOutAnimation;
            public string fadeOutAnimation => m_fadeOutAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lifeDrainAnticipationAnimation;
            public string lifeDrainAnticipationAnimation => m_lifeDrainAnticipationAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lifeDrainAnimation;
            public string lifeDrainAnimation => m_lifeDrainAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lifeDrainEndAnimation;
            public string lifeDrainEndAnimation => m_lifeDrainEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_healAnticipationAnimation;
            public string healAnticipationAnimation => m_healAnticipationAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_healAnimation;
            public string healAnimation => m_healAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_healEndAnimation;
            public string healEndAnimation => m_healEndAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Detect,
            ReturnToPatrol,
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
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_floorSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFXline;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainFXline2;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFXline;
        [SerializeField, TabGroup("FX")]
        private GameObject m_lifeDrainEnragedFXline2;
        [SerializeField]
        private bool m_willPatrol;
        [SerializeField]
        private LifeDrain m_lifedrain;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentCD;
        private bool m_isDetecting;
        private Vector2 m_startPos;
        private bool m_isEnraged;
        private bool m_isdraining;

        private Coroutine m_executeMoveCoroutine;


        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_selfCollider.SetActive(true);
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            StopAllCoroutines();
            m_agent.Stop();
            m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            if (m_targetInfo.isValid)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                if (m_animation.GetCurrentAnimation(0).ToString() == m_info.attack1.animation)
                {
                    StartCoroutine(CounterFlinchRoutine());
                }
                else
                {
                    StartCoroutine(FlinchRoutine());
                }
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator CounterFlinchRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.counterFlinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.counterFlinchAnimation);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            ExecuteAttack(Attack.Attack1);
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_lifedrain.setrage(true);
            m_isEnraged = true;
            if (!m_targetInfo.isValid)
            {
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

        //Patience Handler
        private void Patience()
        {
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            StartCoroutine(HealRoutine());
           
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
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.fadeInAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeInAnimation);
            yield return new WaitForSeconds(2);
            var random = UnityEngine.Random.Range(0, 2);
            transform.position = new Vector2(m_targetInfo.position.x + (IsFacingTarget() ? -5 : 5), m_targetInfo.position.y);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            
            m_animation.SetAnimation(0, m_info.fadeOutAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fadeOutAnimation);
            m_hitbox.gameObject.SetActive(true);
            StartCoroutine(LifeDrainRoutine());
            yield return null;
            //m_animation.SetAnimation(0, m_info.attack1.animation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
           // m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_selfCollider.SetActive(false);
            //m_stateHandle.ApplyQueuedState();
           // yield return null;
        }
        private IEnumerator LifeDrainRoutine()
        {
            m_animation.SetAnimation(0, m_info.lifeDrainAnticipationAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lifeDrainAnticipationAnimation);
            m_lifedrain.ActivateLifeDrain();
            if (m_isEnraged == true)
            {
                m_lifeDrainEnragedFX.SetActive(true);
            }
            else
            {
                m_lifeDrainFX.SetActive(true);
            }
            m_isdraining = true;
            m_animation.SetAnimation(0, m_info.lifeDrainAnimation, true);
            yield return null;

        }
        public void DeActivateLifeDrain()
        {
            StartCoroutine(LifeDrainEndRoutine());

        }
        private IEnumerator LifeDrainEndRoutine()
        {
            m_lifeDrainFX.SetActive(false);
            m_lifeDrainEnragedFX.SetActive(false);
            m_isdraining = false;
            m_animation.SetAnimation(0, m_info.lifeDrainEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lifeDrainEndAnimation);
            if (m_isEnraged == false)
            {
                
                if (m_executeMoveCoroutine != null)
                {
                    StopCoroutine(m_executeMoveCoroutine);
                    m_executeMoveCoroutine = null;
                }
                StartCoroutine(HealRoutine());
            }
            else
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.ApplyQueuedState();
            }
            yield return null;

        }
        private IEnumerator HealRoutine()
        {
            m_animation.SetAnimation(0, m_info.healAnticipationAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnticipationAnimation);
            m_lifedrain.AfterRage();
            m_animation.SetAnimation(0, m_info.healAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healAnimation);
            m_animation.SetAnimation(0, m_info.healEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.healEndAnimation);
            m_agent.Stop();
            m_lifedrain.setrage(false);
            m_isEnraged = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
            private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_bodyCollider.enabled = true;
            m_agent.Stop();
            var rb2d = GetComponent<Rigidbody2D>();
            m_character.physics.simulateGravity = true;
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        return;
                    }
                }
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        private void ExecuteAttack(Attack m_attack)
        {
           
                    m_animation.EnableRootMotion(true, false);
                    StartCoroutine(AttackRoutine());
              
        }

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var newPos = Vector2.zero;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y);
                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float movespeed)
        {
            var rb2d = GetComponent<Rigidbody2D>();
            m_agent.SetDestination(target);

            if (/*m_wallSensor.allRaysDetecting ||*/ m_selfSensor.isDetecting)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                //m_bodyCollider.SetActive(true);
                m_agent.Stop();
                rb2d.isKinematic = false;
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                return;
            }

            if (IsFacing(m_agent.hasPath && TargetBlocked() && !m_floorSensor.allRaysDetecting && !m_roofSensor.allRaysDetecting ? m_agent.segmentDestination : m_targetInfo.position))
            {
                if (!m_wallSensor.allRaysDetecting && (m_floorSensor.allRaysDetecting || m_roofSensor.allRaysDetecting))
                {
                    m_bodyCollider.enabled = false;
                    m_agent.Stop();
                    Vector3 dir = (target - (Vector2)rb2d.transform.position).normalized;
                    rb2d.MovePosition(rb2d.transform.position + dir * movespeed * Time.fixedDeltaTime);


                    return;
                }

                //m_bodyCollider.enabled = true;
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                m_agent.SetDestination(target);
                m_agent.Move(movespeed);

            }
            else
            {
                m_turnState = State.ReevaluateSituation;
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        //private void DynamicMovement(Vector2 target)
        //{
        //    if (IsFacingTarget())
        //    {
        //        var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
        //        var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
        //        m_agent.SetDestination(target);
        //        m_agent.Move(m_info.move.speed);

        //        m_animation.SetAnimation(0, m_info.move.animation, true);
        //    }
        //    else
        //    {
        //        m_turnState = State.Attacking;
        //        m_stateHandle.OverrideState(State.Turning);
        //    }
        //}
        #endregion

        protected override void Start()
        {
            base.Start();
            m_startPos = transform.position;
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            LineRenderer lineRenderer1 = m_lifeDrainFXline.GetComponent<LineRenderer>();
            LineRenderer lineRenderer2 = m_lifeDrainFXline2.GetComponent<LineRenderer>();
            LineRenderer EnragedlineRenderer1 = m_lifeDrainEnragedFXline.GetComponent<LineRenderer>();
            LineRenderer EnragedlineRenderer2 = m_lifeDrainEnragedFXline2.GetComponent<LineRenderer>();
            //m_selfCollider.SetActive(false);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack1);
            m_attackRangeCache = new List<float>();
            m_attackUsed = new bool[m_attackCache.Count];
            m_isEnraged = false;
            m_isdraining = false;
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReturnToPatrol:
                    if (IsFacing(m_startPos))
                    {
                        if (Vector2.Distance(m_startPos, transform.position) > 5f)
                        {
                            var rb2d = GetComponent<Rigidbody2D>();
                            m_bodyCollider.enabled = false;
                            m_agent.Stop();
                            Vector3 dir = (m_startPos - (Vector2)rb2d.transform.position).normalized;
                            rb2d.MovePosition(rb2d.transform.position + dir * m_info.patrol.speed * Time.fixedDeltaTime);
                            m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Patrol);
                        }
                    }
                    else
                    {
                        m_turnState = State.ReturnToPatrol;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.Patrol;
                    m_animation.DisableRootMotion();
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(AttackRoutine());
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                       
                            m_agent.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;

                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_selfCollider.SetActive(true);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    m_agent.Stop();
                    m_stateHandle.SetState(State.Attacking);
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

            if (m_targetInfo.isValid)
            {
                if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_bodyCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            ResetAI();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}