using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SkullThrow : AttackBehaviour
    {
        [SerializeField]
        private ProjectileInfo m_projectile;
        [SerializeField]
        private Transform m_spawnPoint;
        [SerializeField]
        private float m_skullThrowCooldown;
        [SerializeField, BoxGroup("Aim")]
        private Vector2 m_defaultAim;
        [SerializeField, BoxGroup("Aim")]
        private Vector2 m_minAimValue;
        [SerializeField, BoxGroup("Aim")]
        private Vector2 m_maxAimValue;
        [SerializeField, BoxGroup("Aim"),MinValue(0f)]
        private float m_aimSensitivity = 1f;

        private Vector2 m_currentAim; //Relative to Character Facing

        private IProjectileThrowState m_throwState;
        private Character m_character;
        private ProjectileLauncher m_launcher;
        private int m_skullThrowAnimationParameter;

        #region Aim
        public void StartAim()
        {
            m_currentAim = m_defaultAim;
            UpdateTrajectoryProjectile();
            var simulatorHandle = GameplaySystem.simulationHandler;
            simulatorHandle.ShowSimulation(simulatorHandle.GetTrajectorySimulator());
            UpdateTrajectorySimulation();
            m_throwState.isAimingProjectile = true;
        }

        public void MoveAim(Vector2 delta)
        {
            var relativeDelta = delta.normalized * m_aimSensitivity;
            relativeDelta.x *= (int)m_character.facing;
            var newAim = m_currentAim += relativeDelta;
            if (newAim.x < m_minAimValue.x)
            {
                newAim.x = m_minAimValue.x;
                m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            else if (newAim.x > m_maxAimValue.x)
            {
                newAim.x = m_maxAimValue.x;
            }

            if (newAim.y < m_minAimValue.y)
            {
                newAim.y = m_minAimValue.y;
            }
            else if (newAim.y > m_maxAimValue.y)
            {
                newAim.y = m_maxAimValue.y;
            }
            m_currentAim = newAim;
            UpdateTrajectorySimulation();
        }

        public void EndAim()
        {
            var simulatorHandle = GameplaySystem.simulationHandler;
            simulatorHandle.HideSimulation(simulatorHandle.GetTrajectorySimulator());
            m_animator.SetBool(m_skullThrowAnimationParameter, false);
            m_throwState.isAimingProjectile = false;
        }

        private Vector2 GetStartingPosition()
        {
            //var startingPosition = m_currentAim;
            ////startingPosition.x *= (int)m_character.facing;
            //startingPosition.x = 0;
            //startingPosition += (Vector2)m_character.centerMass.position + new Vector2(5 * (int)m_character.facing,0);
            return (Vector2)m_spawnPoint.position;
        }

        private void UpdateTrajectorySimulation()
        {
            var simulator = GameplaySystem.simulationHandler.GetTrajectorySimulator();
            simulator.SetStartPosition(GetStartingPosition());
            var velocity = m_currentAim.normalized * m_projectile.speed;
            velocity.x *= (int)m_character.facing;
            simulator.SetVelocity(velocity);
            simulator.SimulateTrajectory();
        }

        private void UpdateTrajectoryProjectile()
        {
            var projectile = m_projectile.projectile;
            var simulator = GameplaySystem.simulationHandler.GetTrajectorySimulator();
            var physics = projectile.GetComponent<IsolatedObjectPhysics2D>();
            var rigidbody = projectile.GetComponent<Rigidbody2D>();
            float gravity = physics.simulateGravity ? rigidbody.gravityScale : 0;
            simulator.SetObjectValues(rigidbody.mass, gravity, projectile.GetComponent<Collider2D>());
        }
        #endregion

        public void HandleNextAttackDelay()
        {
            if (m_timer >= 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = -1;
                    m_state.canAttack = true;
                }
            }
        }

        public void SpawnProjectile()
        {
            var currentFlightDirection = m_currentAim.normalized;
            currentFlightDirection.x *= (float)m_character.facing;
            m_launcher.LaunchProjectile(currentFlightDirection);
        }

        public override void AttackOver()
        {
            base.AttackOver();
            m_animator.SetBool(m_skullThrowAnimationParameter, false);
        }

        public void Execute()
        {
            m_timer = m_skullThrowCooldown;
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_skullThrowAnimationParameter, true);
        }

        public void StartThrow()
        {
            m_state.waitForBehaviour = true;
        }

        public override void Cancel()
        {
            base.Cancel();
            EndAim();
            m_animator.SetBool(m_skullThrowAnimationParameter, false);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_throwState = info.state;
            m_animator = info.animator;
            m_character = info.character;
            m_launcher = new ProjectileLauncher(m_projectile, m_spawnPoint);
            m_skullThrowAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SkullThrow);

            m_launcher.SetProjectile(m_projectile);
            m_launcher.SetSpawnPoint(m_spawnPoint);
        }
    }

}