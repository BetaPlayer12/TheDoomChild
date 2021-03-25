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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/DireBrothers")]
    public class DireBrothersAI : CombatAIBrain<DireBrothersAI.Info>, IResetableAIBrain, IBattleZoneAIBrain
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
            private SimpleAttackInfo m_heavyGroundAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundAttack => m_heavyGroundAttack;
            [SerializeField]
            private SimpleAttackInfo m_spearAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spearAttack => m_spearAttack;
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
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_heavyFlinchAnimation;
            public string heavyFlinchAnimation => m_heavyFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lightFlinchAnimation;
            public string lightFlinchAnimation => m_lightFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_backFlinchAnimation;
            public string backFlinchAnimation => m_backFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_counterFlinchAnimation;
            public string counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defenceStanceAnimation;
            public string defenceStanceAnimation => m_defenceStanceAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defenceWhenHitAnimation;
            public string defenceWhenHitAnimation => m_defenceWhenHitAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_exhaustedAnimation;
            public string exhaustedAnimation => m_exhaustedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_heavyGroundAttack.SetData(m_skeletonDataAsset);
                m_spearAttack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Patrol,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            HeavyGroundAttack,
            SpearAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private State m_turnState;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

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

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_selfCollider.SetActive(false);
                m_targetInfo.Set(null, null);
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            m_selfCollider.SetActive(false);
            m_hitbox.Disable();
            m_boundBoxGO.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_animation.animationState.TimeScale = .5f;
            m_animation.SetAnimation(0, m_info.lightFlinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_animation.animationState.TimeScale = 1f;
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                m_animation.SetEmptyAnimation(0, 0);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.HeavyGroundAttack, m_info.heavyGroundAttack.range),
                                    new AttackInfo<Attack>(Attack.SpearAttack, m_info.spearAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator HeavyGroundAttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.heavyGroundAttack.animation, false);
            yield return new WaitForSeconds(1.5f);
            m_character.physics.SetVelocity(50 * transform.localScale.x, 0);
            yield return new WaitForSeconds(3f);
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavyGroundAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
                        m_currentAttackRange = m_attackRangeCache[i];
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

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.HeavyGroundAttack, Attack.SpearAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.heavyGroundAttack.range, m_info.spearAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
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

                case State.Patrol:
                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting)
                        {
                            m_stateHandle.Wait(State.Cooldown);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);

                            switch (/*m_attackDecider.chosenAttack.attack*/ m_currentAttack)
                            {
                                case Attack.HeavyGroundAttack:
                                    //m_animation.EnableRootMotion(true, false);
                                    //m_attackHandle.ExecuteAttack(m_info.heavyGroundAttack.animation, m_info.idleAnimation);
                                    //
                                    StartCoroutine(HeavyGroundAttackRoutine());
                                    break;
                                case Attack.SpearAttack:
                                    m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.spearAttack.animation, m_info.idleAnimation);
                                    break;
                            }
                            m_attackDecider.hasDecidedOnAttack = false;
                        }
                        else
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                            }
                            else
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (IsTargetInRange(100))
                    {
                        if (!IsFacingTarget())
                        {
                            m_turnState = State.Cooldown;
                            m_stateHandle.SetState(State.Turning);
                        }
                        else
                        {
                            m_animation.EnableRootMotion(false, false);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }

                    if (m_currentCD <= m_info.attackCD && !m_wallSensor.isDetecting)
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
                        //m_attackDecider.DecideOnAttack();
                        m_attackDecider.hasDecidedOnAttack = false;
                        ChooseAttack();
                        if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                        {
                            m_movement.Stop();
                            m_stateHandle.SetState(State.Attacking);
                        }
                        //else
                        //{
                        //    m_attackDecider.hasDecidedOnAttack = false;
                        //    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                        //    {
                        //        var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                        //        m_animation.EnableRootMotion(false, false);
                        //        m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                        //        m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                        //    }
                        //    else
                        //    {
                        //        m_movement.Stop();
                        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        //    }
                        //}
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
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_hitbox.Enable();
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(true);
        }

        public void SwitchToBattleZoneAI()
        {
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}
