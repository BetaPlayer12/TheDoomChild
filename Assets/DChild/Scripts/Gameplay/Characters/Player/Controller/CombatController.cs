﻿using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [AddComponentMenu("DChild/Gameplay/Player/Controller/Combat Controller")]
    public class CombatController : MonoBehaviour
    {

        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private BasicAttack m_basicAttack;
        public event EventAction<EventActionArgs> ProjectileAimCall;
        public event EventAction<ControllerEventArgs> ProjectileAimUpdate;

        public void Initialize(GameObject behaviours)
        {
            m_basicAttack = behaviours.GetComponentInChildren<BasicAttack>();
        }


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
                        m_basicAttack?.SetAttackDirection(eventArgs.input.direction);
                        m_basicAttack?.Execute();
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