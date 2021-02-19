﻿using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
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
        private RangeFloat m_horizontalThreshold;
        [SerializeField, BoxGroup("Aim"), MinValue(0f)]
        private float m_verticalThreshold;
        [SerializeField, BoxGroup("Aim"), MinValue(0f)]
        private float m_aimSensitivity = 1f;
        [SerializeField]
        private bool m_adjustableXSpeed;

        private Vector2 m_currentAim; //Relative to Character Facing

        private IProjectileThrowState m_throwState;
        private Character m_character;
        private ProjectileLauncher m_launcher;
        private int m_skullThrowAnimationParameter;
        private bool m_updateProjectileInfo;

        public event EventAction<EventActionArgs> ExecutionRequested;

        public void RequestExecution()
        {
            ExecutionRequested?.Invoke(this, EventActionArgs.Empty);
        }

        #region Aim
        public void StartAim()
        {
            GameSystem.ResetCursorPosition(); //FOr Quality of Life thing
            m_currentAim = m_defaultAim;

            if(m_adjustableXSpeed == false)
            {
                m_currentAim.x = m_horizontalThreshold.max;
            }

            if (m_updateProjectileInfo)
            {
                UpdateTrajectoryProjectile();
                m_updateProjectileInfo = false;
            }
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
            if (newAim.x < m_horizontalThreshold.min)
            {
                newAim.x = m_horizontalThreshold.min;
                m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            else if (newAim.x > m_horizontalThreshold.max)
            {
                newAim.x = m_horizontalThreshold.max;
            }

            if (newAim.y < m_verticalThreshold * -1)
            {
                newAim.y = m_verticalThreshold * -1;
            }
            else if (newAim.y > m_verticalThreshold)
            {
                newAim.y = m_verticalThreshold;
            }
            m_currentAim = newAim;
            UpdateTrajectorySimulation();
        }

        public void EndAim()
        {
            //var simulatorHandle = GameplaySystem.simulationHandler;
            //simulatorHandle.HideSimulation(simulatorHandle.GetTrajectorySimulator());
            //m_animator.SetBool(m_skullThrowAnimationParameter, false);
            //m_throwState.isAimingProjectile = false;
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

            var velocity = CalculateThrowDirection() * m_projectile.speed;
            velocity.x *= (int)m_character.facing;
            simulator.SetVelocity(velocity);
            simulator.SimulateTrajectory();
        }

        private Vector2 CalculateThrowDirection()
        {
            var normalizedAim = Vector2.zero;

            if (m_adjustableXSpeed)
            {
                normalizedAim = m_currentAim;
            }
            else
            {
                normalizedAim = m_currentAim;
                normalizedAim.x = m_horizontalThreshold.max;
            }

            normalizedAim.x = CalculateNormalizedValue(normalizedAim.x, m_horizontalThreshold.min, m_horizontalThreshold.max);
            var ySign = Mathf.Sign(normalizedAim.y);
            normalizedAim.y = Mathf.Abs(normalizedAim.y) / m_verticalThreshold * ySign;
            return normalizedAim;

            float CalculateNormalizedValue(float value, float min, float max)
            {
                if (value == min)
                {
                    return 0.01f;
                }
                else if (value == max)
                {
                    return 1f;
                }
                else
                {
                    return (normalizedAim.x - min) / (max - min);
                }
            }
        }

        private void UpdateTrajectoryProjectile()
        {
            var projectile = m_projectile.projectile;
            var simulator = GameplaySystem.simulationHandler.GetTrajectorySimulator();
            var physics = projectile.GetComponent<IsolatedObjectPhysics2D>();
            float gravity = physics.simulateGravity ? physics.gravity.gravityScale : 0;
            simulator.SetObjectValues(projectile.GetComponent<Rigidbody2D>().mass, gravity, projectile.GetComponent<Collider2D>());
        }
        #endregion

        public void SetProjectileInfo(ProjectileInfo info)
        {
            if (m_projectile != info)
            {
                m_projectile = info;
                m_launcher.SetProjectile(m_projectile);
                m_updateProjectileInfo = true;
            }
        }

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
            var direction = CalculateThrowDirection();
            direction.x *= (int)m_character.facing;
            m_launcher.LaunchProjectile(direction);
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
            m_updateProjectileInfo = true;
        }
    }

}