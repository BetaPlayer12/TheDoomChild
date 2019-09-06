using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class CombatController : MonoBehaviour, IProjectileThrowController
    {
<<<<<<< HEAD
        public event EventAction<CombatEventArgs> BasicAttackCall;
        public event EventAction<CombatEventArgs> WhipAttackCall;
        public event EventAction<EventActionArgs> ProjectileAimCall;
        public event EventAction<ControllerEventArgs> ProjectileAimUpdate;
        
=======
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private BasicAttack m_basicAttack;
        public event EventAction<EventActionArgs> ProjectileAimCall;
        public event EventAction<ControllerEventArgs> ProjectileAimUpdate;

        public void Initialize(GameObject behaviours)
        {
            m_basicAttack = behaviours.GetComponentInChildren<BasicAttack>();
        }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818

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

<<<<<<< HEAD
                    }

                    if (eventArgs.input.combat.isMainHandPressed)
                    {
                        BasicAttackCall?.Invoke(this, new CombatEventArgs(eventArgs.input.direction));
                    }
                    if (eventArgs.input.combat.isOffHandPressed)
                    {
                        WhipAttackCall?.Invoke(this, new CombatEventArgs(eventArgs.input.direction)); 
                    }
=======
                    }

                    if (eventArgs.input.combat.isMainHandPressed)
                    {
                        m_basicAttack?.SetAttackDirection(eventArgs.input.direction);
                        m_basicAttack?.Execute();
                    }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                }
            }
            else if (state.isAimingProjectile)
            {
                ProjectileAimUpdate?.Invoke(this, eventArgs);
            }
        }
    }
}