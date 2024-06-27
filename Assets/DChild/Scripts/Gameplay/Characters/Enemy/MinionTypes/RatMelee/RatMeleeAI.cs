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
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/RatMelee")]
    public class RatMeleeAI : CombatAIBrain<RatMeleeAI.Info>, IResetableAIBrain, IKnockbackable
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
            private SimpleAttackInfo m_attackMelee = new SimpleAttackInfo();
            public SimpleAttackInfo attackMelee => m_attackMelee;
            [SerializeField, MinValue(0)]
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
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_burrowAnim;
            public string burrowAnim => m_burrowAnim;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_burrowEvent;
            public string burrowEvent => m_burrowEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attackMelee.SetData(m_skeletonDataAsset);
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
            RunAway,
            Burrow,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            AttackMelee,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private IsolatedCharacterPhysics2D m_characterPhysics;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
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
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private SpineEventListener m_spineListener;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_burrowFX;
    

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        [SerializeField]
        private Collider2D m_biteBox;

        [SerializeField, Range(0, 1)]
        private float m_stealPercentage;

        private Coroutine m_randomIdleRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomTurnRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_selfCollider.enabled = false;
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
                if (!TargetBlocked() && !m_enablePatience)
                {
                    m_selfCollider.enabled = false;
                    if (m_randomIdleRoutine != null)
                    {
                        StopCoroutine(m_randomIdleRoutine);
                    }

                    if (!m_isDetecting)
                    {
                        m_isDetecting = true;
                        m_stateHandle.SetState(State.Detect);
                    }
                    m_currentPatience = 0;
                    //m_randomIdleRoutine = null;
                    //var patienceRoutine = PatienceRoutine();
                    //StopCoroutine(patienceRoutine);
                    m_enablePatience = false;
                }
            }
            else
            {
                m_enablePatience = true;
            }
        }

        private bool TargetBlocked()
        {
            Vector2 wat = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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
            m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
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
            
            m_characterPhysics.UseStepClimb(true);
            m_movement.Stop();
            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_selfCollider.enabled = false;
            m_stateHandle.OverrideState(m_targetInfo.isValid ? State.RunAway : State.ReevaluateSituation);
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.AttackMelee, m_info.attackMelee.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            //m_animation.SetAnimation(0, m_info.detectAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator RandomIdleRoutine()
        {
            var timer = UnityEngine.Random.Range(5, 10);
            var currentTimer = 0f;
            while (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
                yield return null;
            }
            m_stateHandle.Wait(State.Patrol);
            m_movement.Stop();
            var idleAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.idleAnimation : m_info.idle2Animation;
            m_animation.SetAnimation(0, idleAnim, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, idleAnim);
            m_stateHandle.ApplyQueuedState();
            yield return null;
            StartCoroutine(RandomIdleRoutine());
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
            m_movement.Stop();
            while (true)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                //m_animation.SetAnimation(0, m_info.idle2Animation, true);
                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }
        private void BurrowEvent()
        {
            m_burrowFX.Play();
            //throw new System.NotImplementedException();
        }
        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_spineListener.Subscribe(m_info.burrowEvent, BurrowEvent);
            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
            if (m_willPatrol)
            {
                StopCoroutine(m_randomTurnRoutine);
                m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
            }

            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
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
                    m_movement.Stop();
                    StopCoroutine(m_randomTurnRoutine);
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

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true)/*.TimeScale = 0.5f*/;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;


                case State.Standby:
                    Patience();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.RunAway);


                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackMelee:
                            m_attackHandle.ExecuteAttack(m_info.attackMelee.animation, m_info.idleAnimation);
                            StartCoroutine(BiteRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        if (!IsFacingTarget())
                        {
                            m_turnState = State.Cooldown;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
                        else
                        {
                            var animation = UnityEngine.Random.Range(0, 100) > 2 ? m_info.idleAnimation : m_info.idle2Animation;
                            if (m_animation.animationState.GetCurrent(0).IsComplete)
                            {
                                m_animation.SetAnimation(0, animation, true);
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

                case State.RunAway:
                    if (IsFacingTarget())
                    {
                        m_turnState = State.RunAway;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance /*&& !m_wallSensor.isDetecting && m_groundSensor.isDetecting*/ && m_edgeSensor.isDetecting)
                        {
                            var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                            m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                            m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance <= m_info.targetDistanceTolerance ? m_currentMoveSpeed : m_info.patrol.speed);
                        }
                        else
                        {
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                            m_stateHandle.SetState(State.Burrow);
                        }
                    }

                    if (m_patienceRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        m_enablePatience = false;
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }

                    if (m_sneerRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }

                    break;
                case State.Burrow:
                    StartCoroutine(BurrowRoutine());
                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                            {
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(false, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                                }
                                else
                                {
                                    if (m_character.physics.velocity.x != 0) m_animation.SetAnimation(0, m_info.idleAnimation, true);

                                    m_movement.Stop();
                                    m_selfCollider.enabled = true;
                                    var animation = UnityEngine.Random.Range(0, 100) > 2 ? m_info.idleAnimation : m_info.idle2Animation;
                                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                                    {
                                        m_animation.SetAnimation(0, animation, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
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
        private IEnumerator BurrowRoutine()
        {
            /*m_burrowFX.Play();*/
            m_animation.SetAnimation(0, m_info.burrowAnim, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.burrowAnim);
            m_burrowFX.Stop();
            yield return null;
        }
        private IEnumerator BiteRoutine()
        {
            yield return new WaitForSeconds(.5f);
            m_biteBox.enabled = true;
            StartCoroutine(StealItems());
            yield return new WaitForSeconds(.2f);
            m_biteBox.enabled = false;
            yield return null;
        }
        private IEnumerator StealItems()
        {
            var random = UnityEngine.Random.RandomRange(0, 1f);
            var playerInventory = GameplaySystem.playerManager.player.inventory.FindStoredItemsOfType(ItemCategory.Consumable);
            if ((Mathf.Round(random * 10.0f) * 0.1f) <= m_stealPercentage)
            {
                var randomItem = UnityEngine.Random.RandomRange(0, (playerInventory.Length));
                if (playerInventory.Length > 0)
                {
                    GameplaySystem.playerManager.player.inventory.RemoveItem(playerInventory[randomItem].data, 1);
                    Debug.Log("You're too late, you'll never find your " + playerInventory[randomItem].data.name + " now!");
                }
                else
                {
                    Debug.Log("Aw man!");
                }
            }
            yield return null;
        }
        protected override void OnTargetDisappeared()
        {
            m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
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
            m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
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

        public void HandleKnockback(float resumeAIDelay)
        {
            StopAllCoroutines();
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            m_characterPhysics.UseStepClimb(false);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            m_characterPhysics.UseStepClimb(true);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
