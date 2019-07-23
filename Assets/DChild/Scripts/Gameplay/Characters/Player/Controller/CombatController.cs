using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Inputs;
using UnityEngine;
using Spine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class CombatController : MonoBehaviour, IBasicAttackController, IProjectileThrowController
    {
        [SerializeField]
        private CountdownTimer m_attackIdleDuration;

        private IBehaviourState m_behaviourState;
        private ICombatState m_combatState;
        private IHighJumpState m_highjumpState;
        private IIsolatedTime m_time;

        private CharacterPhysics2D m_physics;
        private CombatEventArgs m_combatEventArgs;
        private bool m_hasAttacked;

        public event EventAction<CombatEventArgs> BasicAttackCall;
        public event EventAction<CombatEventArgs> JumpAttackUpwardCall;
        public event EventAction<CombatEventArgs> JumpAttackDownwardCall;
        public event EventAction<CombatEventArgs> JumpAttackForwardCall;
        public event EventAction<EventActionArgs> ProjectileAimCall;
        public event EventAction<ControllerEventArgs> ProjectileAimUpdate;
        public event EventAction<CombatEventArgs> UpwardAttackCall;

        public bool isMainHandPressed { get; set; }
        public bool isOffHandPressed { get; set; }

        public void Initialize(CharacterPhysics2D physics, IFacing facing,  IBehaviourState behaviourState, ICombatState combatState, IHighJumpState highjumpState, IIsolatedTime time)
        {
            m_time = time;

            m_physics = physics;
            m_combatEventArgs = new CombatEventArgs(facing);
            m_behaviourState = behaviourState;
            m_combatState = combatState;
            m_highjumpState = highjumpState;
        }

        public void CallUpdate(IPlayerState state, ControllerEventArgs eventArgs)
        {
            if (m_hasAttacked) m_attackIdleDuration.Tick(m_time.deltaTime);

            if (state.canAttack)
            {
                //if (m_behaviourState.waitForBehaviour)
                //{
                //    m_behaviourState.waitForBehaviour = false;
                //}

                if (state.isDashing)
                {

                }

                else if (state.isStickingToWall)
                {

                }

                else
                {
                    if (state.isGrounded)
                    {
                        if (eventArgs.input.combat.isThrowProjectilePressed)
                        {
                            ProjectileAimCall?.Invoke(this, EventActionArgs.Empty);
                        }

                        else
                        {
                            HandleBasicGroundAttacks(eventArgs.input);
                        }
                    }
                    else
                    {
                        HandleBasicJumpAttacks(eventArgs.input);
                    }
                }
            }
            else if (state.isAimingProjectile)
            {
                ProjectileAimUpdate?.Invoke(this, eventArgs);
            }
        }

        private void Update()
        {
            ////used bc sometimes on complete function is not called
            ////causing to player to get stuck in after attack animation
            //if (m_animationState.hasAttacked)
            //{
            //    if (m_animation.animationState.GetCurrent(0).ToString() == m_animation.currentAttackAnimation)
            //    {
            //        if (m_animation.animationState.GetCurrent(0).IsComplete)
            //        {
            //            OnComplete(m_animation.animationState.GetCurrent(0));
            //        }
            //    }
            //}
            //else
            //{
            //    if (m_behaviourState.waitForBehaviour)
            //    {
            //        if (m_animation.animationState.GetCurrent(0).IsComplete)
            //        {
            //            OnComplete(m_animation.animationState.GetCurrent(0));
            //        }
            //    }
            //}
        }

        private void HandleBasicJumpAttacks(PlayerInput input)
        {
            if (input.combat.isMainHandPressed)
            {
                if (input.direction.isUpHeld)
                {
                    JumpAttackUpwardCall?.Invoke(this, m_combatEventArgs);
                }
                else if (input.direction.isDownHeld)
                {
                    JumpAttackDownwardCall?.Invoke(this, m_combatEventArgs);
                }

                else
                {
                    JumpAttackForwardCall?.Invoke(this, m_combatEventArgs);
                }

                m_physics.simulateGravity = false;
                m_highjumpState.canHighJump = false;
                isMainHandPressed = true;
                OnAttack();
            }
        }

        private void HandleBasicGroundAttacks(PlayerInput input)
        {
            if (input.combat.isMainHandPressed)
            {
                if (input.direction.isUpHeld)
                {
                    UpwardAttackCall?.Invoke(this, m_combatEventArgs);
                }
                else
                {
                    BasicAttackCall?.Invoke(this, m_combatEventArgs);
                }

                isMainHandPressed = true;
                OnAttack();
            }
            else if (input.combat.isOffHandPressed)
            {
                BasicAttackCall?.Invoke(this, m_combatEventArgs);
                isOffHandPressed = true;
                OnAttack();
            }
        }

        private void OnAttack()
        {
            m_hasAttacked = true;
            m_attackIdleDuration.Reset();
            m_physics.SetVelocity(Vector2.zero);
            m_behaviourState.waitForBehaviour = true;
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            //if (trackEntry.Animation.Name == m_animation.currentAttackAnimation)
            //{
            //    m_behaviourState.waitForBehaviour = false;
            //    m_physics.simulateGravity = true;
            //    m_combatState.canAttack = true;
            //    m_combatState.isAttacking = false;
            //    m_physics.SetVelocity(0);
            //    isMainHandPressed = false;
            //    isOffHandPressed = false;
            //}
        }

        private void OnStart(TrackEntry trackEntry)
        {
            //if (trackEntry.Animation.Name == m_animation.currentAttackAnimation)
            //{
            //    //m_behaviourState.waitForBehaviour = true;
            //    m_combatState.canAttack = false;
            //    m_combatState.isAttacking = true;
            //    m_physics.SetVelocity(Vector2.zero);
            //}
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_hasAttacked = false;
        }

        private void Start()
        {
            //m_animation.animationState.Complete += OnComplete;
            //m_animation.animationState.Start += OnStart;
            m_attackIdleDuration.CountdownEnd += OnCountdownEnd;
            m_attackIdleDuration.Reset();
        }

#if UNITY_EDITOR
        public void Initialize(float duration)
        {
            m_attackIdleDuration = new CountdownTimer(duration);
        }
#endif
    }
}