using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Skill;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

<<<<<<< HEAD
    public class AirController : MonoBehaviour, IAirMoveController, IHighJumpController, IDoubleJumpController,
                                 IWallStickController, IWallJumpController, IAirDashController, ILedgeController
    {
        public event EventAction<ControllerEventArgs> MoveCall;
        public event EventAction<ControllerEventArgs> HighJumpCall;
        public event EventAction<EventActionArgs> DoubleJumpCall;
        public event EventAction<EventActionArgs> WallStickCall;
        public event EventAction<ControllerEventArgs> UpdateCall;
        public event EventAction<EventActionArgs> WallJumpCall;
        public event EventAction<EventActionArgs> DashCall;
        public event EventAction<EventActionArgs> LedgeGrabCall;//
        public event EventAction<EventActionArgs> WallSlideCall;
        public event EventAction<ControllerEventArgs> AttempWallStickCall;
        public event EventAction<EventActionArgs> WallStickCancel;

        private SkillResetRequester m_skillRequester;
        public void Initialize(SkillResetRequester skillRequester)
        {
            m_skillRequester = skillRequester;
=======
    public class AirController : MonoBehaviour
    {
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private MovementHandle m_movement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private MoveSpeedTransistor m_speedTransistor;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private HighJump m_highJump;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private DoubleJump m_doubleJump;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private WallStick m_wallStick;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private WallJump m_wallJump;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private AirDash m_dash;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private LedgeGrab m_ledgeGrab;


        private SkillResetRequester m_skillRequester;
        public void Initialize(GameObject behaviours, SkillResetRequester skillRequester)
        {
            m_skillRequester = skillRequester;
            m_movement = behaviours.GetComponentInChildren<MovementHandle>();
            m_speedTransistor = behaviours.GetComponentInChildren<MoveSpeedTransistor>();
            m_highJump = behaviours.GetComponentInChildren<HighJump>();
            m_doubleJump = behaviours.GetComponentInChildren<DoubleJump>();
            m_wallStick = behaviours.GetComponentInChildren<WallStick>();
            m_wallJump = behaviours.GetComponentInChildren<WallJump>();
            m_dash = behaviours.GetComponentInChildren<AirDash>();
            m_ledgeGrab = behaviours.GetComponentInChildren<LedgeGrab>();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isFalling)
            {
                if (state.isMoving)
                {
<<<<<<< HEAD
                    LedgeGrabCall?.Invoke(this, EventActionArgs.Empty);
                    if (state.waitForBehaviour)
=======
                    if (m_ledgeGrab?.AttemptToLedgeGrab() ?? false)
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                    {
                        m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                        return;
                    }
                }
            }

            if (state.isStickingToWall)
            {

            }
            else if (state.isDashing)
            {

            }
            else
            {
                m_movement?.Move(callArgs.input.direction.horizontalInput);
            }
        }

        public void CallUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
<<<<<<< HEAD
=======
            m_speedTransistor.SwitchToAirMoveSpeed();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
            if (state.isStickingToWall)
            {
                m_wallStick?.HandleWallStick();
                if (skills.IsEnabled(PrimarySkill.WallJump) && callArgs.input.isJumpPressed)
                {
<<<<<<< HEAD
                    WallJumpCall?.Invoke(this, EventActionArgs.Empty);
                    WallStickCancel?.Invoke(this, EventActionArgs.Empty);
=======
                    m_wallStick?.CancelWallStick();
                    m_wallJump?.HandleJump();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                }
                else if (state.isSlidingToWall == false && callArgs.input.direction.isDownPressed)
                {
                    m_wallStick?.StartWallSlide();
                }
            }

            else if (state.isDashing)
            {

            }

            else
            {
<<<<<<< HEAD
              

                if (state.canHighJump)
                {
                    HighJumpCall?.Invoke(this, callArgs);
=======
                if (state.canHighJump)
                {
                    m_highJump?.HandleHighJump(callArgs.input.isJumpHeld);
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                }

                if (skills.IsEnabled(PrimarySkill.DoubleJump) && state.canDoubleJump)
                {
                    if (callArgs.input.isJumpPressed)
                    {
<<<<<<< HEAD
                        DoubleJumpCall?.Invoke(this, EventActionArgs.Empty);
=======
                        m_doubleJump?.HandleJump();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                    }
                }

                if (skills.IsEnabled(PrimarySkill.Dash) && state.canDash)
                {
                    if (callArgs.input.skillInput.isDashPressed)
                    {
                        m_dash?.StartDash();
                    }
                }
            }

<<<<<<< HEAD
            UpdateCall?.Invoke(this, callArgs);
            if (skills.IsEnabled(PrimarySkill.WallJump) && state.isDroppingFromPlatform == false)
            {
                AttempWallStickCall?.Invoke(this, callArgs);
=======
            //UpdateCall?.Invoke(this, callArgs);
            if (skills.IsEnabled(PrimarySkill.WallJump) && state.isDroppingFromPlatform == false)
            {
                m_wallStick?.AttemptToWallStick();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                if (state.isStickingToWall)
                {
                    m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                }
            }
        }
    }
}