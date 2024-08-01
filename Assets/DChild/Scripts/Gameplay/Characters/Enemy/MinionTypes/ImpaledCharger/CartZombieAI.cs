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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/CartZombie")]
    public class CartZombieAI : CombatAIBrain<CartZombieAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveBackwards = new MovementInfo();
            public MovementInfo moveBackwards => m_moveBackwards;

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
            private BasicAnimationInfo m_struggleAnimation;
            public BasicAnimationInfo struggleAnimation => m_struggleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_anticipationAnimation;
            public BasicAnimationInfo anticipationAnimation => m_anticipationAnimation;
            [SerializeField]
            private BasicAnimationInfo m_getOffAnimation;
            public BasicAnimationInfo getOffAnimation => m_getOffAnimation;
            [SerializeField]
            private BasicAnimationInfo m_impactAnimation;
            public BasicAnimationInfo impactAnimation => m_impactAnimation;
            [SerializeField]
            private BasicAnimationInfo m_cartStopAnimation;
            public BasicAnimationInfo cartStopAnimation => m_cartStopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathPusherAnimation;
            public BasicAnimationInfo deathPusherAnimation => m_deathPusherAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveBackwards.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_struggleAnimation.SetData(m_skeletonDataAsset);
                m_anticipationAnimation.SetData(m_skeletonDataAsset);
                m_getOffAnimation.SetData(m_skeletonDataAsset);
                m_impactAnimation.SetData(m_skeletonDataAsset);
                m_cartStopAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_deathPusherAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Returning,
            Standby,
            //Turning,
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
        private GameObject m_boundingBox;
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
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
        private float m_chargeDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_initialPos;
        private bool m_isPusherDead;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_playerSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_breakSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_dustStartFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_dustFrontFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_dustBackFX;

        [SerializeField, TabGroup("Pusher")]
        private ImpaledChargerAI m_chargerAI;
        [SerializeField, TabGroup("Pusher")]
        private Character m_chargerCharacter;
        [SerializeField, TabGroup("Pusher")]
        private Hitbox m_chargerHitbox;
        [SerializeField, TabGroup("Pusher")]
        private MovementHandle2D m_chargerMovement;
        [SerializeField, TabGroup("Pusher")]
        private List<RaySensorFaceRotator> m_rotators;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomTurnRoutine;
        private Coroutine m_playerStepRoutine;

        public event EventAction<EventActionArgs> IsDead;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => /*m_stateHandle.SetState(State.Turning)*/ CustomTurn();

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

        public void SetAI(AITargetInfo targetInfo, float chargeDuration)
        {
            m_targetInfo = targetInfo;
            m_chargeDuration = chargeDuration;
            m_isDetecting = true;
            m_stateHandle.OverrideState(State.Detect);
            m_chargerAI?.OverrideState(ImpaledChargerAI.State.Detect);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_chargerCharacter.SetFacing(m_character.facing);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            if (!m_isPusherDead)
            {
                for (int i = 0; i < m_rotators.Count; i++)
                {
                    m_rotators[i].AlignRotationToFacing(eventArgs.currentFacingDirection);
                }
            }
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            m_chargerCharacter.SetFacing(m_character.facing);

            float xScale = 1f;
            if (m_chargerCharacter.facing == HorizontalDirection.Right && m_character.facing == HorizontalDirection.Right)
            {
                xScale = 1;
            }
            else if (m_chargerCharacter.facing == HorizontalDirection.Right && m_character.facing == HorizontalDirection.Left)
            {
                xScale = -1;
            }
            else if (m_chargerCharacter.facing == HorizontalDirection.Left && m_character.facing == HorizontalDirection.Left)
            {
                xScale = 1;
            }
            else if (m_chargerCharacter.facing == HorizontalDirection.Left && m_character.facing == HorizontalDirection.Right)
            {
                xScale = -1;
            }
            m_chargerAI.transform.localScale = new Vector3(xScale, 1, 1);
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
                    //m_turnState = State.WaitBehaviourEnd;
                    //m_stateHandle.SetState(State.Turning);
                    CustomTurn();
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
            m_hitbox.Disable();
            m_boundingBox.SetActive(false);
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            
            this.transform.SetParent(null);
            IsDead?.Invoke(this, new EventActionArgs());
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            m_dustBackFX.Stop();
            m_dustFrontFX.Stop();
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            m_chargerMovement?.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState == State.Cooldown && m_attackRoutine == null)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(!m_isPusherDead ? State.Cooldown : State.Idle);
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

        private void ChargerIsDead(object sender, EventActionArgs eventArgs)
        {
            m_isPusherDead = true;
            m_chargerAI.transform.SetParent(null);
            m_chargerMovement = null;
            m_hitbox.Enable();
            m_dustBackFX.Stop();
            m_dustFrontFX.Stop();
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Idle);
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

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        private IEnumerator DetectRoutine()
        {

            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.ApplyQueuedState();
            m_attackRoutine = StartCoroutine(AttackRoutine());
            yield return null;
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
                //m_turnState = State.Idle;
                //m_stateHandle.SetState(State.Turning);
                CustomTurn();
                //m_cartAI.TurnCart(CartZombieAI.State.Idle);
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_chargerMovement?.Stop();
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
            //m_animation.SetAnimation(0, m_info.detectAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_chargerAI.transform.localPosition = new Vector2(-6.5f, 0);
            m_animation.SetAnimation(0, m_info.anticipationAnimation, false);
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.85f;
            yield return new WaitForSeconds(waitTime);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.anticipationAnimation);
            m_dustStartFX.Play();
            m_dustStartFX.transform.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? -1 : 1, 1, 1);
            m_dustFrontFX.Play();
            m_dustFrontFX.transform.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? 1 : -1, 1, 1);
            m_dustBackFX.Play();
            m_dustBackFX.transform.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? 1 : -1, 1, 1);
            m_animation.SetAnimation(0, m_info.move.animation, true);
            m_playerStepRoutine = StartCoroutine(PlayerSteppedOnRoutine());
            float time = 0;
            while (time < m_chargeDuration&& m_edgeSensor.isDetecting)
            {
                time += Time.deltaTime;
                m_chargerAI.transform.localPosition = new Vector2(-6.5f, 0);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                m_chargerMovement?.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                yield return new WaitForSeconds(0.1f);
                if (m_breakSensor.isDetecting || !m_edgeSensor.isDetecting)
                {
                    time = m_chargeDuration;
                }
                yield return null;
            }
            StopCoroutine(m_playerStepRoutine);
            m_playerStepRoutine = null;
            m_dustStartFX.Play();
            m_dustFrontFX.Stop();
            m_dustBackFX.Stop();
            m_animation.SetAnimation(0, m_info.cartStopAnimation, false);
            if (m_wallSensor.isDetecting || !m_edgeSensor.isDetecting)
            {
                m_movement.Stop();
                m_chargerMovement?.Stop();
            }
            else
            {
                StartCoroutine(RecoilStopRoutine());
            }
            //m_movement.Stop();
            //m_chargerMovement?.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.cartStopAnimation);
            m_animation.SetAnimation(0, m_info.getOffAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.getOffAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackRoutine = null;
            m_stateHandle.ApplyQueuedState();
            m_chargerAI?.OverrideState(ImpaledChargerAI.State.Cooldown);
            yield return null;
        }

        private IEnumerator RecoilStopRoutine()
        {
            var moveSpeed = m_info.move.speed;
            while (moveSpeed > 0f)
            {
                if (m_wallSensor.isDetecting || !m_edgeSensor.isDetecting)
                {
                    moveSpeed = 0f;
                }
                else
                {
                    moveSpeed = moveSpeed - (125 * Time.deltaTime);
                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, moveSpeed);
                    m_chargerMovement?.MoveTowards(Vector2.one * transform.localScale.x, moveSpeed);
                }
                yield return null;
            }
            m_movement.Stop();
            m_chargerMovement?.Stop();
            yield return null;
        }

        private IEnumerator PlayerSteppedOnRoutine()
        {
            while (true)
            {
                if (m_playerSensor.isDetecting)
                {
                    //m_character.physics.AddForce(new Vector2(0, 100f), ForceMode2D.Force);
                    //m_chargerCharacter.physics.AddForce(new Vector2(0, 100f), ForceMode2D.Force);
                    m_animation.SetAnimation(0, m_info.impactAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.impactAnimation);
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                }
                yield return null;
            }
        }

        protected override void Start()
        {
            base.Start();

            m_initialPos = new Vector2(transform.position.x, GroundPosition().y);
            m_hitbox.SetInvulnerability(Invulnerability.Level_1);

        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            //m_turnHandle.TurnDone += OnTurnDone;
            m_chargerCharacter.CharacterTurn += OnCharacterTurn;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            if (m_chargerAI != null)
            {
                m_chargerAI.IsDead += ChargerIsDead;
            }
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_chargerMovement?.Stop();

                    StopCoroutine(m_randomTurnRoutine);

                    //if (!IsFacingTarget() && !m_isPusherDead)
                    //{
                    //    m_turnState = State.Detect;
                    //    m_stateHandle.SetState(State.Turning);
                    //    return;
                    //}

                    m_stateHandle.Wait(State.Cooldown);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    m_movement.Stop();
                    if (!m_isPusherDead)
                    {
                        m_chargerMovement?.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.struggleAnimation, true);
                    }
                    break;

                case State.Returning:
                    if (Vector2.Distance(m_initialPos, transform.position) > 2f)
                    {
                        m_animation.SetAnimation(0, IsFacing(m_initialPos) ? m_info.move.animation : m_info.moveBackwards.animation, true);
                        if (!m_isPusherDead)
                        {
                            m_movement.MoveTowards(Vector2.one * transform.localScale.x, IsFacing(m_initialPos) ? m_info.move.speed : m_info.moveBackwards.speed);
                        }
                    }
                    else
                    {
                        m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
                        m_stateHandle.OverrideState(State.Idle);
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackMelee:
                            //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                            m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //if (!IsFacingTarget())
                    //{
                    //    if (!m_isPusherDead)
                    //    {
                    //        m_turnState = State.Cooldown;
                    //        m_stateHandle.SetState(State.Turning);
                    //    }
                    //}
                    //else
                    //{
                    //}
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                        m_chargerAI?.OverrideState(ImpaledChargerAI.State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    {
                        if (IsTargetInRange(m_info.attack.range) && !m_breakSensor.allRaysDetecting && m_edgeSensor.allRaysDetecting)
                        {
                            m_movement.Stop();
                            m_chargerMovement?.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.SetState(State.Attacking);
                            m_chargerAI?.OverrideState(ImpaledChargerAI.State.Attacking);
                        }
                        else
                        {
                            m_animation.EnableRootMotion(false, false);
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                m_chargerMovement?.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                            }
                            else
                            {
                                m_movement.Stop();
                                //m_model.localPosition = new Vector2(0, 0);
                                m_chargerMovement?.Stop();
                                if (m_animation.animationState.GetCurrent(0).IsComplete)
                                {
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        //if (IsFacingTarget())
                        //{
                        //}
                        //else
                        //{
                        //    m_turnState = State.ReevaluateSituation;
                        //    m_stateHandle.SetState(State.Turning);
                        //}
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Attacking);
                        m_chargerAI?.OverrideState(ImpaledChargerAI.State.Attacking);
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

            //if (m_targetInfo.isValid)
            //{
            //    if (IsFacingTarget())
            //    {
            //        m_hitbox.Enable();
            //        m_chargerHitbox.Disable();
            //    }
            //    else
            //    {
            //        m_hitbox.Disable();
            //        m_chargerHitbox.Enable();
            //    }
            //}

            if (m_isPusherDead && m_enablePatience && m_stateHandle.currentState != State.Standby)
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
            transform.position = m_initialPos;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
