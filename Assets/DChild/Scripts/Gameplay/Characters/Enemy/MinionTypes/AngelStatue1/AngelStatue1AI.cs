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
            private SimpleAttackInfo m_attackRight = new SimpleAttackInfo();
            public SimpleAttackInfo attackRight => m_attackRight;
            [SerializeField]
            private SimpleAttackInfo m_tripleAttackLeft = new SimpleAttackInfo();
            public SimpleAttackInfo tripleAttackLeft => m_tripleAttackLeft;
            [SerializeField]
            private SimpleAttackInfo m_tripleAttackRight = new SimpleAttackInfo();
            public SimpleAttackInfo tripleAttackRight => m_tripleAttackRight;
            [SerializeField]
            private SimpleAttackInfo m_wingAttack = new SimpleAttackInfo();
            public SimpleAttackInfo wingAttack => m_wingAttack;

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
                m_tripleAttackLeft.SetData(m_skeletonDataAsset);
                m_tripleAttackRight.SetData(m_skeletonDataAsset);
                m_wingAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_awakenAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);

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
            TripleAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_leftWing;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_rightWing;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("ProjectileInfo")]
        private List<Transform> m_projectilePointsLeft;
        [SerializeField, TabGroup("ProjectileInfo")]
        private List<Transform> m_projectilePointsRight;
        [SerializeField, TabGroup("ProjectileInfo")]
        private List<Transform> m_projectileTargetsLeft;
        [SerializeField, TabGroup("ProjectileInfo")]
        private List<Transform> m_projectileTargetsRight;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;


        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private ProjectileLauncher m_projectileLauncherLeft;
        private ProjectileLauncher m_projectileLauncherRight;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;


        private State m_turnState;
        private string m_attackLeftAnimation;
        private string m_attackRightAnimation;


        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.m_autoFlinch = true;
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
                m_flinchHandle.m_autoFlinch = true;
                m_stateHandle.SetState(State.Idle);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            m_leftWing.SetActive(false);
            m_rightWing.SetActive(false);
            m_selfCollider.SetActive(false);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            base.OnDestroyed(sender, eventArgs);
            GameplaySystem.minionManager.Unregister(this);
            m_movement.Stop();
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(1, 0);
            StartCoroutine(DeathRoutine());

        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathAnimation, false).MixDuration = 0;
            yield return new WaitForSeconds(10);
            m_animation.animationState.TimeScale = 0;
            enabled = false;
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.damageAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                    m_animation.SetEmptyAnimation(0, 0);
                m_selfCollider.SetActive(false);
                m_stateHandle.ApplyQueuedState();
            }
        }


        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }
        private void UpdateAttackDeciderList()
        {
            Debug.Log("Apply Attacker Data");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attackLeft.range),
                                    new AttackInfo<Attack>(Attack.TripleAttack, m_info.tripleAttackLeft.range)
                                  );
            m_attackDecider.hasDecidedOnAttack = false;
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

        private void ExecuteAttack(Attack m_attack,String direction)
        {
            m_flinchHandle.m_autoFlinch = false;
            switch (m_attack)
            {
                
                case Attack.Attack:
                    m_animation.EnableRootMotion(true, false);
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    //LaunchProjectile();
                    StartCoroutine(DaggerAttackRoutine(direction));
                    m_attackDecider.hasDecidedOnAttack = true;
                    break;
                case Attack.TripleAttack:
                    m_animation.EnableRootMotion(true, false);
                    //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    //LaunchProjectile();
                    StartCoroutine(TripleDaggerAttackRoutine(direction));
                    m_attackDecider.hasDecidedOnAttack = true;
                    break;
            }
        }
        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.awakenAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
        private IEnumerator DaggerAttackRoutine(string attack)
        {
            if (attack == "left")
            {
                m_animation.SetAnimation(0, m_info.attackLeft.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackLeft.animation);
                m_animation.SetEmptyAnimation(0, 0);
                LaunchProjectileLeft();
            }
            if (attack == "right")
            {
                m_animation.SetAnimation(0, m_info.attackRight.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackRight.animation);
                m_animation.SetEmptyAnimation(0, 0);
                LaunchProjectileRight();
            }
            m_movement.Stop();
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator TripleDaggerAttackRoutine(string attack)
        {
            if (attack == "left")
            {
                m_animation.SetAnimation(0, m_info.tripleAttackLeft.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tripleAttackLeft.animation);
                m_animation.SetEmptyAnimation(0, 0);
                LaunchtripleProjectileLeft();
            }
            if (attack == "right")
            {
                m_animation.SetAnimation(0, m_info.tripleAttackRight.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tripleAttackRight.animation);
                m_animation.SetEmptyAnimation(0, 0);
                LaunchtripleProjectileRight();
            }
            m_movement.Stop();
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator WingAttackRoutine()
        {
            m_leftWing.SetActive(true);
            m_rightWing.SetActive(true);
            m_animation.SetAnimation(0, m_info.wingAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wingAttack.animation);
            m_animation.SetEmptyAnimation(0, 0);
            m_movement.Stop();
            m_selfCollider.SetActive(false);
            m_flinchHandle.m_autoFlinch = true;
            m_leftWing.SetActive(false);
            m_rightWing.SetActive(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private void LaunchProjectileLeft()
        {

            
                m_projectileLauncherLeft = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsLeft[0]);
                m_projectileLauncherLeft.AimAt(m_targetInfo.position);
                m_projectileLauncherLeft.LaunchProjectile();


        }

        private void LaunchProjectileRight()
        {

         
                m_projectileLauncherRight = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsRight[0]);
                m_projectileLauncherRight.AimAt(m_targetInfo.position);
                m_projectileLauncherRight.LaunchProjectile();


        }
        private void LaunchtripleProjectileLeft()
        {
          
                for (int i = 0; i < m_projectilePointsLeft.Count; i++)
                {
                    m_projectileLauncherLeft = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsLeft[i]);
                    m_projectileLauncherLeft.AimAt(m_projectileTargetsLeft[i].position);
                    m_projectileLauncherLeft.LaunchProjectile();
                }
    
        }

        private void LaunchtripleProjectileRight()
        {

            for (int i = 0; i < m_projectilePointsRight.Count; i++)
            {
                m_projectileLauncherRight = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsRight[i]);
                m_projectileLauncherRight.AimAt(m_projectileTargetsRight[i].position);
                m_projectileLauncherRight.LaunchProjectile();
            }

        }

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_leftWing.SetActive(false);
            m_rightWing.SetActive(false);
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchtripleProjectileLeft);
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchtripleProjectileRight);
            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            GameplaySystem.minionManager.Register(this);
            m_attackHandle.AttackDone += OnAttackDone;
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_projectileLauncherLeft = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsLeft[0]);
            m_projectileLauncherLeft.SetProjectile(m_info.projectile.projectileInfo);
            m_projectileLauncherRight = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePointsRight[0]);
            m_projectileLauncherRight.SetProjectile(m_info.projectile.projectileInfo);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Attack, Attack.TripleAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attackLeft.range, m_info.tripleAttackLeft.range);
            m_attackUsed = new bool[m_attackCache.Count];
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
                    if (IsTargetInRange(m_info.wingAttack.range))
                    {
                        m_flinchHandle.m_autoFlinch = false;
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(WingAttackRoutine());
                    }
                    else
                    {
                        if (IsTargetInRange(m_info.attackLeft.range))
                        {
                            ChooseAttack();
                            m_flinchHandle.m_autoFlinch = false;
                            m_stateHandle.Wait(State.Cooldown);

                            if (m_targetInfo.position.x < transform.position.x)
                            {
                                ExecuteAttack( m_currentAttack, "left");
                                m_attackDecider.hasDecidedOnAttack = false;

                            }
                            else
                            {
                                ExecuteAttack(m_currentAttack, "right");
                                m_attackDecider.hasDecidedOnAttack = false;

                            }
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
