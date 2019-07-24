using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GroundController : MonoBehaviour, IGroundMoveController, ICrouchController, IPlatformDropController,
                                                   IJumpController, IGroundDashController
    {
        public event EventAction<ControllerEventArgs> MoveCall;
        public event EventAction<ControllerEventArgs> CrouchCall;
        public event EventAction<ControllerEventArgs> CrouchMoveCall;
        public event EventAction<EventActionArgs> JumpCall;
        public event EventAction<EventActionArgs> DashCall;
        public event EventAction<EventActionArgs> ShadowDashCall;
        public event EventAction<ControllerEventArgs> PlatformDropCall;

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isDashing)
            {

            }
            else
            {
                if (state.isCrouched)
                {
                    CrouchMoveCall?.Invoke(this, callArgs);
                }
                else
                {
                    MoveCall?.Invoke(this, callArgs);
                }
            }
        }

        public void CallUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            CrouchCall?.Invoke(this, callArgs);

            if (state.isCrouched)
            {
                if (state.canPlatformDrop && callArgs.input.isJumpPressed)
                {

                    PlatformDropCall?.Invoke(this, callArgs);
                }
            }
            else if (state.isDashing)
            {

            }
            else
            {
                if (callArgs.input.isJumpPressed)
                {
                    JumpCall?.Invoke(this, EventActionArgs.Empty);
                }

                else if (skills.IsEnabled(PrimarySkill.Dash))
                {
                    if (callArgs.input.skillInput.isDashPressed)
                    {
                        DashCall?.Invoke(this, EventActionArgs.Empty);
                    }
                }
                //else if (skills.IsEnabled(MovementSkill.ShadowDash))
                //{
                //    if (callArgs.input.skillInput.isDashPressed)
                //    {
                //        ShadowDashCall?.Invoke(this, EventActionArgs.Empty);
                //    }
                //}

            }
        }
    }
}