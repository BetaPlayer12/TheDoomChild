using DChild.Gameplay.Pathfinding;
using UnityEngine;

namespace DChild.Gameplay
{

    [AddComponentMenu("DChild/Gameplay/AI/Movement/Navigation Agent")]
    public class NavigationAgent : PathFinderAgent
    {
        [SerializeField]
        private NavigationTracker m_navigation;
        [SerializeField]
        private MovementHandle2D m_movementHandle;

        public override Vector2 segmentDestination => m_navigation.currentPathSegment;
        public override bool hasPath => m_navigation.pathUpdated;

        public override void SetDestination(Vector2 position)
        {
            if (m_navigation.IsCurrentDestination(position) == false)
            {
                m_navigation.SetDestination(position);
            }
        }

        public override void Move(float speed)
        {
            m_movementHandle.MoveTowards(m_navigation.directionToPathSegment, speed);
        }

        public override void MoveTowardsForced(Vector2 direction, float speed) => m_movementHandle.MoveTowards(direction, speed);

        public override void Stop()
        {
            m_movementHandle.Stop();
        }
    }
}