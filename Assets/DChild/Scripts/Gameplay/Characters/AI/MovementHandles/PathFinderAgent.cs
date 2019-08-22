using DChild.Gameplay.Pathfinding;
using UnityEngine;

namespace DChild.Gameplay
{
    public class PathFinderAgent : MonoBehaviour
    {
        [SerializeField]
        private NavigationTracker m_navigation;
        [SerializeField]
        private MovementHandle2D m_movementHandle;

        public Vector2 segmentDestination => m_navigation.currentPathSegment;
        public bool hasPath => m_navigation.pathUpdated;

        public void SetDestination(Vector2 position)
        {
            if (m_navigation.IsCurrentDestination(position) == false)
            {
                m_navigation.SetDestination(position);
            }
        }

        public void Move(float speed)
        {
            m_movementHandle.MoveTowards(m_navigation.directionToPathSegment, speed);
        }

        public void MoveTowardsForced(Vector2 direction, float speed) => m_movementHandle.MoveTowards(direction, speed);

        public void Stop()
        {
            m_movementHandle.Stop();
        }
    }
}