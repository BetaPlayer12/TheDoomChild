using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GrappleSensor : MonoBehaviour
    {
        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        [SerializeField, MinValue(1f)]
        private float m_tolerance;
        private List<IGrappleObject> m_grappleCandidates;
        private List<IGrappleObject> m_cacheList;

        //Place these function somewhere else

        public IGrappleObject GetClosestObject()
        {
            if (m_grappleCandidates.Count > 0)
            {
                Vector2 currentPosition = transform.position;
                return GetClosestObjectTo(transform.position, m_grappleCandidates);
            }
            else
            {
                return null;
            }
        }

        public IGrappleObject GetNearestRightObjectFrom(IGrappleObject reference)
        {
            CacheGrapplePointsOnDirectionOf(reference, Direction.Right);
            if (m_cacheList.Count > 0)
            {
                return GetClosestObjectTo(reference.position, m_cacheList, true);
            }
            else
            {
                return null;
            }
        }

        public IGrappleObject GetNearestLeftObjectFrom(IGrappleObject reference)
        {
            CacheGrapplePointsOnDirectionOf(reference, Direction.Left);
            if (m_cacheList.Count > 0)
            {
                return GetClosestObjectTo(reference.position, m_cacheList, true);
            }
            else
            {
                return null;
            }
        }

        public IGrappleObject GetNearestTopObjectFrom(IGrappleObject reference)
        {
            CacheGrapplePointsOnDirectionOf(reference, Direction.Up);
            if (m_cacheList.Count > 0)
            {
                return GetClosestObjectTo(reference.position, m_cacheList, false);
            }
            else
            {
                return null;
            }
        }

        public IGrappleObject GetNearestBottomObjectFrom(IGrappleObject reference)
        {
            CacheGrapplePointsOnDirectionOf(reference, Direction.Down);
            if (m_cacheList.Count > 0)
            {
                return GetClosestObjectTo(reference.position, m_cacheList, false);
            }
            else
            {
                return null;
            }
        }

        private void CacheGrapplePointsOnDirectionOf(IGrappleObject reference, Direction direction)
        {
            m_cacheList.Clear();
            var referencePosition = reference.position;
            for (int i = 0; i < m_grappleCandidates.Count; i++)
            {
                if (m_grappleCandidates[i] != reference)
                {
                    Vector2 toCandidate = m_grappleCandidates[i].position - referencePosition;
                    if (Vector2.Distance(referencePosition, m_grappleCandidates[i].position) <= m_tolerance)
                    {
                        switch (direction)
                        {
                            case Direction.Up:
                                if (toCandidate.normalized.y > 0)
                                {
                                    m_cacheList.Add(m_grappleCandidates[i]);
                                }
                                break;

                            case Direction.Down:
                                if (toCandidate.normalized.y < 0)
                                {
                                    m_cacheList.Add(m_grappleCandidates[i]);
                                }
                                break;

                            case Direction.Right:
                                if (toCandidate.normalized.x > 0)
                                {
                                    m_cacheList.Add(m_grappleCandidates[i]);
                                }
                                break;

                            case Direction.Left:
                                if (toCandidate.normalized.x < 0)
                                {
                                    m_cacheList.Add(m_grappleCandidates[i]);
                                }
                                break;
                        }
                    }
                }
            }
        }

        private IGrappleObject GetClosestObjectTo(Vector2 referencePosition, List<IGrappleObject> list)
        {
            float closestDistance = Vector2.Distance(list[0].position, referencePosition);
            int closestIndex = 0;
            for (int i = 1; i < list.Count; i++)
            {
                var distance = Vector2.Distance(list[i].position, referencePosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return list[closestIndex];
        }

        private IGrappleObject GetClosestObjectTo(Vector2 referencePosition, List<IGrappleObject> list, bool useX)
        {
            int closestIndex = 0;
            if (useX)
            {
                float closestDistance = Mathf.Abs(list[0].position.x - referencePosition.x);
                for (int i = 1; i < list.Count; i++)
                {
                    var distance = Mathf.Abs(list[i].position.x - referencePosition.x);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }
            }
            else
            {
                float closestDistance = Mathf.Abs(list[0].position.y - referencePosition.y);
                for (int i = 1; i < list.Count; i++)
                {
                    var distance = Mathf.Abs(list[i].position.y - referencePosition.y);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }
            }

            return list[closestIndex];
        }

        private void Awake()
        {
            m_grappleCandidates = new List<IGrappleObject>();
            m_cacheList = new List<IGrappleObject>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Droppable"))
            {
                m_grappleCandidates.Add(collision.GetComponent<IGrappleObject>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Droppable"))
            {
                m_grappleCandidates.Remove(collision.GetComponent<IGrappleObject>());
            }
        }
    }
}