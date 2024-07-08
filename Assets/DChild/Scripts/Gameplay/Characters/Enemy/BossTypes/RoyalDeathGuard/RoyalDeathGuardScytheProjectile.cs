using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardScytheProjectile : MonoBehaviour
    {
        [System.Serializable]
        public class FlightInfo
        {
            [System.Serializable]
            public class SegmentConfig
            {
                [SerializeField, Min(1f)]
                private float m_speed = 30f;
                [SerializeField, Min(0)]
                private float m_offset = 5f;
                [SerializeField]
                private bool m_useOffsetCurve;
                [SerializeField, ShowIf("m_useOffsetCurve")]
                private AnimationCurve m_offsetCurve;

                public float speed => m_speed;
                public float GetOffset(float time) => m_offset * (m_useOffsetCurve ? m_offsetCurve.Evaluate(time) : 1);
            }

            [SerializeField, Min(1f)]
            private float m_distance = 100f;
            [SerializeField]
            private SegmentConfig m_forwardInfo;
            [SerializeField]
            private float m_returnDelay;
            [SerializeField]
            private SegmentConfig m_returnInfo;


            public float distance => m_distance;
            public SegmentConfig forwardInfo => m_forwardInfo;
            public float returnDelay => m_returnDelay;
            public SegmentConfig returnInfo => m_returnInfo;
        }

        [SerializeField]
        private FlightInfo m_info;

        private bool m_isInFlight;
        public event EventAction<EventActionArgs> FlightDone;


        public void SetFlightInfo(FlightInfo info)
        {
            m_info = info;
        }

        [Button]
        public void ExecuteFlight(Vector3 startingPosition, Vector3 aimTowards)
        {
            if (m_isInFlight == false)
            {
                transform.position = startingPosition;

                Vector3 spitPos = startingPosition;
                Vector3 v_diff = (aimTowards - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                //if(transform.localScale.x > -1)
                //else
                //    transform.rotation = Quaternion.Euler(0f, 0f, -atan2 * Mathf.Rad2Deg);

                gameObject.SetActive(true);
                StartCoroutine(FlightRoutine());
            }
        }

        private IEnumerator FlightRoutine()
        {
            m_isInFlight = true;

            var startingPosition = transform.position;
            var destination = startingPosition + (transform.right * m_info.distance);

#if UNITY_EDITOR
            m_startingPosition = startingPosition;
            m_destination = destination;
#endif

            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f); //JUst for the eyes to catch up that it appeared;
            yield return MoveTo(startingPosition, destination, m_info.forwardInfo);
            yield return new WaitForSeconds(m_info.returnDelay);
            yield return MoveTo(destination, startingPosition, m_info.returnInfo);
            FlightDone?.Invoke(this, EventActionArgs.Empty);
            yield return new WaitForSeconds(0.1f); //JUst for the eyes to catch up that it disappeared;
            gameObject.SetActive(false);

            m_isInFlight = false;
        }

        private IEnumerator MoveTo(Vector3 from, Vector3 to, FlightInfo.SegmentConfig segmentConfig)
        {
            var travelTime = Vector3.Distance(from, to) / segmentConfig.speed;
            var maxLerpDelta = 1 / travelTime;
            var lerpValue = 0f;
            do
            {
                lerpValue += GameplaySystem.time.deltaTime * maxLerpDelta;
                transform.position = CalculatePosition(from, to, transform.up, segmentConfig, lerpValue);
                yield return null;
            } while (lerpValue < 1);
        }

        private Vector3 CalculatePosition(Vector3 from, Vector3 to, Vector3 upDirection, FlightInfo.SegmentConfig segmentConfig, float lerpValue)
        {
            upDirection.y = Mathf.Abs(upDirection.y); //When Projectile is Flipped due to Raycast Sensor requiremnt
            var position = Vector3.Lerp(from, to, lerpValue);
            position += upDirection * segmentConfig.GetOffset(lerpValue);
            return position;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private Vector3 m_startingPosition;
        private Vector3 m_destination;

        private void DrawTrajectory(Vector3 from, Vector3 to, FlightInfo.SegmentConfig segmentConfig)
        {
            var lerpValue = 0f;
            var previousPosition = CalculatePosition(from, to, transform.up, segmentConfig, lerpValue);
            ;
            do
            {
                lerpValue += 0.05f;
                var nextPosition = CalculatePosition(from, to, transform.up, segmentConfig, lerpValue);
                Gizmos.DrawLine(previousPosition, nextPosition);
                previousPosition = nextPosition;
            } while (lerpValue < 1);
        }

        private void DrawSpeedIndicators(Vector3 from, Vector3 to, FlightInfo.SegmentConfig segmentConfig, float size)
        {
            var travelTime = Vector3.Distance(from, to) / segmentConfig.speed;
            var maxLerpDelta = 1 / travelTime;
            var lerpValue = 0f;
            ;
            do
            {
                lerpValue += 0.05f * maxLerpDelta;
                var position = CalculatePosition(from, to, transform.up, segmentConfig, lerpValue);
                Gizmos.DrawCube(position, Vector3.one * size);
            } while (lerpValue < 1);
        }
#endif

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (m_isInFlight == false)
            {
                m_startingPosition = transform.position;
                m_destination = m_startingPosition + (transform.right * m_info.distance);
            }
            Gizmos.DrawLine(m_startingPosition, m_destination);

            Gizmos.color = Color.yellow;
            DrawTrajectory(m_startingPosition, m_destination, m_info.forwardInfo);
            DrawSpeedIndicators(m_startingPosition, m_destination, m_info.forwardInfo, 0.7f);

            Gizmos.color = Color.red;
            DrawTrajectory(m_destination, m_startingPosition, m_info.returnInfo);
            DrawSpeedIndicators(m_destination, m_startingPosition, m_info.returnInfo, 0.5f);
#endif

        }


    }
}