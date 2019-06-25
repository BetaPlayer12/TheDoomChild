using UnityEngine;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;

namespace Refactor.DChild.Gameplay.Characters.AI
{
    public class TerrainPatrol : PatrolHandle
    {
        [SerializeField]
        private TerrainPatrolSensor m_sensors;

        private Vector2 m_moveDirection;
        private bool m_directionFound;

        public override void Patrol(MovementHandle2D movement, float speed, CharacterInfo characterInfo)
        {
            if (m_sensors.shouldTurnAround)
            {
                CallTurnRequest();
                m_directionFound = false;
            }
            else
            {
                if (m_directionFound == false)
                {
                    m_moveDirection = characterInfo.currentFacing == HorizontalDirection.Left ? Vector2.left : Vector2.right;
                    m_directionFound = true;
                }
                movement.MoveTowards(m_moveDirection, speed);
            }
        }
    }
}