using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class PlatformDrop : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private CountdownTimer m_ignoreColliderDuration;

        private Animator m_animator;
        private string m_platformDropParameter;
        private CharacterColliders m_playerColliders;
        private IPlatformDropState m_state;
        private CharacterPhysics2D m_physics;
        private Collider2D m_platformCollider;
        private RaySensor m_groundSensor;
        private IIsolatedTime m_time;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_groundSensor = info.GetSensor(PlayerSensorList.SensorType.Ground);
            m_state = info.state;
            m_playerColliders = info.character.colliders;
            m_time = info.character.isolatedObject;
            info.groundednessHandle.LandExecuted += OnLand;
            m_physics = info.physics;

            m_animator = info.animator;
            m_platformDropParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.PlatformDrop);
        }

        private void OnLand(object sender, EventActionArgs eventArgs)
        {
            ResetBehaviour();
        }

        private void ResetBehaviour()
        {
            ReapplyPlatformCollision(m_platformCollider);
            m_state.isDroppingFromPlatform = false;
            m_platformCollider = null;
            enabled = false;
        }

        public void DropFromPlatform()
        {
            if (m_platformCollider != null)
            {
                ReapplyPlatformCollision(m_platformCollider);
            }
            m_platformCollider = m_groundSensor.GetProminentHitCollider();

            var colliders = m_playerColliders.colliders;
            for (int i = 0; i < colliders.Length; i++)
            {

                Physics2D.IgnoreCollision(colliders[i], m_platformCollider, true);

            }
            m_ignoreColliderDuration.Reset();
            enabled = true;
            m_state.isDroppingFromPlatform = true;
            m_physics.StopCoyoteTime();
            m_animator.SetTrigger(m_platformDropParameter);
        }

        private void OnDoubleJumpCall(object sender, EventActionArgs eventArgs)
        {
            ReapplyPlatformCollision(m_platformCollider);
            m_state.isDroppingFromPlatform = false;
            m_platformCollider = null;
            enabled = false;
        }

        private void OnIgnoreColliderEnd(object sender, EventActionArgs eventArgs)
        {
            ResetBehaviour();
        }

        private void ReapplyPlatformCollision(Collider2D platformCollider)
        {
            if (platformCollider != null)
            {
                var colliders = m_playerColliders.colliders;
                for (int i = 0; i < colliders.Length; i++)
                {
                    Physics2D.IgnoreCollision(colliders[i], platformCollider, false);

                }
            }
        }

        private void Awake()
        {
            enabled = false;
            m_ignoreColliderDuration.CountdownEnd += OnIgnoreColliderEnd;
        }

        private void Update()
        {
            m_ignoreColliderDuration.Tick(m_time.deltaTime);
            if (m_physics.velocity.y > 0)
            {
                ResetBehaviour();
            }
        }

    }

}