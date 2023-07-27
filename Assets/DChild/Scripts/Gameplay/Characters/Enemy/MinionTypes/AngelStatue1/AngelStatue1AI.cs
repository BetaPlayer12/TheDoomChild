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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/AngelStatue1")]
    public class AngelStatue1AI : CombatAIBrain<AngelStatue1AI.Info>, IResetableAIBrain, IBattleZoneAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attackLeft = new SimpleAttackInfo();
            public SimpleAttackInfo attackLeft => m_attackLeft;
            [SerializeField]
            private BasicAnimationInfo m_attackLeftNoShieldAnimation;
            public BasicAnimationInfo attackLeftNoShieldAnimation => m_attackLeftNoShieldAnimation;
            [SerializeField]
            private BasicAnimationInfo m_attackLeftSLAnimation;
            public BasicAnimationInfo attackLeftSLAnimation => m_attackLeftSLAnimation;
            [SerializeField]
            private BasicAnimationInfo m_attackLeftSRAnimation;
            public BasicAnimationInfo attackLeftSRAnimation => m_attackLeftSRAnimation;
            [SerializeField]
            private SimpleAttackInfo m_attackRight= new SimpleAttackInfo();
            public SimpleAttackInfo attackRight => m_attackRight;
            [SerializeField]
            private BasicAnimationInfo m_attackRightNoShieldAnimation;
            public BasicAnimationInfo attackRightNoShieldAnimation => m_attackRightNoShieldAnimation;
            [SerializeField]
            private BasicAnimationInfo m_attackRightSLAnimation;
            public BasicAnimationInfo attackRightSLAnimation => m_attackRightSLAnimation;
            [SerializeField]
            private BasicAnimationInfo m_attackRightSRAnimation;
            public BasicAnimationInfo attackRightSRAnimation => m_attackRightSRAnimation;
            [SerializeField]
            private SimpleAttackInfo m_fistAttackLeft = new SimpleAttackInfo();
            public SimpleAttackInfo fistAttackLeft => m_fistAttackLeft;
            [SerializeField]
            private SimpleAttackInfo m_fistAttackRight = new SimpleAttackInfo();
            public SimpleAttackInfo fistAttackRight => m_fistAttackRight;
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
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_idleAnimation;
            //public string idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_awakenAnimation;
            public BasicAnimationInfo awakenAnimation => m_awakenAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_shieldLeftHitAnimation;
            public BasicAnimationInfo shieldLeftHitAnimation => m_shieldLeftHitAnimation;
            [SerializeField]
            private BasicAnimationInfo m_shieldRightHitAnimation;
            public BasicAnimationInfo shieldRightHitAnimation => m_shieldRightHitAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wingLeftDestroyAnimation;
            public BasicAnimationInfo wingLeftDestroyAnimation => m_wingLeftDestroyAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wingRightDestroyAnimation;
            public BasicAnimationInfo wingRightDestroyAnimation => m_wingRightDestroyAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;
            [SerializeField]
            private float m_launchDelay;
            public float launchDelay => m_launchDelay;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_attackLeft.SetData(m_skeletonDataAsset);
                m_attackRight.SetData(m_skeletonDataAsset);
                m_fistAttackLeft.SetData(m_skeletonDataAsset);
                m_fistAttackRight.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_attackLeftNoShieldAnimation.SetData(m_skeletonDataAsset);
                m_attackLeftSLAnimation.SetData(m_skeletonDataAsset);
                m_attackLeftSRAnimation.SetData(m_skeletonDataAsset);
                m_attackRightNoShieldAnimation.SetData(m_skeletonDataAsset);
                m_attackRightSLAnimation.SetData(m_skeletonDataAsset);
                m_attackRightSRAnimation.SetData(m_skeletonDataAsset);

                m_awakenAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_shieldLeftHitAnimation.SetData(m_skeletonDataAsset);
                m_shieldRightHitAnimation.SetData(m_skeletonDataAsset);
                m_wingLeftDestroyAnimation.SetData(m_skeletonDataAsset);
                m_wingRightDestroyAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Attacking,
            Cooldown,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            FistAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Health")]
        private Health m_health;
        [SerializeField, TabGroup("Health")]
        private float m_leftHealth;
        [SerializeField, TabGroup("Health")]
        private float m_rightHealth;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private ProjectileLauncher m_projectileLauncher;
        [SerializeField]
        private Transform m_projectileStart;


        private State m_turnState;
        private string m_attackLeftAnimation;
        private string m_attackRightAnimation;
        private bool m_shieldLeftDestroyed;
        private bool m_shieldRightDestroyed;
        private int m_shieldLeftHitCount;
        private int m_shieldRightHitCount;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            if (m_attackLeftAnimation == m_info.fistAttackLeft.animation)
            {
                m_attackLeftAnimation = m_info.attackLeft.animation;
                m_attackRightAnimation = m_info.attackRight.animation;
            }
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
                if (m_stateHandle.currentState != State.Attacking && !m_isDetecting)
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
                m_stateHandle.SetState(State.Idle);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            m_selfCollider.SetActive(false);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_boundBoxGO.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            
            m_movement.Stop();
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //if (m_stateHandle.currentState == State.Cooldown)
            //{
            //}
            if (m_targetInfo.isValid)
            {
                if (m_targetInfo.position.x < transform.position.x)
                {
                    if (!m_shieldLeftDestroyed)
                    {
                        //m_stateHandle.Wait(State.ReevaluateSituation);
                        //m_hitbox.SetInvulnerability(Invulnerability.Level_1);
                        if (m_shieldLeftHitCount < 3)
                        {
                            m_health.SetHealthPercentage(1);
                            m_animation.SetEmptyAnimation(1, 0);
                            m_animation.SetAnimation(1, m_info.shieldLeftHitAnimation, false);
                            m_shieldLeftHitCount++;
                        }
                        else
                        {
                            m_attackLeftAnimation = m_info.attackLeftSRAnimation.animation;
                            m_attackRightAnimation = m_info.attackRightSRAnimation.animation;
                            if (m_shieldRightDestroyed)
                            {
                                //m_leftHealth = m_rightHealth;
                                m_health.SetHealthPercentage(.001f);
                            }
                            //m_health.SetHealthPercentage(m_leftHealth);
                            //m_leftHealth = ((float)m_health.currentValue / m_health.maxValue);
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.wingLeftDestroyAnimation.animation)
                            {
                                StopAllCoroutines();
                                StartCoroutine(WingsDestroyedRoutine(1, m_info.wingLeftDestroyAnimation.animation));
                            }
                        }
                    }
                }
                else
                {
                    if (!m_shieldRightDestroyed)
                    {
                        if (m_shieldRightHitCount < 3)
                        {
                            m_health.SetHealthPercentage(1);
                            m_animation.SetEmptyAnimation(2, 0);
                            m_animation.SetAnimation(2, m_info.shieldRightHitAnimation, false);
                            m_shieldRightHitCount++;
                        }
                        else
                        {
                            m_attackLeftAnimation = m_info.attackLeftSLAnimation.animation;
                            m_attackRightAnimation = m_info.attackRightSLAnimation.animation;
                            if (m_shieldLeftDestroyed)
                            {
                                //m_rightHealth = m_leftHealth;
                                m_health.SetHealthPercentage(.001f);
                            }
                            //m_health.SetHealthPercentage(m_rightHealth);
                            //m_rightHealth = ((float)m_health.currentValue / m_health.maxValue);
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.wingRightDestroyAnimation.animation)
                            {
                                StopAllCoroutines();
                                StartCoroutine(WingsDestroyedRoutine(2, m_info.wingRightDestroyAnimation.animation));
                            }
                        }
                    }
                }

                if (m_shieldLeftDestroyed && m_shieldRightDestroyed)
                {
                    m_attackLeftAnimation = m_info.attackLeftNoShieldAnimation.animation;
                    m_attackRightAnimation = m_info.attackRightNoShieldAnimation.animation;
                }
            }
        }

        private IEnumerator WingsDestroyedRoutine(int index, string animation)
        {
            m_animation.animationState.GetCurrent(0).TimeScale = 0;
            m_animation.SetAnimation(index, animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, animation);
            if (animation == m_info.wingLeftDestroyAnimation.animation)
            {
                m_shieldLeftDestroyed = true;
            }
            else if (animation == m_info.wingRightDestroyAnimation.animation)
            {
                m_shieldRightDestroyed = true;
            }
            m_stateHandle.OverrideState(State.Cooldown);
            yield return null;
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Idle);
            enabled = true;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.awakenAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ExecuteAttackRoutine(string attack)
        {
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, attack, false).MixDuration = 0;
            yield return new WaitForSeconds(m_attackLeftAnimation == m_info.fistAttackLeft.animation ? 4.9f : 3);
            m_animation.animationState.TimeScale = 0;
            if (m_attackLeftAnimation == m_info.fistAttackLeft.animation)
            {
                m_attackLeftAnimation = m_info.attackLeft.animation;
                m_attackRightAnimation = m_info.attackRight.animation;
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator LaunchProjectilRoutine()
        {
            yield return new WaitForSeconds(m_info.launchDelay);
            LaunchProjectile();
            yield return null;
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {

                //m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
                m_projectileLauncher.LaunchProjectile();
                //m_Audiosource.clip = m_RangeAttackClip;
                //m_Audiosource.Play();
            }
        }

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_attackLeftAnimation = m_info.fistAttackLeft.animation;
            m_attackRightAnimation = m_info.fistAttackRight.animation;

            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_attackHandle.AttackDone += OnAttackDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectileStart);
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_movement.Stop();
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    m_movement.Stop();
                    break;

                case State.Attacking:
                    if (IsTargetInRange(m_info.attackLeft.range))
                    {
                        m_stateHandle.Wait(State.Cooldown);

                        //var target = new Vector2(m_targetInfo.position.x, m_projectileStart.position.y);
                        m_projectileLauncher.AimAt(m_targetInfo.position);
                        //StartCoroutine(LaunchProjectilRoutine());

                        if (m_targetInfo.position.x < transform.position.x)
                        {
                            //m_attackHandle.ExecuteAttack(m_attackLeftAnimation, null);
                            StartCoroutine(ExecuteAttackRoutine(m_attackLeftAnimation));
                        }
                        else
                        {
                            //m_attackHandle.ExecuteAttack(m_attackRightAnimation, null);
                            StartCoroutine(ExecuteAttackRoutine(m_attackRightAnimation));
                        }
                    }
                    break;

                case State.Cooldown:

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_animation.animationState.TimeScale = 1;
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
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
            m_stateHandle.OverrideState(State.Idle);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(true);
        }

        public void SwitchToBattleZoneAI()
        {
            m_stateHandle.SetState(State.Attacking);
        }

        public void SwitchToBaseAI()
        {
            m_stateHandle.SetState(State.ReevaluateSituation);
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
