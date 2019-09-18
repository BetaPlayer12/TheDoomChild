using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Skill;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
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

        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isFalling)
            {
                if (state.isMoving)
                {

                    if (m_ledgeGrab?.AttemptToLedgeGrab() ?? false)
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

            m_speedTransistor.SwitchToAirMoveSpeed();
            if (state.isStickingToWall)
            {
                m_wallStick?.HandleWallStick();
                if (skills.IsEnabled(PrimarySkill.WallJump) && callArgs.input.isJumpPressed)
                {

                    m_wallStick?.CancelWallStick();
                    m_wallJump?.HandleJump();
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
                if (state.canHighJump)
                {
                    m_highJump?.HandleHighJump(callArgs.input.isJumpHeld);
                }

                if (skills.IsEnabled(PrimarySkill.DoubleJump) && state.canDoubleJump)
                {
                    if (callArgs.input.isJumpPressed)
                    {
                        m_doubleJump?.HandleJump();
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
            //UpdateCall?.Invoke(this, callArgs);
            if (skills.IsEnabled(PrimarySkill.WallJump) && state.isDroppingFromPlatform == false)
            {
                m_wallStick?.AttemptToWallStick();
                if (state.isStickingToWall)
                {
                    m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                }
            }
        }
    }
}