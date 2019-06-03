using System;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ExplodingFlyingDrone : ExplodingObject
    {
        [SerializeField]
        [MinValue(0)]
        private float m_chargeSpeed;
        private PhysicsMovementHandler2D m_movement;

        public void ChargeTo(Vector2 targetPosition)
        {
            m_movement.MoveTo(targetPosition, m_chargeSpeed);
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<ObjectPhysics2D>(), transform);
        }
    }

    public class ExplodingFlyingDroneBrain : MonoBehaviour, IAITargetingBrain
    {
        [SerializeField]
        private CountdownTimer m_detectionDuration;

        private ExplodingFlyingDrone m_drone;
        private bool m_hasFoundTarget;
        private bool m_isCharging;
        private Vector2 m_chargeDestination;

        public void SetTarget(IEnemyTarget target)
        {
            if (m_hasFoundTarget == false)
            {
                m_chargeDestination = target.position;
                m_hasFoundTarget = true;
            }
        }

        private void OnDetectionEnd(object sender, EventActionArgs eventArgs)
        {
            m_isCharging = true;
        }


        private void Awake()
        {
            m_drone = GetComponent<ExplodingFlyingDrone>();
            m_detectionDuration.CountdownEnd += OnDetectionEnd;
        }


        private void Update()
        {
            if (m_isCharging)
            {
                m_drone.ChargeTo(m_chargeDestination);
            }
            else if (m_hasFoundTarget)
            {
                m_detectionDuration.Tick(GameplaySystem.time.deltaTime);
            }
        }


    }
}