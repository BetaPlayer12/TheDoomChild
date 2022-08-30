/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment
{
    public abstract class DistanceShift<TargetType> : MonoBehaviour
    {
        private enum CalculationType
        {
            Vector_Distance,
            X_Distance,
            Y_Distance
        }

        [SerializeField, Range(0, 1), Tooltip("When Reference is in minDistanceThreshold")]
        private float m_fromAlpha = 0;
        [SerializeField, Range(0, 1), Tooltip("When Reference is in maxDistanceThreshold")]
        private float m_toAlpha = 1f;

        [SerializeField]
        private float m_minDistanceThreshold = 5f;
        [SerializeField]
        private float m_maxDistanceThreshold = 20f;
        [SerializeField]
        private CalculationType m_calculationType = CalculationType.Vector_Distance;
        [SerializeField]
        private Transform m_spriteCenter;
        [SerializeField, HideIf("m_distanceToReferenceIsPlayer")]
        private Transform m_distanceToReference;
        [SerializeField, HideInPlayMode]
        private bool m_distanceToReferenceIsPlayer;

        protected abstract TargetType[] targets { get; }
        public Vector3 spriteCenter => m_spriteCenter?.position ?? transform.position;

        protected abstract void SetShiftValue(TargetType renderer, float value);

        private float GetCalculatedDistance(Vector3 playerPosition, CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.Vector_Distance:
                    return Vector2.Distance(spriteCenter, playerPosition);
                case CalculationType.X_Distance:
                    return Mathf.Abs(playerPosition.x - spriteCenter.x);
                case CalculationType.Y_Distance:
                    return Mathf.Abs(playerPosition.y - spriteCenter.y);
            }
            return 0;
        }

        private float CalculateLerpValue(float distance)
        {
            if (distance <= m_minDistanceThreshold)
            {
                return 0;
            }
            else if (distance <= m_minDistanceThreshold)
            {
                return 1f;
            }
            else
            {
                var thresholdReleativeDistance = m_maxDistanceThreshold - m_minDistanceThreshold;
                var relativeDistanceToMinThreshold = distance - m_minDistanceThreshold;
                return relativeDistanceToMinThreshold / thresholdReleativeDistance;
            }
        }

        private float CalculateAlphaShift(Vector3 targetPosition)
        {
            var distance = GetCalculatedDistance(targetPosition, m_calculationType);
            var lerpValue = CalculateLerpValue(distance);
            return Mathf.Lerp(m_fromAlpha, m_toAlpha, lerpValue);
        }


        public void LateUpdate()
        {
            float alpha = CalculateAlphaShift(m_distanceToReference.position);

            for (int i = 0; i < targets.Length; i++)
            {
                SetShiftValue(targets[i], alpha);
            }
        }

        private void Reset()
        {
            m_fromAlpha = 0;
            m_toAlpha = 1f;
            m_minDistanceThreshold = 5;
            m_maxDistanceThreshold = 20f;
            m_distanceToReferenceIsPlayer = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (m_distanceToReferenceIsPlayer)
            {
                if (Application.isPlaying)
                {
                    var position = GameplaySystem.playerManager.player.character.centerMass.position;
                    Gizmos.DrawLine(spriteCenter, position);
#if UNITY_EDITOR
                    Handles.Label(transform.position, $"{Vector2.Distance(spriteCenter, position)}");
#endif
                }
            }
            else if (m_distanceToReference != null)
            {
                var position = m_distanceToReference.position;
                Gizmos.DrawLine(spriteCenter, position);
#if UNITY_EDITOR
                Handles.Label(transform.position, $"{Vector2.Distance(spriteCenter, position)}");
#endif
            }

            Gizmos.color = new Color(0.5f, 0.5f, 0, 0.25f);
            Gizmos.DrawSphere(spriteCenter, m_minDistanceThreshold);
            Gizmos.color = new Color(0.5f, 0, 0.5f, 0.25f);
            Gizmos.DrawSphere(spriteCenter, m_maxDistanceThreshold);
        }


        protected virtual void Start()
        {
            if (m_distanceToReferenceIsPlayer)
            {
                m_distanceToReference = GameplaySystem.playerManager.player.character.centerMass;
            }
        }
    }
}