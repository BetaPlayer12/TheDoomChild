using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Constant Motion Platform")]
    public class ConstantMotionPlatformHandle : MonoBehaviour
    {
        [System.Serializable]
        public class HandleInfo
        {
            [SerializeField, InlineEditor]
            private MovingPlatform m_instance;
            [SerializeField, MinValue(0)]
            private int m_movementDelay;
            [SerializeField]
            private bool m_reverseMovement;

            public MovingPlatform instance { get => m_instance; }
            public int movementDelay { get => m_movementDelay; }
            public bool reverseMovement => m_reverseMovement;

        }

        [SerializeField]
        private HandleInfo[] m_platforms;
        private Dictionary<int, HandleInfo> m_dictionary;
        private HandleInfo m_cacheHandleInfo;

        private void Start()
        {
            m_dictionary = new Dictionary<int, HandleInfo>();
            m_cacheHandleInfo = null;
            for (int i = 0; i < m_platforms.Length; i++)
            {
                m_cacheHandleInfo = m_platforms[i];
                var ID = m_cacheHandleInfo.instance.GetInstanceID();
                if (m_dictionary.ContainsKey(ID) == false)
                {
                    m_dictionary.Add(ID, m_cacheHandleInfo);
                    m_cacheHandleInfo.instance.DestinationReached += OnDestinationReached;
                    if (m_cacheHandleInfo.reverseMovement)
                    {
                        m_cacheHandleInfo.instance.GoToPreviousWaypoint();
                    }
                    else
                    {
                        m_cacheHandleInfo.instance.GoToNextWayPoint();
                    }
                }
            }
        }

        private void OnDestinationReached(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            m_cacheHandleInfo = m_dictionary[eventArgs.instance];
            if (eventArgs.isGoingForward)
            {

                MovePlatform(m_cacheHandleInfo.instance, m_cacheHandleInfo.movementDelay, eventArgs.currentWaypointIndex != (eventArgs.waypointCount-1));
            }
            else
            {
                MovePlatform(m_cacheHandleInfo.instance, m_cacheHandleInfo.movementDelay, eventArgs.currentWaypointIndex == 0);
            }
        }

        private void MovePlatform(MovingPlatform platform, float delay, bool moveForward)
        {
            if (delay == 0)
            {
                if (moveForward)
                {
                    platform.GoToNextWayPoint();
                }
                else
                {
                    platform.GoToPreviousWaypoint();
                }
            }
            else
            {
                StartCoroutine(DoDelayedMove(platform, delay, moveForward));
            }
        }

        private IEnumerator DoDelayedMove(MovingPlatform platform, float delay, bool moveForward)
        {
            yield return new WaitForSeconds(delay);
            if (moveForward)
            {
                platform.GoToNextWayPoint();
            }
            else
            {
                platform.GoToPreviousWaypoint();
            }
        }
    }
}




