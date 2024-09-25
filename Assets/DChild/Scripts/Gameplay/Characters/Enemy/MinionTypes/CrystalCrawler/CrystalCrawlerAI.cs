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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/CrystalCrawler")]
    public class CrystalCrawlerAI : CombatAIBrain<CrystalCrawlerAI.Info>, IResetableAIBrain
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
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
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


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinch1Animation;
            public BasicAnimationInfo flinch1Animation => m_flinch1Animation;
            [SerializeField]
            private BasicAnimationInfo m_flinch2Animation;
            public BasicAnimationInfo flinch2Animation => m_flinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_death1Animation;
            public BasicAnimationInfo death1Animation => m_death1Animation;
            [SerializeField]
            private BasicAnimationInfo m_death2Animation;
            public BasicAnimationInfo death2Animation => m_death2Animation;

            [SerializeField]
            private GameObject m_spikeGO;
            public GameObject spikeGO => m_spikeGO;
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

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinch1Animation.SetData(m_skeletonDataAsset);
                m_flinch2Animation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_death1Animation.SetData(m_skeletonDataAsset);
                m_death2Animation.SetData(m_skeletonDataAsset);
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
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroCollider;
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

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_spikeEdgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_explodeFX;

        [SerializeField]
        private Transform m_model;
        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;

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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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
            //m_animation.SetEmptyAnimation(0, 0);
            //var deathAnim = UnityEngine.Random.Range(0, 2) == 0 ? m_info.death1Animation : m_info.death2Animation;
            //m_animation.SetAnimation(0, m_info.death2Animation, false);
            Debug.Log("death");
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation && m_attackRoutine == null)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                m_selfCollider.enabled = true;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.ApplyQueuedState();
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.AttackMelee, m_info.attack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator AttackRoutine(Vector2 target)
        {

            //var spike = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_info.spikeGO);
            //spike.GetComponent<CrystalCrawlerSpike>().StopShatterShard();
            GameObject spike = m_info.spikeGO;
            
            spike.transform.position = transform.position;
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            //float speed = (2f * transform.localScale.x);
            //var spike = this.InstantiateToScene(m_info.spikeGO, transform.position, Quaternion.identity);
            //m_spike.transform.SetParent(null);
            //m_spike.transform.localScale = new Vector3(-transform.localScale.x, m_spike.transform.localScale.y, m_spike.transform.localScale.z);
            spike.transform.localScale = new Vector3(-transform.localScale.x, spike.transform.localScale.y, spike.transform.localScale.z);
            while (m_spikeEdgeSensor.isDetecting && Mathf.Abs(target.x - spike.transform.position.x) > 2f)
            {
                Debug.Log("Distance of Spike to Destination: " + Vector2.Distance(target, spike.transform.position));
                //m_spike.transform.position = new Vector2(m_spike.transform.position.x + speed, m_spike.transform.position.y);
                spike.transform.position = Vector2.MoveTowards(spike.transform.position, target, 5);
                m_spikeEdgeSensor.transform.position = Vector2.MoveTowards(m_spikeEdgeSensor.transform.position, target, 5);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            Instantiate(m_info.spikeGO, target, Quaternion.identity);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_spikeEdgeSensor.transform.localPosition = new Vector2(7.5f, 2f); 
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //yield return new WaitUntil(() => !m_spike.gameObject.activeSelf);
            //m_spike.transform.SetParent(m_model);
            //m_spike.transform.localPosition = Vector2.zero;
            m_attackRoutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {

            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //Debug.Log("DETECT PLAYER CRYSTAL");
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
            //m_deathHandle.SetAnimation(m_info.death1Animation.animation);
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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        m_movement.Stop();

                    if (!IsFacingTarget() && m_willPatrol)
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                            m_movement.Stop();

                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);


                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackMelee:
                            //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                            var groundedTarget = new Vector2(m_targetInfo.position.x, transform.position.y);
                            m_attackRoutine = StartCoroutine(AttackRoutine(groundedTarget));
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        //if (m_animation.animationState.GetCurrent(0).IsComplete)
                        //{
                        //}
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting /*&& !m_spike.gameObject.activeSelf*/)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    m_movement.Stop();

                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(false, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                        m_movement.Stop();

                                    m_selfCollider.enabled = true;
                                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                                    {
                                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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

            if (m_enablePatience && m_stateHandle.currentState != State.Standby && m_targetInfo.isValid)
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
