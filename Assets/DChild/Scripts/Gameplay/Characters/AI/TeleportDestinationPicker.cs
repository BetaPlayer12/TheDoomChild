using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class TeleportDestinationPicker
    {
        [SerializeField]
        private LayerMask m_toAvoid;
        [SerializeField]
        private LayerMask m_toLandOn;
        [SerializeField]
        private LayerMask m_heightCheck;
        [SerializeField, MinValue(1)]
        private int m_maxTrials;
        [SerializeField, MinValue(1)]
        private float m_distanceCast;
        [SerializeField, MinValue(0.001f)]
        private float m_offsetIncrements;
        [SerializeField]
        private MultiRaycast m_multiRaycast;

        private RaycastHit2D[] m_hitBuffer;

        public void Initialize()
        {
            m_multiRaycast.Initialize();
        }

        public void SetDistanceCast(float distanceCast) => m_distanceCast = Mathf.Max(1, distanceCast);
        public void SetDistanceCast(LayerMask toAvoidLayers) => m_toAvoid = toAvoidLayers;

        public bool CanTeleportTo(Vector2 destination, float entityWidth, float entityHeight)
        {
            //increasing height of origin to get better results?
            destination.y += 1;

            //Check if its actually safe to land based on the width of the entity
            m_multiRaycast.SetCast(m_toAvoid, true);
            m_multiRaycast.SetCastDistance(m_distanceCast);
            m_multiRaycast.Cast(destination, Vector2.down);
            if (m_multiRaycast.isDetecting)
            {
                return false;
            }
            else
            {
                //Check if the destination is high enough for it to teleport
                var heightCheckOrigin = destination;
                heightCheckOrigin.y += entityHeight - 1;
                m_multiRaycast.SetCast(m_heightCheck, true);
                m_multiRaycast.SetCastDistance(entityHeight);
                m_multiRaycast.Cast(heightCheckOrigin, Vector2.down);
                if (m_multiRaycast.isDetecting)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //Teleport relative to target
        public Vector2 GetDestination(CombatCharacterVisualInfo target, RelativeDirection relativeDirection, float entityWidth, float entityHeight)
        {
            float incrementSign;
            if (target.currentFacingDirection == HorizontalDirection.Left)
            {
                incrementSign = (relativeDirection == RelativeDirection.Front ? -1 : 1);
            }
            else
            {
                incrementSign = (relativeDirection == RelativeDirection.Front ? 1 : -1);
            }

            float increments = incrementSign * m_offsetIncrements;
            Vector2 destinationOrigin = target.position;
            destinationOrigin.x += increments * (entityWidth / 2 + 1);
            int hitCount = 0;
            bool searchForDestination = true;
            int trialCount = 0;
            do
            {
                destinationOrigin.x += increments;
                Raycaster.SetLayerMask(m_toLandOn);
                m_hitBuffer = Raycaster.Cast(destinationOrigin, Vector2.down, m_distanceCast, true, out hitCount);
                if (hitCount > 0)
                {
                    int avoidCollisionHitCount;
                    Raycaster.SetLayerMask(m_toAvoid);
                    Raycaster.Cast(destinationOrigin, Vector2.down, m_hitBuffer[0].distance, true, out avoidCollisionHitCount);
                    if (avoidCollisionHitCount == 0)
                    {
                        searchForDestination = !CanTeleportTo(destinationOrigin, entityWidth, entityHeight);
                    }
                    else
                    {
                        searchForDestination = true;
                    }
                }
                else
                {
                    searchForDestination = true;
                }
                trialCount++;
            } while (searchForDestination && trialCount < m_maxTrials);
            return m_hitBuffer[0].point;
        }
    }
}