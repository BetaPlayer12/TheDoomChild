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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/ImpaledCharger")]
    public class ImpaledChargerAI : CombatAIBrain<ImpaledChargerAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveSolo = new MovementInfo();
            public MovementInfo moveSolo => m_moveSolo;
            [SerializeField]
            private MovementInfo m_moveBackwards = new MovementInfo();
            public MovementInfo moveBackwards => m_moveBackwards;

            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_chargeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chargeAttack => m_chargeAttack;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_chargeAttackDuration;
            public float chargeAttackDuration => m_chargeAttackDuration;
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
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleSoloAnimation;
            public string idleSoloAnimation => m_idleSoloAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_cartStopAnimation;
            public string cartStopAnimation => m_cartStopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_getOffAnimation;
            public string getOffAnimation => m_getOffAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_impactAnimation;
            public string impactAnimation => m_impactAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_anticipationAnimation;
            public string anticipationAnimation => m_anticipationAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBothAnimation;
            public string flinchBothAnimation => m_flinchBothAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_turnAnimation;
            //public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathWithCartAnimation;
            public string deathWithCartAnimation => m_deathWithCartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathCartOnlyAnimation;
            public string deathCartOnlyAnimation => m_deathCartOnlyAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveSolo.SetData(m_skeletonDataAsset);
                m_moveBackwards.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_chargeAttack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        public enum State
        {
            Detect,
            Idle,
            Returning,
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
            ChargeAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Transform m_model;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_solidCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroCollider;
        [SerializeField, TabGroup("Reference")]
        private List<RaySensorFaceRotator> m_rotators;
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private bool m_isCartDead;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_initialPos;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_playerSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cartPlayerSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cartWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cartGroundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cartEdgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cartBreakSensor;

        [SerializeField, TabGroup("Adutukat")]
        private CartZombieAI m_cartAI;
        [SerializeField, TabGroup("Adutukat")]
        private Transform m_cartModel;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_playerStepRoutine;

        public event EventAction<EventActionArgs> IsDead;

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
                if (!TargetBlocked() && !m_enablePatience)
                {
                    m_selfCollider.SetActive(true);

                    if (!m_isDetecting)
                    {
                        m_isDetecting = true;
                        m_cartAI.SetAI(m_targetInfo, m_info.chargeAttackDuration);
                        if (m_isCartDead)
                        {
                            m_stateHandle.SetState(State.Detect);
                        }
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

        public void OverrideState(State state)
        {
            m_stateHandle.OverrideState(state);
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

        //private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        //{
        //    m_stateHandle.ApplyQueuedState();
        //}

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            //transform.localScale = new Vector3(transform.localScale.x * m_cartAI.transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
                if (!IsFacingTarget() && m_isCartDead)
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
                    //m_cartAI.TurnCart(CartZombieAI.State.WaitBehaviourEnd);
                    return;
                }

                if (m_sneerRoutine == null)
                {
                    m_sneerRoutine = StartCoroutine(SneerRoutine());
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
            m_selfCollider.SetActive(false);
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
            this.transform.SetParent(null);
            m_hitbox.Disable();
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
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState == State.Cooldown && m_attackRoutine == null)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.Cooldown);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.ApplyQueuedState();
            }
        }

        private void CartIsDead(object sender, EventActionArgs eventArgs)
        {
            m_isCartDead = true;
            this.transform.SetParent(null);
            m_solidCollider.SetActive(false);
            m_hitbox.Enable();
            StopAllCoroutines();
            switch (m_character.facing)
            {
                case HorizontalDirection.Left:
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case HorizontalDirection.Right:
                    transform.localScale = Vector3.one;
                    break;
            }
            for (int i = 0; i < m_rotators.Count; i++)
            {
                m_rotators[i].AlignRotationToFacing(m_character.facing);
            }
            StartCoroutine(CartDiesRoutine());
        }

        private IEnumerator CartDiesRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.deathCartOnlyAnimation, false).TimeScale = 0.5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathCartOnlyAnimation);
            m_animation.SetAnimation(0, m_info.idleSoloAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
            m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);
            m_attackRoutine = StartCoroutine(AttackRoutine());
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            if (!m_isCartDead)
            {
                if (!IsFacing(m_cartModel.position))
                {
                    CustomTurn();
                }
                m_animation.SetAnimation(0, m_info.chargeAttack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeAttack.animation);
                m_playerStepRoutine = StartCoroutine(PlayerSteppedOnRoutine());
            }
            m_animation.SetAnimation(0, !m_isCartDead ? m_info.move.animation : m_info.attack.animation, true);
            float time = 0;
            while (time < (!m_isCartDead ? m_info.chargeAttackDuration : 0.65f))
            {
                time += Time.deltaTime;
                if (m_isCartDead)
                {
                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.moveSolo.speed);
                }
                yield return new WaitForSeconds(!m_isCartDead ? 0.1f : 0);
                if (m_isCartDead ? m_wallSensor.isDetecting || !m_edgeSensor.isDetecting : m_cartBreakSensor.isDetecting || !m_cartEdgeSensor.isDetecting)
                {
                    time = m_info.chargeAttackDuration;
                }
                yield return null;
            }
            if (!m_isCartDead)
            {
                StopCoroutine(m_playerStepRoutine);
                m_playerStepRoutine = null;
                m_animation.SetAnimation(0, m_info.cartStopAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.cartStopAnimation);
                m_animation.SetAnimation(0, m_info.getOffAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.getOffAnimation);
            }
            else
            {
                m_movement.Stop();
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            }
            m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);
            if (m_isCartDead)
            {
                m_stateHandle.ApplyQueuedState();
            }
            yield return null;
        }

        private IEnumerator PlayerSteppedOnRoutine()
        {
            while (true)
            {
                if (m_cartPlayerSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.impactAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.impactAnimation);
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                }
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_isCartDead)
            {
                m_movement.Stop();
            }
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

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_hitbox.SetInvulnerability(Invulnerability.Level_1);

            //m_animation.EnableRootMotion(true, false);
            m_initialPos = new Vector2(transform.position.x, GroundPosition().y);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            //m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_cartAI.IsDead += CartIsDead;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_isCartDead)
                    {
                        m_movement.Stop();
                    }

                    if (!IsFacingTarget() /*&& m_isCartDead*/)
                    {
                        //m_turnState = State.Detect;
                        //m_stateHandle.SetState(State.Turning);
                        CustomTurn();
                        //m_cartAI.TurnCart(CartZombieAI.State.Detect);
                    }

                    m_stateHandle.Wait(State.Cooldown);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);
                    if (m_isCartDead)
                    {
                        m_movement.Stop();
                    }
                    break;

                //case State.Turning:
                //    m_stateHandle.Wait(m_turnState);
                //    m_turnHandle.Execute();
                //    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    switch (m_isCartDead)
                    {
                        case true:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;

                        case false:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                    }

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget() /*&& m_isCartDead*/)
                    {
                        //m_turnState = State.Cooldown;
                        //m_stateHandle.SetState(State.Turning);
                        CustomTurn();
                    }
                    m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        if (m_isCartDead)
                        {
                            m_stateHandle.OverrideState(State.ReevaluateSituation);
                        }
                        //else
                        //{
                        //    m_stateHandle.Wait(State.ReevaluateSituation);
                        //}
                    }

                    break;

                case State.Chasing:
                    {
                        if (!IsFacingTarget() && m_isCartDead)
                        {
                            //m_turnState = State.ReevaluateSituation;
                            //m_stateHandle.SetState(State.Turning);
                            CustomTurn();
                        }

                        var attackRange = m_isCartDead ? m_info.attack.range : m_info.chargeAttack.range;
                        if (IsTargetInRange(attackRange) && m_isCartDead ? !m_wallSensor.allRaysDetecting : !m_cartWallSensor.allRaysDetecting)
                        {
                            if (m_isCartDead)
                            {
                                m_stateHandle.SetState(State.Attacking);
                                m_movement.Stop();
                            }
                            m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);
                        }
                        else
                        {
                            if (m_isCartDead ? !m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting : !m_cartWallSensor.isDetecting && m_cartGroundSensor.isDetecting && m_cartEdgeSensor.isDetecting)
                            {
                                m_animation.SetAnimation(0, m_isCartDead ? m_info.moveSolo.animation : m_info.move.animation, true);
                                if (m_isCartDead)
                                {
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.moveSolo.speed);
                                }
                                //m_model.localPosition = new Vector2(4.5f, 0);
                            }
                            else
                            {
                                if (m_isCartDead)
                                {
                                    m_movement.Stop();
                                }
                                if (m_animation.animationState.GetCurrent(0).IsComplete)
                                {
                                    m_animation.SetAnimation(0, !m_isCartDead ? m_info.idleAnimation : m_info.idleSoloAnimation, true);
                                }
                            }
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

            if (m_targetInfo.isValid)
            {
                if (TargetBlocked() && Vector2.Distance(m_targetInfo.position, transform.position) > m_info.chargeAttack.range)
                {
                    StopAllCoroutines();
                    m_selfCollider.SetActive(false);
                    m_enablePatience = false;
                    m_targetInfo.Set(null, null);
                    m_isDetecting = false;
                    if (m_sneerRoutine != null)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    if (m_patienceRoutine != null)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }
                    if (m_isCartDead)
                    {
                        m_stateHandle.OverrideState(State.Returning);
                    }
                    return;
                }

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
                        }
                    }
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
            m_selfCollider.SetActive(false);
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Detect);
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
