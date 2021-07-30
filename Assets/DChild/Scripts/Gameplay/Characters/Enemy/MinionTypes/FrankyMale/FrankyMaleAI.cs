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
using System.Linq;
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/FrankyMale")]
    public class FrankyMaleAI : CombatAIBrain<FrankyMaleAI.Info>, IResetableAIBrain, IAmbushingAI
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
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
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
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_appearAnimation;
            public string appearAnimation => m_appearAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_disappearAnimation;
            public string disappearAnimation => m_disappearAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_dormantAnimation;
            public string dormantAnimation => m_dormantAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_awakenAnimation;
            public string awakenAnimation => m_awakenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallAnimation;
            public string fallAnimation => m_fallAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_landAnimation;
            public string landAnimation => m_landAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;

            public SimpleProjectileAttackInfo projectile => m_projectile;
            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_attack1Event;
            public string attack1Event => m_attack1Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_attack2Event;
            public string attack2Event => m_attack2Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_teleportEvent;
            public string teleportEvent => m_teleportEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Dormant,
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
            Attack1,
            Attack2,
            //Attack3_2,
            [HideInInspector]
            _COUNT
        }


        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_teleportFX;

        [SerializeField, TabGroup("Spawn Points")]
        private List<Collider2D> m_randomSpawnColliders;

        [SerializeField]
        private bool m_willPatrol;

        [SerializeField]
        private GameObject m_projectilePoint;
        [SerializeField]
        private GameObject m_targetPointIK;

        private Vector2 m_lastTargetPos;
        private Vector2 m_initialPos;

        private ProjectileLauncher m_projectileLauncher;

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
            m_animation.animationState.TimeScale = 1;
            m_stateHandle.ApplyQueuedState();
            //StartCoroutine(TeleportRoutine());
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        private void CustomTurn()
        {
            if (!IsFacingTarget())
            {
                //m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (!TargetBlocked() && !m_enablePatience)
                {
                    m_selfCollider.SetActive(true);

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
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Environment", "Player"));
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }
        private void Patience()
        {
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }

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
                m_turnState = State.ReevaluateSituation;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    m_stateHandle.SetState(State.Turning);
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
            m_selfCollider.SetActive(false);
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

        private IEnumerator SneerRoutine()
        {
            m_movement.Stop();
            while (true)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        private IEnumerator TeleportRoutine()
        {
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            m_hitbox.Disable();
            //var randomOffset = 50 * UnityEngine.Random.Range(0, 2) == 1 ? -1 : 1;
            //while (Vector2.Distance(m_targetInfo.position, transform.position) < 25f)
            //{
            //    yield return null;
            //}
            if (m_targetInfo.isValid)
            {
                transform.position = new Vector2(RandomTeleportPoint(transform.position).x /*+ randomOffset*/, GroundPosition().y);
            }
            else
            {
                transform.position = m_initialPos;
            }
            yield return new WaitForSeconds(1f);
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        private Vector3 RandomTeleportPoint(Vector3 transformPos)
        {
            Vector3 randomPos = transformPos;
            List<float> m_targetDistances = new List<float>();
            for (int i = 0; i < m_randomSpawnColliders.Count; i++)
            {
                m_targetDistances.Add(Vector2.Distance(m_targetInfo.position, m_randomSpawnColliders[i].bounds.center));
            }
            Collider2D m_chosenSpawnBox = new Collider2D();
            for (int i = 0; i < m_randomSpawnColliders.Count; i++)
            {
                Debug.Log("randomCollider " + Vector2.Distance(m_targetInfo.position, m_randomSpawnColliders[i].bounds.center));
                Debug.Log("targetDistance " + m_targetDistances.Min());
                if (Mathf.Abs(Vector2.Distance(m_targetInfo.position, m_randomSpawnColliders[i].bounds.center) - m_targetDistances.Min()) < 5f)
                {
                    m_chosenSpawnBox = m_randomSpawnColliders[i];
                }
            }
            while (/*!m_chosenSpawnBox.IsTouching(m_selfCollider.GetComponent<Collider2D>()) &&*/ Vector2.Distance(transformPos, randomPos) < UnityEngine.Random.Range(25f, 50f)
                /*&& Vector2.Distance(m_targetInfo.position, transform.position) <= UnityEngine.Random.Range(10f, 20f)*/)
            {
                randomPos = m_chosenSpawnBox.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_chosenSpawnBox.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_chosenSpawnBox.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_chosenSpawnBox.bounds.size.z);
            }
            return randomPos;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_animation.SetEmptyAnimation(0, 0);
                //m_stateHandle.ApplyQueuedState();
                StartCoroutine(TeleportRoutine());
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)/*,
                                    new AttackInfo<Attack>(Attack.Attack3_2, m_info.attack3_2.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_character.physics.simulateGravity = true;
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.dormantAnimation)
            {
                m_animation.EnableRootMotion(true, true);
                m_animation.SetAnimation(0, m_info.awakenAnimation, false);
                //m_animation.AddAnimation(0, m_info.idleAnimation, false, 0)/*.TimeScale = 5f*/;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true).MixDuration = 0;
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                //yield return new WaitForSeconds(0.5f);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                //if (!IsFacingTarget())
                //{
                //    CustomTurn();
                //}
                //m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().simulateGravity = m_attackDecider.chosenAttack.attack != Attack.Attack3 ? true : false;
                //m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().simulateGravity = false;
                m_targetPointIK.transform.position = m_lastTargetPos;
                m_projectileLauncher.AimAt(m_lastTargetPos);
                m_projectileLauncher.LaunchProjectile();
                //if (m_chosenAttack != Attack.Attack3)
                //{
                //    //ShootProjectile();
                //    m_projectileLauncher.AimAt(m_lastTargetPos);
                //    m_projectileLauncher.LaunchProjectile();
                //}
                //else
                //{
                //    m_info.projectile.projectileInfo.projectile.transform.localScale = transform.localScale;
                //    m_projectileLauncher.AimAt(new Vector2(m_shootStraight.position.x, m_projectileStart.position.y - GroundDistance()));
                //    m_projectileLauncher.LaunchProjectile();
                //}
            }
        }

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);

            m_spineEventListener.Subscribe(m_info.attack1Event, LaunchProjectile);
            m_spineEventListener.Subscribe(m_info.attack2Event, LaunchProjectile);
            m_spineEventListener.Subscribe(m_info.teleportEvent, m_teleportFX.Play);
            
            m_character.physics.simulateGravity = m_willPatrol ? true : false;
            //m_aggroCollider.enabled = m_willPatrol ? true : false;
            if (m_willPatrol)
            {
                m_hitbox.Enable();
                m_animation.DisableRootMotion();
            }
            m_initialPos = new Vector2(transform.position.x, GroundPosition().y);
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
            //m_randomSpawnColliders = new List<Collider2D>();
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectilePoint.transform);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Dormant, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Dormant:
                    m_animation.SetAnimation(0, m_info.dormantAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (Vector2.Distance(m_initialPos, transform.position) >= 100)
                    {
                        StartCoroutine(TeleportRoutine());
                        return;
                    }
                    if (m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
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
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                            break;
                        case Attack.Attack2:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
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

                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting && !TargetBlocked())
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_movement.Stop();
                                m_stateHandle.Wait(State.ReevaluateSituation);
                                StartCoroutine(TeleportRoutine());
                            }
                        }
                        else
                        {
                            var xDistance = Mathf.Abs(m_targetInfo.position.x - transform.position.x);
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation && xDistance >= 3)
                            {
                                m_stateHandle.SetState(State.Turning);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                    }
                    break;

                case State.ReevaluateSituation:
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
                    if (m_targetInfo.isValid)
                    {
                        if (IsFacing(m_lastTargetPos))
                        {
                            m_targetPointIK.transform.position = m_lastTargetPos;
                        }
                        else
                        {
                            if (m_attackDecider.hasDecidedOnAttack)
                            {
                                CustomTurn();
                            }
                        }
                    }
                    return;
            }

            if (m_targetInfo.isValid)
            {
                if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.targetDistanceTolerance)
                {
                    Patience();
                }
                else
                {
                    if (!TargetBlocked())
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
                            //m_stateHandle.OverrideState(State.ReevaluateSituation);
                        }
                    }
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(false);
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }

        public void LaunchAmbush(Vector2 position)
        {
            enabled = true;
            m_aggroCollider.enabled = true;
            //m_stateHandle.OverrideState(State.Detect);
        }

        public void PrepareAmbush(Vector2 position)
        {
            enabled = false;
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Dormant);
        }
    }
}
