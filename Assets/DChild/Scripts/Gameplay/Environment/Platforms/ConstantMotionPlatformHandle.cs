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
            private float m_startDelay;
            [SerializeField, MinValue(0)]
            private float m_movementDelay;
            [SerializeField]
            private bool m_reverseMovement;
            [SerializeField]
            private bool m_loop;

            public MovingPlatform instance => m_instance;

            public float startDelay => m_startDelay;

            public float movementDelay => m_movementDelay;
            public bool reverseMovement => m_reverseMovement;

            public bool loop => m_loop;
        }

        [SerializeField, OnValueChanged("OnUpdateDestinationChange")]
        private bool m_updatePlatformDestination;
        [SerializeField]
        private HandleInfo[] m_platforms;
        private Dictionary<int, HandleInfo> m_dictionary;
        private HandleInfo m_cacheHandleInfo;

        public bool updatePlatformDestination
        {
            set
            {
                m_updatePlatformDestination = value;
                OnUpdateDestinationChange();
            }
        }

        private List<MovingPlatform.UpdateEventArgs> m_waitingForUpdate;

        public void SetPlatformActivity(bool isActive)
        {
            for (int i = 0; i < m_platforms.Length; i++)
            {
                m_platforms[i].instance.enabled = isActive;
            }
        }

        private void OnDestinationReached(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            if (m_updatePlatformDestination)
            {
                HandleDestinationUpdate(eventArgs);
            }
            else
            {
                m_waitingForUpdate.Add(eventArgs);
            }
        }

        private void HandleDestinationUpdate(MovingPlatform.UpdateEventArgs eventArgs)
        {
            m_cacheHandleInfo = m_dictionary[eventArgs.instance];
            if (eventArgs.isGoingForward)
            {
                var atTheEnd = eventArgs.currentWaypointIndex == (eventArgs.waypointCount - 1);
                if (atTheEnd && m_cacheHandleInfo.loop)
                {
                    m_cacheHandleInfo.instance.GoDestination(0, false);
                }
                else
                {
                    MovePlatform(m_cacheHandleInfo.instance, m_cacheHandleInfo.movementDelay, !atTheEnd);
                }

            }
            else
            {
                var atTheEnd = eventArgs.currentWaypointIndex == 0;
                if (atTheEnd && m_cacheHandleInfo.loop)
                {
                    m_cacheHandleInfo.instance.GoDestination(eventArgs.waypointCount - 1, false);
                }
                else
                {
                    MovePlatform(m_cacheHandleInfo.instance, m_cacheHandleInfo.movementDelay, atTheEnd);
                }
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

        private void OnUpdateDestinationChange()
        {
            if (m_updatePlatformDestination)
            {
                for (int i = m_waitingForUpdate.Count - 1; i >= 0; i--)
                {
                    HandleDestinationUpdate(m_waitingForUpdate[i]);
                }
                m_waitingForUpdate.Clear();
            }
        }

        private void Start()
        {
            m_waitingForUpdate = new List<MovingPlatform.UpdateEventArgs>();
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

                    if (m_cacheHandleInfo.startDelay == 0)
                    {
                        if (m_cacheHandleInfo.reverseMovement)
                        {
                            m_cacheHandleInfo.instance.GoToPreviousWaypoint();
                        }
                        else
                        {
                            m_cacheHandleInfo.instance.GoToNextWayPoint();
                        }
                    }
                    else
                    {
                        StartCoroutine(DoDelayedMove(m_cacheHandleInfo.instance, m_cacheHandleInfo.startDelay, m_cacheHandleInfo.reverseMovement == false));
                    }
                }
            }
        }

    }
}




