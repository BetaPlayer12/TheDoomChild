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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PosessedFemale")]
    public class PosessedFemaleAI : CombatAIBrain<PosessedFemaleAI.Info>
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
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private SimpleAttackInfo m_attackEnrage = new SimpleAttackInfo();
            public SimpleAttackInfo attackEnrage => m_attackEnrage;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stunAnimation;
            public string stunAnimation => m_stunAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [Title("FX")]
            [SerializeField]
            private GameObject m_explodeFX;
            public GameObject explodeFX => m_explodeFX;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_explodeEvent;
            public string explodeEvent => m_explodeEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_attackEnrage.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Patrol,
            Cooldown,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            //AttackEnrage,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("Hitbox")]
        private GameObject m_explodeHitbox;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_summonFX;

        [SerializeField]
        private Transform m_knocbackDirection;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;

        private float m_currentCD;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_isDetecting;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_currentAttack = Attack.Attack;
            m_flinchHandle.gameObject.SetActive(true);
            m_flinchHandle.m_autoFlinch = true;
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

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

        private bool TargetBlocked()
        {
            Vector2 wat = m_character.centerMass.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_summonFX.Play();
            m_stateHandle.OverrideState(State.Detect);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_agent.Stop();
            m_character.physics.SetVelocity(Vector2.zero);
            StartCoroutine(KnockbackRoutine());
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                //m_stateHandle.Wait(State.Cooldown);
            }
        }

        private IEnumerator KnockbackRoutine()
        {
            float time = 0;
            m_agent.enabled = false;
            m_selfCollider.enabled = false;
            while (time < .25f)
            {
                m_character.physics.SetVelocity(50 * (transform.position.x > m_targetInfo.position.x ? 1 : -1), 0);
                time += Time.deltaTime;
                yield return null;
            }
            m_agent.enabled = true;
            m_selfCollider.enabled = true;
            m_agent.Stop();
            m_character.physics.SetVelocity(Vector2.zero);
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation && m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                    m_animation.SetEmptyAnimation(0, 0);
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
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                StopAllCoroutines();
                m_agent.Stop();
                m_character.physics.SetVelocity(Vector2.zero);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range)
                                  /*, new AttackInfo<Attack>(Attack.AttackEnrage, m_info.attackEnrage.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            m_bodyCollider.SetActive(true);
            m_selfCollider.enabled = false;
            m_agent.Stop();
            m_character.physics.SetVelocity(Vector2.zero);
            m_character.physics.simulateGravity = true;
        }
        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_character.physics.SetVelocity(Vector2.zero);
            switch (/*m_attack*/ m_currentAttack)
            {
                case Attack.Attack:
                    m_animation.EnableRootMotion(false, false);
                    m_selfCollider.enabled = false;
                    StartCoroutine(ExplodeRoutine());
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                //case Attack.AttackEnrage:
                //    m_animation.EnableRootMotion(false, false);
                //    m_attackHandle.ExecuteAttack(m_info.attackEnrage.animation, m_info.idleAnimation);
                //    break;
            }
        }

        private IEnumerator ExplodeRoutine()
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            yield return null;
        }

        private void Explode()
        {
            Debug.Log("EXPLODE FEMALE");
            var explodeFX = this.InstantiateToScene(m_info.explodeFX, new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity);
            StartCoroutine(SpawnExplodeHitboxRoutine());
        }

        private IEnumerator SpawnExplodeHitboxRoutine()
        {
            m_explodeHitbox.SetActive(true);
            yield return new WaitForSeconds(.25f);
            m_explodeHitbox.SetActive(false);
            m_hitbox.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            var randXPos = UnityEngine.Random.Range(-2f, 2f);
            var randYPos = UnityEngine.Random.Range(10f, 15f); ;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_targetInfo.position.x + randXPos, m_targetInfo.position.y + randYPos);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(/*m_targetInfo.position.y*/newPos.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //DynamicMovement(/*new Vector2(m_targetInfo.position.x, m_targetInfo.position.y)*/ newPos);
                DynamicMovement(newPos, moveSpeed);
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
            //var rb2d = GetComponent<Rigidbody2D>();
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

            if (IsFacing(m_agent.hasPath && TargetBlocked()  ? m_agent.segmentDestination : target))
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                m_agent.SetDestination(target);
                m_agent.Move(movespeed);

                m_animation.SetAnimation(0, m_info.move.animation, true);
            }
            else
            {
                m_turnState = State.Attacking;
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            //m_currentAttack = Attack.AttackEnrage;
            m_currentAttack = Attack.Attack;
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_spineEventListener.Subscribe(m_info.explodeEvent, Explode);
            //m_selfCollider.SetActive(false);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    m_character.physics.SetVelocity(Vector2.zero);
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (m_animation.GetCurrentAnimation(0).ToString() == m_info.patrol.animation)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_agent.Stop();
                    m_character.physics.SetVelocity(Vector2.zero);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idle2Animation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_flinchHandle.m_autoFlinch = false;
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_character.physics.SetVelocity(Vector2.zero);
                    StartCoroutine(ExecuteMove(m_attackDecider.chosenAttack.range, m_attackDecider.chosenAttack.attack));
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
                            m_character.physics.SetVelocity(Vector2.zero);
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
                    m_attackDecider.DecideOnAttack();
                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                    {
                        m_agent.Stop();
                        m_character.physics.SetVelocity(Vector2.zero);
                        m_stateHandle.SetState(State.Attacking);
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
            m_hitbox.gameObject.SetActive(true);
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = true;
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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