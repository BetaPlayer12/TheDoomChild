using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundMovement : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        private CountdownTimer m_changeSpeedDuration;

        [SerializeField]
        private GroundMoveHandler m_moveHandler;

        [SerializeField]
        [TabGroup("TabGroup", "Jog Configurations")]
        private float m_jogSpeed;
        [SerializeField]
        [TabGroup("TabGroup", "Jog Configurations")]
        private float m_jogAcceleration;
        [SerializeField]
        [TabGroup("TabGroup", "Jog Configurations")]
        private float m_jogDecceleration;

        [SerializeField]
        [TabGroup("TabGroup", "Sprint Configurations")]
        private float m_sprintSpeed;
        [SerializeField]
        [TabGroup("TabGroup", "Sprint Configurations")]
        private float m_sprintAcceleration;
        [SerializeField]
        [TabGroup("TabGroup", "Sprint Configurations")]
        private float m_sprintDecceleration;

        private CharacterPhysics2D m_characterPhysics2D;

        private IMoveState m_state;
        private IIsolatedTime m_time;
        private IFacingConfigurator m_facingConfig;
        private HorizontalDirection m_previousFacing;
        private IPlayerState m_characterState;
        private Character m_character;
        private Animator m_animator;
        private string m_speedParameter;

        private bool m_increaseVelocity;


        public void Initialize(ComplexCharacterInfo info)
        {
            m_time = info.character.isolatedObject;
            m_characterPhysics2D = info.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_state = info.state;
            m_characterState = info.state;
            m_character = info.character;
            m_animator = info.animator;
            m_speedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);

        }

        public void ConnectTo(IMainController controller)
        {
            controller.GetSubController<IGroundMoveController>().MoveCall += OnMoveCall;
        }
        public void Move(float direction)
        {
            if (direction == 0)
            {
                if (m_increaseVelocity)
                {
                    ResetMoveVelocity();
                }

                if (m_characterPhysics2D.inContactWithGround)
                {
                    m_characterPhysics2D.SetVelocity(Vector2.zero);
                }
                else
                {
                    m_characterPhysics2D.SetVelocity(0);
                }
                

                m_state.isMoving = false;
                m_changeSpeedDuration.Reset();
            }
            else
            {
                m_changeSpeedDuration.Tick(m_time.deltaTime);
                var moveDirection = direction > 0 ? Vector2.right : Vector2.left;
                m_moveHandler.SetDirection(moveDirection);
                m_moveHandler.Accelerate();
                m_state.isMoving = true;
                m_animator.SetInteger(m_speedParameter, 1);
                m_character.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);

                //if (m_increasevelocity)
                //{
                //    if (m_facingconfig.currentfacingdirection != m_previousfacing)
                //    {
                //        resetmovevelocity();
                //    }
                //}
                //else
                //{
                //    m_state.isjogging = true;
                //}


            }
        }

        public void UpdateVelocity()
        {
            var moveDirection = m_characterPhysics2D.velocity.x >= 0 ? Vector2.right : Vector2.left;
            m_moveHandler.SetDirection(moveDirection);
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IGroundMoveController>().MoveCall += OnMoveCall;
        }

        private void Update()
        {
            if (m_increaseVelocity)
            {
                if (m_characterState.waitForBehaviour || m_characterState.isDashing || !m_characterState.isGrounded)
                {
                    ResetMoveVelocity();
                }
            }
        }

        private void ResetMoveVelocity()
        {
            m_state.isJogging = false;
            m_state.isSprinting = false;
            m_increaseVelocity = false;
            m_moveHandler.ResetMoveVelocity(m_jogSpeed, m_jogAcceleration, m_jogDecceleration);
            m_animator.SetInteger(m_speedParameter, 0);
            m_changeSpeedDuration.Reset();
        }

        private void OnMoveCall(object sender, ControllerEventArgs eventArgs)
        {
            Move(eventArgs.input.direction.horizontalInput);
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_state.isJogging = false;
            m_state.isSprinting = true;
            m_increaseVelocity = true;
            m_moveHandler.IncreaseMoveVelocity(m_sprintSpeed, m_sprintAcceleration, m_sprintDecceleration);
            m_previousFacing = m_facingConfig.currentFacingDirection;
        }

        private void Awake()
        {
            m_changeSpeedDuration.CountdownEnd += OnCountdownEnd;
            m_moveHandler.ResetMoveVelocity(m_jogSpeed, m_jogAcceleration, m_jogDecceleration);
        }

#if UNITY_EDITOR
        public void Initialize(float maxSpeed, float acceleration, float decceleration)
        {
            m_moveHandler = new GroundMoveHandler(maxSpeed, acceleration, decceleration);
        }

#endif
    }
}