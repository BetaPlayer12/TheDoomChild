using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class PlatformDrop : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private CountdownTimer m_ignoreColliderDuration;

        private CharacterColliders m_playerColliders;
        private IPlatformDropState m_state;
        private Collider2D m_platformCollider;
        private RaySensor m_groundSensor;
        
        private IIsolatedTime m_time;

        public void ConnectEvents()
        {
            GetComponentInParent<IPlatformDropController>().PlatformDropCall += OnPlatformDropCall;
            GetComponentInParent<IDoubleJumpController>().DoubleJumpCall += OnDoubleJumpCall;
        }

        public void Initialize(IPlayerModules player)
        {
            m_groundSensor = player.sensors.groundSensor;
           
            m_state = player.characterState;
            m_playerColliders = player.colliders;
            m_time = player.isolatedObject;
        }

        public void DropFromPlatform()
        {
            var colliders = m_playerColliders.colliders;
            for (int i = 0; i < colliders.Length; i++)
            {
                Physics2D.IgnoreCollision(colliders[i], m_platformCollider, true);
            }
            m_ignoreColliderDuration.Reset();
            enabled = true;
            m_state.isDroppingFromPlatform = true;
        }

        private void OnPlatformDropCall(object sender, ControllerEventArgs eventArgs)
        {
            if (m_platformCollider != null)
            {
                ReapplyPlatformCollision(m_platformCollider);
            }
            m_platformCollider = m_groundSensor.GetProminentHitCollider();
            DropFromPlatform();
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
            ReapplyPlatformCollision(m_platformCollider);
            m_state.isDroppingFromPlatform = false;
            m_platformCollider = null;
            enabled = false;
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
        }

#if UNITY_EDITOR
        public void Initialize(float time)
        {
            m_ignoreColliderDuration = new CountdownTimer(time);
        }
#endif
    }

}