using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public static class Raycaster
    {
        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        /// <summary>
        /// Allows Collision To These Layers Only,
        /// If you are not using a LayerMask use LayerMask.GetMask() instead
        /// </summary>
        /// <param name="layer"></param>
        public static void SetLayerMask(int layer) => m_contactFilter.SetLayerMask(layer);

        /// <summary>
        /// Copies Allowed Collision of Layer as set in the Physics Matrix
        /// </summary>
        /// <param name="layer"></param>
        public static void SetLayerCollisionMask(int layer) => m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(layer));

        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_hitResults = new RaycastHit2D[16];
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                } 
            }
#endif
            return m_hitResults;
        }


        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * 50f, Color.cyan, 1f);
                }
            }
#endif

            return m_hitResults;
        }

        public static RaycastHit2D[] CastAll(Vector2 origin, Vector2 direction, float distance, bool debugMode = false)
        {
            Initialize();
#if UNITY_EDITOR
            if (debugMode)
            {
                Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
            }
#endif
            return Physics2D.RaycastAll(origin, direction, distance, m_contactFilter.layerMask);
        }

        public static bool SearchCast(Vector2 origin, Vector2 target, LayerMask layerMask, out RaycastHit2D[] hitbuffers, float yOffset = 3f, int increments = 1)
        {
            int hitCount = 0;
            var toRayTarget = target - origin;
            Raycaster.SetLayerMask(layerMask);
            hitbuffers = Raycaster.Cast(origin, toRayTarget.normalized, toRayTarget.magnitude, true, out hitCount);
            var isInterrupted = IsInterrupted();
            if (isInterrupted)
            {
                for (int i = 1; i <= increments; i++)
                {
                    var hasFound = OffsetSearch(toRayTarget, i, out hitbuffers);
                    if (hasFound)
                    {
                        return true;
                    }
                    else if (i == increments)
                    {
                        return hasFound;
                    }
                }
                return !isInterrupted;
            }
            return true;

            bool IsInterrupted() => hitCount > 0;
            bool OffsetSearch(Vector2 searchTarget, int increment, out RaycastHit2D[] buffer)
            {
                var offsetTarget = searchTarget;
                offsetTarget.y += yOffset * increment;
                buffer = Raycaster.Cast(origin, offsetTarget.normalized, offsetTarget.magnitude, true, out hitCount);
                if (IsInterrupted())
                {
                    offsetTarget = searchTarget;
                    offsetTarget.y -= yOffset;
                    buffer = Raycaster.Cast(origin, offsetTarget.normalized, offsetTarget.magnitude, true, out hitCount);
                    return !IsInterrupted();
                }
                return true;
            }
        }
    }
}