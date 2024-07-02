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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PlantIllusion")]
    public class PlantIllusionAI : CombatAIBrain<PlantIllusionAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack1Hide = new SimpleAttackInfo();
            public SimpleAttackInfo attack1Hide => m_attack1Hide;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private SimpleAttackInfo m_attack2Hide = new SimpleAttackInfo();
            public SimpleAttackInfo attack2Hide => m_attack2Hide;
            //

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation2 = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimatio2n => m_idleAnimation2;
            [SerializeField]
            private BasicAnimationInfo m_surpriseAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo surpriseAnimation => m_surpriseAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dartFXEvent;
            public string dartFXEvent => m_dartFXEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeFXEvent;
            public string smokeFXEvent => m_smokeFXEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeStopFXEvent;
            public string smokeStopFXEvent => m_smokeStopFXEvent;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack1Hide.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack2Hide.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation2.SetData(m_skeletonDataAsset);
                m_surpriseAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Burrowed,
            Turning,
            Attacking,
            Cooldown,
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

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_smokeFX;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("AttackHitBoxes")]
        private GameObject m_smokeHitbox;

        private float m_currentCD;
        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_projectileLauncher;

        [SerializeField]
        private Transform m_projectileStart;

        private bool isFirstEncounter = true;
        protected override void Start()
        {
            base.Start();

            m_spineEventListener.Subscribe(m_info.dartFXEvent, LaunchProjectile);
            m_spineEventListener.Subscribe(m_info.smokeFXEvent, SmokeStart);
            m_spineEventListener.Subscribe(m_info.smokeStopFXEvent, SmokeEnd);
            //GameplaySystem.SetBossHealth(m_character);
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                m_projectileLauncher.LaunchProjectile();
                //m_Audiosource.clip = m_RangeAttackClip;
                //m_Audiosource.Play();
            }
        }

        private void SmokeStart()
        {
            m_smokeHitbox.SetActive(true);
            m_smokeFX.Play();
        }

        private void SmokeEnd()
        {
            m_smokeHitbox.SetActive(false);
            m_smokeFX.Stop();
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.SetState(State.Chasing);
                m_currentPatience = 0;
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
                m_flinchHandle.m_autoFlinch = true;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Burrowed);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            m_animation.SetEmptyAnimation(1, 0);
            base.OnDestroyed(sender, eventArgs);
            
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.attack1Hide.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.attack2Hide.animation)
            {
                StartCoroutine(FlinchRoutine());
            }
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(1, m_info.flinchAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation.animation);
            m_animation.SetEmptyAnimation(1, 0);
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                    m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
                m_stateHandle.ApplyQueuedState();
            }
        }

      
        private IEnumerator SurpriseAttack()
        {
            isFirstEncounter = false;
            m_animation.SetAnimation(0, m_info.surpriseAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.surpriseAnimation);
        }
        private IEnumerator AttackRoutineOne()
        {
            if (isFirstEncounter == true)
            {
                yield return SurpriseAttack();
            }
                

            isFirstEncounter = false;
            var target = new Vector2(m_targetInfo.position.x, m_projectileStart.position.y);
            //var target = m_targetInfo.position; //No Parabola      
            Vector2 spitPos = m_projectileStart.position;
            Vector3 v_diff = (target - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            //m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
            m_projectileLauncher.AimAt(target);
            m_animation.EnableRootMotion(true, false);
            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimatio2n.animation);
           

        }
        private IEnumerator AttackRoutineTwo()
        {
            if (isFirstEncounter == true)
            {
                yield return SurpriseAttack();
            }

            isFirstEncounter = false;
            m_animation.EnableRootMotion(true, false);
            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimatio2n.animation);
        }
        
  
        public override void ApplyData()
        {
            if (m_info != null)
            {
                m_spineEventListener.Unsubcribe(m_info.projectile.launchOnEvent, m_projectileLauncher.LaunchProjectile);
            }
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }

            if (m_projectileLauncher != null)
            {
                m_projectileLauncher.SetProjectile(m_info.projectile.projectileInfo);
                m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, m_projectileLauncher.LaunchProjectile);
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Burrowed, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectileStart);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Burrowed:
                    isFirstEncounter = true;
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
                    //m_animation.SetEmptyAnimation(0, 0);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            //if(isFirstEncounter)
                            //StartCoroutine(SurpriseAttack());

                            //isFirstEncounter = false;
                            //var target = new Vector2(m_targetInfo.position.x, m_projectileStart.position.y);
                            ////var target = m_targetInfo.position; //No Parabola      
                            //Vector2 spitPos = m_projectileStart.position;
                            //Vector3 v_diff = (target - spitPos);
                            //float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                            ////m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
                            //m_projectileLauncher.AimAt(target);
                            //m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimatio2n.animation);
                            StartCoroutine(AttackRoutineOne());

                            break;
                        case Attack.Attack2:
                            //if (isFirstEncounter)
                            //    StartCoroutine(SurpriseAttack());

                            //isFirstEncounter = false;
                            //m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimatio2n.animation);
                            StartCoroutine(AttackRoutineTwo());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimatio2n.animation, true);
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
                        Debug.Log(IsFacingTarget() + " facing target");
                        if (IsFacingTarget())
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                            {
                                m_attackDecider.DecideOnAttack();
                                if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                                {
                                    m_stateHandle.SetState(State.Attacking);
                                }
                                else
                                {
                                    //if(m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation)
                                    //{
                                    //    m_animation.SetAnimation(0, m_info.surpriseAnimation, false);
                                    //    m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                                    //}
                                    m_attackDecider.hasDecidedOnAttack = false;

                                    Debug.Log(isFirstEncounter + "else");
                                    m_stateHandle.SetState(State.ReevaluateSituation);
                                    // m_animation.SetAnimation(0, m_info.idleAnimatio2n.animation, true);
                                }
                            }
                            //else
                            //{
                            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            //}
                        }
                        else
                        {
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
                        m_stateHandle.SetState(State.Burrowed);
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
            m_stateHandle.OverrideState(State.Burrowed);
            m_currentPatience = 0;
            m_enablePatience = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_enablePatience = false;
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
