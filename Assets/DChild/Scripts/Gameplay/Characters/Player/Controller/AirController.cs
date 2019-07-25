using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class AirController : MonoBehaviour, IAirMoveController, IHighJumpController, IDoubleJumpController,
                                 IWallStickController, IWallJumpController, IAirDashController, IFallController, ILedgeController, IFeetLedgeController
    {
        public event EventAction<ControllerEventArgs> MoveCall;
        public event EventAction<ControllerEventArgs> HighJumpCall;
        public event EventAction<EventActionArgs> DoubleJumpCall;
        public event EventAction<EventActionArgs> WallStickCall;
        public event EventAction<ControllerEventArgs> UpdateCall;
        public event EventAction<EventActionArgs> WallJumpCall;
        public event EventAction<EventActionArgs> DashCall;
        public event EventAction<EventActionArgs> DoubleJumpReset;
        public event EventAction<EventActionArgs> FallUpdate;
        public event EventAction<EventActionArgs> FallCall;
        public event EventAction<EventActionArgs> LedgeGrabCall;//
        public event EventAction<ControllerEventArgs> FeetLedgeCall;//
        public event EventAction<EventActionArgs> WallSlideCall;

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isFalling)
            {

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
            FeetLedgeCall?.Invoke(this, callArgs);
            if (state.isStickingToWall)
            {
                WallStickCall?.Invoke(this, EventActionArgs.Empty);
                if (skills.IsEnabled(PrimarySkill.WallJump) && callArgs.input.isJumpPressed)
                {
                    WallJumpCall?.Invoke(this, EventActionArgs.Empty);
                    DoubleJumpReset?.Invoke(this, EventActionArgs.Empty);
                }
                else if (state.isSlidingToWall ==false && callArgs.input.direction.isDownPressed)
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
                    FeetLedgeCall?.Invoke(this, callArgs);
                    LedgeGrabCall?.Invoke(this, EventActionArgs.Empty);
                    if (state.waitForBehaviour)
                    {
                        DoubleJumpReset?.Invoke(this, EventActionArgs.Empty);
                        return;
                    }
                }

                if (state.canHighJump)
                {
                    HighJumpCall?.Invoke(this, callArgs);
                    FeetLedgeCall?.Invoke(this, callArgs);
                }

                if (skills.IsEnabled(PrimarySkill.DoubleJump) && state.canDoubleJump)
                {
                    if (callArgs.input.isJumpPressed)
                    {
                        DoubleJumpCall?.Invoke(this, EventActionArgs.Empty);
                        FeetLedgeCall?.Invoke(this, callArgs);
                    }
                }

                if (skills.IsEnabled(PrimarySkill.Dash))
                {
                    if (callArgs.input.skillInput.isDashPressed)
                    {
                        DashCall?.Invoke(this, EventActionArgs.Empty);
                    }
                }
            }

            UpdateCall?.Invoke(this, callArgs);
        }
    }
}