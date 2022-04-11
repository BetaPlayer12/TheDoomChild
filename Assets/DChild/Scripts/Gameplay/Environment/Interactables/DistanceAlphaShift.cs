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
    public class DistanceAlphaShift : MonoBehaviour
    {
        [SerializeField, Range(0, 1), Tooltip("When Player is in minDistanceThreshold")]
        private float m_fromAlpha = 0;
        [SerializeField, Range(0, 1), Tooltip("When Player is in maxDistanceThreshold")]
        private float m_toAlpha = 1f;

        [SerializeField]
        private float m_minDistanceThreshold = 5f;
        [SerializeField]
        private float m_maxDistanceThreshold = 20f;

        [SerializeField]
        private Transform m_spriteCenter;
        [SerializeField, HideIf("m_distanceToReferenceIsPlayer")]
        private Transform m_distanceToReference;
        [SerializeField, HideInPlayMode]
        private bool m_distanceToReferenceIsPlayer;
        [SerializeField]
        private SpriteRenderer[] m_renderers;

        public Vector3 spriteCenter => m_spriteCenter?.position ?? transform.position;

        private void SetRendererAlpha(SpriteRenderer renderer, float value)
        {
            var color = renderer.color;
            color.a = value;
            renderer.color = color;
        }

        public void LateUpdate()
        {
            var playerPosition = m_distanceToReference.position;
            var distance = Vector2.Distance(spriteCenter, playerPosition);

            var lerpValue = 0f;
            if (distance <= m_minDistanceThreshold)
            {
                lerpValue = 0;
            }
            else if (distance <= m_minDistanceThreshold)
            {
                lerpValue = 1f;
            }
            else
            {
                var thresholdReleativeDistance = m_maxDistanceThreshold - m_minDistanceThreshold;
                var relativeDistanceToMinThreshold = distance - m_minDistanceThreshold;
                lerpValue = relativeDistanceToMinThreshold / thresholdReleativeDistance;
            }

            var alpha = Mathf.Lerp(m_fromAlpha, m_toAlpha, lerpValue);

            for (int i = 0; i < m_renderers.Length; i++)
            {
                SetRendererAlpha(m_renderers[i], alpha);
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
            else if(m_distanceToReference != null)
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


        private void Start()
        {
            if (m_distanceToReferenceIsPlayer)
            {
                m_distanceToReference = GameplaySystem.playerManager.player.character.centerMass;
            }
        }
    }
}