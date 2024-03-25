using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Projectiles;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using Spine.Unity;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Combat;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ProjectileThrow : AttackBehaviour
    {
        [SerializeField, HideLabel]
        private ProjectileThrowStatsInfo m_configuration;
        [SerializeField]
        private ProjectileInfo m_projectile;
        public ProjectileInfo projectile => m_projectile;
        private ProjectileInfo m_cacheProjectile;
        
        [SerializeField]
        private Transform m_spawnPoint;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField]
        private Vector2 m_aimOffset;

        private Vector2 m_currentAim; //Relative to Character Facing

        private IProjectileThrowState m_throwState;
        private Character m_character;
        private ProjectileLauncher m_launcher;
        private int m_aimingProjectileAnimationParameter;
        private int m_skullThrowAnimationParameter;
        private int m_skullThrowVariantParameter;
        private bool m_updateProjectileInfo;
        private Projectile m_spawnedProjectile;
        private IPlayerModifer m_modifier;
        public Projectile spawnedProjectile => m_spawnedProjectile;
        private bool m_reachedVerticalThreshold = false;

        private bool m_willResetProjectile;

        public bool willResetProjectile => m_willResetProjectile;

        public event EventAction<EventActionArgs> ExecutionRequested;
        public event EventAction<EventActionArgs> ProjectileThrown;

        public void RequestExecution()
        {
            ExecutionRequested?.Invoke(this, EventActionArgs.Empty);
        }

        #region Aim
        public void StartAim()
        {
            GameSystem.ResetCursorPosition(); //FOr Quality of Life thing
            m_currentAim = m_configuration.defaultAim;

            if (m_configuration.adjustableXSpeed == false)
            {
                m_currentAim.x = m_configuration.horizontalThreshold.max;
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
            m_animator.SetBool(m_aimingProjectileAnimationParameter, true);
        }

        public void MoveAim(Vector2 delta, Vector2 mousePosition)
        {
            var relativeDelta = delta.normalized * m_configuration.aimSensitivity;
            relativeDelta.x *= (int)m_character.facing;
            var newAim = m_currentAim += relativeDelta;

            if (newAim.x < m_configuration.horizontalThreshold.min)
            {
                newAim.x = m_configuration.horizontalThreshold.min;
                m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            else if (newAim.x > m_configuration.horizontalThreshold.max)
            {
                newAim.x = m_configuration.horizontalThreshold.max;
            }

            if (newAim.y < m_configuration.verticalThreshold * -1)
            {
                newAim.y = m_configuration.verticalThreshold * -1;
                m_reachedVerticalThreshold = false;
            }
            else if (newAim.y > m_configuration.verticalThreshold)
            {
                newAim.y = m_configuration.verticalThreshold;
                m_reachedVerticalThreshold = true;
            }
            m_currentAim = newAim;
            var worldPosition = m_character.centerMass.position + (new Vector3(m_character.facing == HorizontalDirection.Right ? m_aimOffset.x : -m_aimOffset.x, m_aimOffset.y));
            var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            Mouse.current.WarpCursorPosition(screenPosition);
            UpdateTrajectorySimulation();
        }

        public void EndAim()
        {
            var simulatorHandle = GameplaySystem.simulationHandler;
            simulatorHandle.HideSimulation(simulatorHandle.GetTrajectorySimulator());
            m_animator.SetBool(m_aimingProjectileAnimationParameter, false);
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

            var velocity = CalculateThrowDirection() * m_projectile.speed;
            velocity.x *= (int)m_character.facing;
            simulator.SetVelocity(velocity);
            simulator.SimulateTrajectory();
        }

        private Vector2 CalculateThrowDirection()
        {
            var normalizedAim = Vector2.zero;

            if (m_configuration.adjustableXSpeed)
            {
                normalizedAim = m_currentAim;
            }
            else
            {
                normalizedAim = m_currentAim;
                normalizedAim.x = m_configuration.horizontalThreshold.max;
            }

            normalizedAim.x = CalculateNormalizedValue(normalizedAim.x, m_configuration.horizontalThreshold.min, m_configuration.horizontalThreshold.max);
            var ySign = Mathf.Sign(normalizedAim.y);
            normalizedAim.y = Mathf.Abs(normalizedAim.y) / m_configuration.verticalThreshold * ySign;
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
            //m_cacheProjectile = m_projectile;
            if (m_projectile != info)
            {
                
                m_projectile = info;
                m_launcher.SetProjectile(m_projectile);
                var skullThrowVariantIndex = info.projectile.GetComponent<Projectile>().hasConstantSpeed ? true : false;
                m_animator.SetBool(m_skullThrowVariantParameter, skullThrowVariantIndex);
                m_updateProjectileInfo = true;
            }
        }

        public void WillResetProjectile()
        {
            m_willResetProjectile = true;
        }

        public void ResetProjectile()
        {
            Debug.Log("projectile Reset");
            m_willResetProjectile = false;
            m_projectile = m_cacheProjectile;
            m_launcher.SetProjectile(m_projectile);
            var skullThrowVariantIndex = m_cacheProjectile.projectile.GetComponent<Projectile>().hasConstantSpeed ? true : false;
            m_animator.SetBool(m_skullThrowVariantParameter, skullThrowVariantIndex);
            m_updateProjectileInfo = true;
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

        public void ThrowProjectile()
        {
            m_skeletonAnimation.state.Complete += State_Complete;

            var direction = CalculateThrowDirection();
            direction.x *= (int)m_character.facing;

            if (m_spawnedProjectile != null)
            {
                m_spawnedProjectile.transform.parent = null;

                if (m_spawnedProjectile.TryGetComponentInChildren(out Animator animator))
                {
                    animator.SetTrigger("Shoot");
                }
                if (m_spawnedProjectile.TryGetComponent(out Collider2D collider))
                {
                    collider.enabled = true;
                }
                if (m_spawnedProjectile.TryGetComponent(out IsolatedObjectPhysics2D physics))
                {
                    physics.Enable();
                }

                m_launcher.LaunchProjectile(direction, m_spawnedProjectile.gameObject);
                m_spawnedProjectile.GetComponent<Attacker>().SetDamageModifier(m_modifier.Get(PlayerModifier.AttackDamage));
                var scale = m_spawnedProjectile.transform.localScale;
                scale.x = 1;
                m_spawnedProjectile.transform.localScale = scale;
                m_spawnedProjectile = null;
            }
            else
            {
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile.projectile, m_spawnPoint.position, Quaternion.identity);
                //instance.transform.position = m_spawnPoint.position;

                if (instance.TryGetComponentInChildren(out Animator animator))
                {
                    animator.SetTrigger("Shoot");
                }

                m_launcher.LaunchProjectile(direction, instance.gameObject);
            }

            ProjectileThrown?.Invoke(this, EventActionArgs.Empty);
        }

        private void State_Complete(Spine.TrackEntry trackEntry)
        {
            m_state.waitForBehaviour = false;
            m_skeletonAnimation.state.Complete -= State_Complete;
        }

        public void SpawnIdleProjectile()
        {
            //TEST
            m_spawnedProjectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile.projectile, m_spawnPoint.position, Quaternion.identity);
            Debug.Log(m_spawnPoint.position);
            //m_spawnedProjectile.transform.position = m_spawnPoint.position;
            m_spawnedProjectile.transform.parent = transform;
            //m_spawnedProjectile.transform.rotation = Quaternion.identity;
            m_spawnedProjectile.GetComponent<Attacker>().SetParentAttacker(m_attacker);
            //TEST

            var scale = m_spawnedProjectile.transform.localScale;
            scale.x = m_character.facing == HorizontalDirection.Right ? scale.x : -scale.x;
            scale.y = 1;
            m_spawnedProjectile.transform.localScale = scale;

            if (m_spawnedProjectile.TryGetComponent(out Collider2D collider))
            {
                collider.enabled = false;
            }
            if (m_spawnedProjectile.TryGetComponent(out IsolatedObjectPhysics2D physics))
            {
                physics.Disable();
            }
        }

        public override void AttackOver()
        {
            //m_state.isAttacking = false;
            m_reachedVerticalThreshold = false;
            m_animator.SetBool(m_skullThrowAnimationParameter, false);
            base.AttackOver();
        }

        public void Execute()
        {
            m_timer = m_configuration.skullThrowCooldown;
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_animator.SetBool(m_skullThrowAnimationParameter, true);

            //m_spawnedProjectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile.projectile);
            //m_spawnedProjectile.transform.position = m_spawnPoint.position;
            //m_spawnedProjectile.transform.parent = transform;
            //m_spawnedProjectile.GetComponent<Attacker>().SetParentAttacker(m_attacker);
            //if (m_spawnedProjectile.TryGetComponent(out IsolatedObjectPhysics2D physics))
            //{
            //    physics.Disable();
            //}
        }

        public void StartThrow()
        {
            m_state.waitForBehaviour = true;
        }

        public bool HasReachedVerticalThreshold()
        {
            return m_reachedVerticalThreshold;
        }

        public override void Cancel()
        {
            m_reachedVerticalThreshold = false;
            EndAim();
            m_skeletonAnimation.state.Complete -= State_Complete;

            if (m_spawnedProjectile != null)
            {
                Destroy(m_spawnedProjectile.gameObject);
            }
            m_animator.SetBool(m_skullThrowAnimationParameter, false);
            base.Cancel();
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
            m_attacker = info.attacker;
            m_launcher = new ProjectileLauncher(m_projectile, m_spawnPoint);
            m_aimingProjectileAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AimingProjectile);
            m_skullThrowAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ProjectileThrow);
            m_skullThrowVariantParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ProjectileThrowVariant);
            m_launcher.SetProjectile(m_projectile);
            m_launcher.SetSpawnPoint(m_spawnPoint);
            m_updateProjectileInfo = true;
            m_cacheProjectile = m_projectile;
            m_modifier = info.modifier;
        }


        public void SetConfiguration(ProjectileThrowStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }
    }
}