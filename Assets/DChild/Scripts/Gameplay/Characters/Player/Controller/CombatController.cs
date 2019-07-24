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
        public event EventAction<CombatEventArgs> BasicAttackCall;
        public event EventAction<EventActionArgs> ProjectileAimCall;
        public event EventAction<ControllerEventArgs> ProjectileAimUpdate;

        public void CallUpdate(IPlayerState state, ControllerEventArgs eventArgs)
        {
            if (state.canAttack)
            {
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

                    }

                    if (eventArgs.input.combat.isMainHandPressed)
                    {
                        BasicAttackCall?.Invoke(this, new CombatEventArgs(eventArgs.input.direction));
                    }
                }
            }
            else if (state.isAimingProjectile)
            {
                ProjectileAimUpdate?.Invoke(this, eventArgs);
            }
        }
    }
}