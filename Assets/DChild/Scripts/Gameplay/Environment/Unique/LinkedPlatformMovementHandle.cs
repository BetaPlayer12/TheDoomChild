using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{

    [System.Serializable]
    public class LinkedPlatformMovementHandle
    {
        [System.Serializable]
        private class PlatformInfo
        {
            [SerializeField]
            private Transform m_platform;
            [SerializeField]
            private bool m_isMovementTracked;
            [SerializeField]
            private bool m_invertX;
            [SerializeField]
            private bool m_invertY;

            public Transform platform => m_platform;

            public bool invertX => m_invertX;
            public bool invertY => m_invertY;
            public bool isMovementTracked => m_isMovementTracked;
        }

        [SerializeField]
        private PlatformInfo[] m_platformInfos;

        private Transform[] m_platformArray;
        private List<Transform> m_trackedPlatformsList;
        private List<Vector3> m_previousPositionsList;

        private static Transform m_cacheMovedPlatform;
        private Vector3 m_cachedMovementVelocity;
        private bool m_cachedHasMoved;
        private bool m_cachedwasMovementOverallNegative;

        public Vector3 movementVelocityOnLastLinkedMovement => m_cachedMovementVelocity;
        public bool hasMovedOnLastLinkedMovement => m_cachedHasMoved;
        public bool wasLastMovementOverallNegative => m_cachedwasMovementOverallNegative;

        public int platformCount => m_platformArray.Length;
        public Transform[] GetPlatforms() => m_platformArray;

        public void SetPlatformPosition(int index, Vector2 position)
        {
            m_platformArray[index].position = position;
        }

        public void RecordTrackedPlatformPositions()
        {
            for (int i = 0; i < m_trackedPlatformsList.Count; i++)
            {
                m_previousPositionsList[i] = m_trackedPlatformsList[i].position;
            }
        }

        public void Initialize()
        {
            m_platformArray = new Transform[m_platformInfos.Length];
            m_trackedPlatformsList = new List<Transform>();
            m_previousPositionsList = new List<Vector3>();

            for (int i = 0; i < m_platformInfos.Length; i++)
            {
                var info = m_platformInfos[i];
                var platform = info.platform;
                m_platformArray[i] = platform;
                if (info.isMovementTracked)
                {
                    m_trackedPlatformsList.Add(platform);
                    m_previousPositionsList.Add(platform.position);
                }
            }
        }

        public void HandleLinkedMovement()
        {
            m_cachedHasMoved = HasMovementOccured(out m_cacheMovedPlatform, out m_cachedMovementVelocity);
            if (m_cachedHasMoved)
            {
                if (m_platformInfos.Length > 1)
                {
                    MoveAllPlatforms(m_cacheMovedPlatform, m_cachedMovementVelocity);
                }
                RecordTrackedPlatformPositions();
            }
        }

        #region Platform Movement
        private bool HasMovementOccured(out Transform movedPlatform, out Vector3 movementVelocity)
        {
            for (int i = 0; i < m_trackedPlatformsList.Count; i++)
            {
                var platform = m_trackedPlatformsList[i];
                var currentPosition = m_trackedPlatformsList[i].position;
                var previousPosition = m_previousPositionsList[i];
                if (currentPosition != previousPosition)
                {
                    var info = GetInfo(platform);
                    movementVelocity = (currentPosition - previousPosition);
                    m_cachedwasMovementOverallNegative = Mathf.Sign(movementVelocity.x) == -1 || Mathf.Sign(movementVelocity.x) == -1;
                    var wasOverallInverted = false;
                    if (info.invertX)
                    {
                        movementVelocity.x *= -1;
                        if(wasOverallInverted == false)
                        {
                            m_cachedwasMovementOverallNegative = !m_cachedwasMovementOverallNegative;
                            wasOverallInverted = true;
                        }
                    }
                    if (info.invertY)
                    {
                        movementVelocity.y *= -1;
                        if (wasOverallInverted == false)
                        {
                            m_cachedwasMovementOverallNegative = !m_cachedwasMovementOverallNegative;
                            wasOverallInverted = true;
                        }
                    }
                    movedPlatform = platform;
                    return true;
                }
            }
            movementVelocity = new Vector3();
            movedPlatform = null;
            return false;
        }

        private PlatformInfo GetInfo(Transform platform)
        {
            foreach (var platformInfo in m_platformInfos)
            {
                if (platformInfo.platform == platform)
                {
                    return platformInfo;
                }
            }

            return null;
        }

        private void MoveAllPlatforms(Transform movedPlatform, Vector3 movementVelocity)
        {
            foreach (var platformInfo in m_platformInfos)
            {
                var platform = platformInfo.platform;
                if (platform != movedPlatform)
                {
                    var movement = movementVelocity;
                    if (platformInfo.invertX)
                    {
                        movement.x *= -1;
                    }
                    if (platformInfo.invertY)
                    {
                        movement.y *= -1;
                    }
                    platform.position += movement;
                }
            }
        }
        #endregion
    }
}