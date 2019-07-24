using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using Holysoft.Event;
using DChild.Inputs;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ProjectileThrowHandler : MonoBehaviour, IEventModule, IPlayerExternalModule
    {
        [SerializeField]
        private ProjectileThrow m_module;
        [SerializeField]
        [MinValue(0)]
        private float m_changeModifier = 1;
        [SerializeField]
        [MinValue(0)]
        private float m_mouseChangeModifier = 1;
        [SerializeField]
        private bool m_useKeysProjectile;

        private CharacterPhysics2D m_physics;
        private TrajectorySimulator m_simulator;
        private Vector2 m_trajectoryForce;

        private IProjectileThrowState m_state;
        private IFacing m_facing;

        private bool m_isCalled;

        public GameObject currentProjectile => m_module.projectile;

        public void ConnectEvents()
        {
            var controller = GetComponentInParent<IProjectileThrowController>();
            controller.ProjectileAimCall += OnCall;
            controller.ProjectileAimUpdate += OnUpdate;
        }

        public void Initialize(IPlayerModules player)
        {
            m_physics = player.physics;
            //m_animation = player.animation;
            m_state = player.characterState;
            m_facing = player;
        }

        public void SetProjectile(GameObject projectile)
        {
            m_module.SetProjectile(projectile);
        }

        private Vector2 ChangeAim(PlayerInput input, bool useKeys)
        {
            float changeX;
            float changeY;
            if (useKeys)
            {
                var modifier = Time.deltaTime * m_changeModifier;
                changeX = input.direction.horizontalInput * modifier;
                changeY = input.direction.verticalInput * modifier;
            }
            else
            {
                var mouseModifier = Time.deltaTime * m_mouseChangeModifier;
                changeX = input.mouseInput.shiftNormalRaw.x * mouseModifier;
                changeY = input.mouseInput.shiftNormalRaw.y * mouseModifier;
            }

            var change = new Vector2(changeX, changeY);
            return change;
        }

        private void OnUpdate(object sender, ControllerEventArgs eventArgs)
        {
            //    if (m_state.isAimingProjectile)
            //    {
            //        m_state.waitForBehaviour = true;
            //        if (m_isCalled)
            //        {
            //            m_animation.DoSkullThrowAim(m_facing.currentFacingDirection);
            //        }
            //        else
            //        {
            //            m_animation.DoSkullThrowCall(m_facing.currentFacingDirection);
            //            if (m_animation.animationState.GetCurrent(0).IsComplete)
            //            {
            //                m_isCalled = true;
            //            }
            //        }

            //        if (eventArgs.input.combat.isThrowProjectileHeld)
            //        {
            //            m_state.waitForBehaviour = true;
            //            var aim = ChangeAim(eventArgs.input, m_useKeysProjectile);
            //            aim.x = 0;
            //            m_module.AdjustAim(aim);
            //            var force = m_module.currentAim * m_module.currentThrowForce;
            //            if (m_trajectoryForce != force)
            //            {
            //                m_trajectoryForce = force;
            //                m_simulator.SetVelocity(force);
            //                GameplaySystem.simulationHandler.ShowSimulation(m_simulator);
            //            }
            //        }
            //        else
            //        {
            //            StartCoroutine(ThrowRoutine());
            //            GameplaySystem.simulationHandler.HideSimulation(m_simulator);

            //            m_state.canAttack = true;
            //        }
            //    }
        }

        private void OnCall(object sender, EventActionArgs eventArgs)
        {
            m_state.isAimingProjectile = true;
            m_state.canAttack = false;
            m_state.waitForBehaviour = true;
            m_physics.SetVelocity(Vector2.zero);

            var target = (m_facing.currentFacingDirection == HorizontalDirection.Left ? new Vector2(-1, 0) : new Vector2(1, 0));
            m_module.SetDefaultAim(target);
            m_module.ResetAim();
            m_module.ResetThrowForce();
            m_trajectoryForce = m_module.currentAim * m_module.currentThrowForce;
            var projectileRigidbody = m_module.projectile.GetComponent<Rigidbody2D>();
            m_simulator = GameplaySystem.simulationHandler.GetTrajectorySimulator();
            m_simulator.SetObjectValues(projectileRigidbody.mass, projectileRigidbody.gravityScale);
            m_simulator.SetStartPosition(m_module.transform.position);
            m_simulator.SetVelocity(m_trajectoryForce);
            GameplaySystem.simulationHandler.ShowSimulation(m_simulator);
        }

        private IEnumerator ThrowRoutine()
        {
            //m_animationState.isThrowingBomb = true;
            //m_isCalled = false;
            //m_animation.DoSkullThrowOnHit(m_facing.currentFacingDirection);
            //yield return new WaitForAnimationEvent(m_animation.animationState, PlayerAnimation.EVENT_SKULLTHROW);
            //m_module.Throw();
            //m_state.isAimingProjectile = false;
            //m_animationState.isThrowingBomb = false;

            yield return null;
        }
    }
}