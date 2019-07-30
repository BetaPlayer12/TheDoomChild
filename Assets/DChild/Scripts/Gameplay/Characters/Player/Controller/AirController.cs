using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class AirController : MonoBehaviour, IAirMoveController, IHighJumpController, IDoubleJumpController,
                                 IWallStickController, IWallJumpController, IAirDashController, IFallController, ILedgeController
    {
        public event EventAction<ControllerEventArgs> MoveCall;
        public event EventAction<ControllerEventArgs> HighJumpCall;
        public event EventAction<EventActionArgs> DoubleJumpCall;
        public event EventAction<EventActionArgs> WallStickCall;
        public event EventAction<ControllerEventArgs> UpdateCall;
        public event EventAction<EventActionArgs> WallJumpCall;
        public event EventAction<EventActionArgs> DashCall;
        public event EventAction<EventActionArgs> FallUpdate;
        public event EventAction<EventActionArgs> FallCall;
        public event EventAction<EventActionArgs> LedgeGrabCall;//
        public event EventAction<EventActionArgs> WallSlideCall;
        public event EventAction<ControllerEventArgs> AttempWallStickCall;
        public event EventAction<EventActionArgs> WallStickCancel;

        private SkillResetRequester m_skillRequester;
        public void Initialize(SkillResetRequester skillRequester)
        {
            m_skillRequester = skillRequester;
        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isFalling)
            {
                if (state.isMoving)
                {
                    LedgeGrabCall?.Invoke(this, EventActionArgs.Empty);
                    if (state.waitForBehaviour)
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
                MoveCall?.Invoke(this, callArgs);
            }
        }

        public void CallUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isStickingToWall)
            {
                WallStickCall?.Invoke(this, EventActionArgs.Empty);
                if (skills.IsEnabled(PrimarySkill.WallJump) && callArgs.input.isJumpPressed)
                {
                    WallJumpCall?.Invoke(this, EventActionArgs.Empty);
                    WallStickCancel?.Invoke(this, EventActionArgs.Empty);
                }
                else if (state.isSlidingToWall == false && callArgs.input.direction.isDownPressed)
                {
                    WallSlideCall?.Invoke(this, EventActionArgs.Empty);
                }
            }

            else if (state.isDashing)
            {

            }

            else
            {
                if (state.isFalling)
                {
                    FallCall?.Invoke(this, EventActionArgs.Empty);
                    FallUpdate?.Invoke(this, EventActionArgs.Empty);
                    // FeetLedgeCall?.Invoke(this, callArgs);

                }

                if (state.canHighJump)
                {
                    HighJumpCall?.Invoke(this, callArgs);
                }

                if (skills.IsEnabled(PrimarySkill.DoubleJump) && state.canDoubleJump)
                {
                    if (callArgs.input.isJumpPressed)
                    {
                        DoubleJumpCall?.Invoke(this, EventActionArgs.Empty);
                    }
                }

                if (skills.IsEnabled(PrimarySkill.Dash) && state.canDash)
                {
                    if (callArgs.input.skillInput.isDashPressed)
                    {
                        DashCall?.Invoke(this, EventActionArgs.Empty);
                    }
                }
            }

            UpdateCall?.Invoke(this, callArgs);
            if (skills.IsEnabled(PrimarySkill.WallJump) && state.isDroppingFromPlatform == false)
            {
                AttempWallStickCall?.Invoke(this, callArgs);
                if (state.isStickingToWall)
                {
                    m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                }
            }
        }
    }
}