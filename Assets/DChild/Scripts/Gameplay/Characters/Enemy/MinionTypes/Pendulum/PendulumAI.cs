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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Pendulum")]
    public class PendulumAI : CombatAIBrain<PendulumAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_attackStartAnimation;
            public string attackStartAnimation => m_attackStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_attackEndAnimation;
            public string attackEndAnimation => m_attackEndAnimation;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_hurtboxDuration;
            public float hurtboxDuration => m_hurtboxDuration;
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathLoopAnimation;
            public string deathLoopAnimation => m_deathLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathTransitionEndAnimation;
            public string deathTransitionEndAnimation => m_deathTransitionEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathEndAnimation;
            public string deathEndAnimation => m_deathEndAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_hitboxStartEvent;
            public string hitboxStartEvent => m_hitboxStartEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_attack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
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
            Attack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_pelvisModel;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("BoundingBox")]
        private List<Collider2D> m_attackBBs;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomTurnRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

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
                    if (!m_isDetecting)
                    {
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
            }
            else
            {
                m_enablePatience = true;
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
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.SetState(State.Idle);
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
            m_movement.Stop();
            m_hitbox.Disable();
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_pelvisModel.SetActive(true);
            m_pelvisModel.transform.SetParent(null);
            m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathLoopAnimation, true);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.deathTransitionEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathTransitionEndAnimation);
            m_animation.SetAnimation(0, m_info.deathEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathEndAnimation);
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idle1Animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle2Animation)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_stateHandle.ApplyQueuedState();
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private string RandomIdleAnimation()
        {
            int idle = UnityEngine.Random.Range(1, 3);

            switch (idle)
            {
                case 1:
                    return m_info.idle1Animation;
                case 2:
                    return m_info.idle2Animation;
            }
            return null;
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
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.attackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackStartAnimation);
            while (IsTargetInRange(m_attackDecider.chosenAttack.range + 20))
            {
                m_animation.SetAnimation(0, m_info.attack.animation, true);
                yield return null;
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
                //m_animation.SetAnimation(0, m_info.attackMidSequenceAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackMidSequenceAnimation);
            }
            m_animation.SetAnimation(0, m_info.attackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackEndAnimation);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_stateHandle.ApplyQueuedState();
        }

        private void EnableAttackBB()
        {
            for (int i = 0; i < m_attackBBs.Count; i++)
            {
                m_attackBBs[i].enabled = !m_attackBBs[i].enabled;
            }
        }

        protected override void Start()
        {
            base.Start();

            //m_aggroCollider.enabled = m_willPatrol ? true : false;
            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());

            m_spineEventListener.Subscribe(m_info.hitboxStartEvent, EnableAttackBB);
        }

        protected override void Awake()
        {
            base.Awake();
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathStartAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
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
                    if (!IsFacingTarget())
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

                case State.Idle:
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, RandomIdleAnimation());
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    
                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
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
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
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
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                            {
                                m_movement.Stop();
                                //m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_movement.Stop();
                                if (m_animation.animationState.GetCurrent(0).IsComplete)
                                {
                                    m_animation.SetAnimation(0, RandomIdleAnimation(), true);
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
                        m_stateHandle.SetState(State.Idle);
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

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Idle);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
        }

        public void ResetAI()
        {
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
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
