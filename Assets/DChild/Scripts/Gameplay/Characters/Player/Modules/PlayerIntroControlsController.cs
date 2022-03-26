using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerIntroControlsController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private InputTranslator m_input;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        private bool m_isUsingIntroControls = false;
        private bool m_canJump = false;
        private bool m_canSlash = false;

        #region Behaviors
        private PlayerPhysicsMatHandle m_physicsMat;
        private GroundednessHandle m_groundedness;
        private InitialDescentBoost m_initialDescentBoost;

        private IdleHandle m_idle;
        private Movement m_movement;
        private GroundJump m_groundJump;
        private Crouch m_crouch;

        private CollisionRegistrator m_attackRegistrator;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;
        #endregion

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Enable()
        {
            m_isUsingIntroControls = true;
        }

        public void Disable()
        {
            m_isUsingIntroControls = false;
        }

        public bool IsUsingIntroControls() => m_isUsingIntroControls;

        public void EnableIntroControls()
        {
            m_isUsingIntroControls = true;
        }

        public void EnableJump()
        {
            m_canJump = true;
        }

        public void HandleIntroControls()
        {
            if (m_state.waitForBehaviour)
                return;

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
                m_basicSlashes?.ResetAerialGravityControl();
            }
            else
            {
                HandleAirBehaviour();
            }
        }

        public void HandleIntroControlsFixedUpdate()
        {
            if (m_state.isGrounded)
            {
                if (m_state.forcedCurrentGroundedness == false)
                {
                    m_groundedness?.Evaluate();
                }

                if (m_groundedness?.isUsingCoyote ?? false)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);
                }
            }
            else
            {
                m_initialDescentBoost?.Handle();
                if (m_rigidbody.velocity.y < m_groundedness?.groundCheckOffset)
                {
                    if (m_state.forcedCurrentGroundedness == false)
                    {
                        m_groundedness?.Evaluate();
                    }
                }
            }
        }

        private void HandleGroundBehaviour()
        {
            if (m_input.jumpPressed)
            {
                m_idle?.Cancel();
                m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                m_groundedness?.ChangeValue(false);
                m_groundJump?.Execute();
            }
            else
            {
                MoveCharacter();
            }
        }

        private void HandleAirBehaviour()
        {
            if (m_state.isHighJumping)
            {
                if (m_groundJump?.CanCutoffJump() ?? true)
                {
                    if (m_input.jumpHeld == false)
                    {
                        m_groundJump?.CutOffJump();
                    }
                }

                if (m_rigidbody.velocity.y <= (m_groundJump?.highJumpCutoffThreshold ?? 0f))
                {
                    m_groundJump?.EndExecution();
                }
                m_groundJump?.HandleCutoffTimer();
            }

            m_movement.Move(m_input.horizontalInput, true);
        }

        private void MoveCharacter()
        {
            if (m_input.horizontalInput == 0)
            {
                m_idle?.Execute(m_state.allowExtendedIdle);
            }
            else
            {
                m_idle?.Cancel();
            }

            m_movement?.Move(m_input.horizontalInput, true);
        }

        private void Awake()
        {
            m_physicsMat = m_character.GetComponentInChildren<PlayerPhysicsMatHandle>();
            m_groundedness = m_character.GetComponentInChildren<GroundednessHandle>();
            m_groundedness.StateChange += OnGroundednessStateChange;
            m_initialDescentBoost = m_character.GetComponentInChildren<InitialDescentBoost>();

            m_idle = m_character.GetComponentInChildren<IdleHandle>();
            m_movement = m_character.GetComponentInChildren<Movement>();
            m_groundJump = m_character.GetComponentInChildren<GroundJump>();
            m_crouch = m_character.GetComponentInChildren<Crouch>();

            m_attackRegistrator = m_character.GetComponentInChildren<CollisionRegistrator>();
            m_basicSlashes = m_character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = m_character.GetComponentInChildren<SlashCombo>();
        }

        private void OnGroundednessStateChange(object sender, EventActionArgs eventArgs)
        {
            if (m_state.isDead)
            {
                //Then you need to git gud.
            }
            else
            {
                #region Groundedness Switch
                if (m_state.isGrounded)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);

                    m_initialDescentBoost?.Reset();
                    m_movement?.SwitchConfigTo(Movement.Type.Jog);

                    if (m_state.isAttacking)
                    {
                        m_basicSlashes.Cancel();
                    }
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);

                    if (m_state.isCrouched)
                    {
                        m_crouch?.Cancel();
                    }

                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                }
                #endregion
            }
        }
    }
}