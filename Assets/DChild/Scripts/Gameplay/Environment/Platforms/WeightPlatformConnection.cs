using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WeightPlatformConnection : MonoBehaviour
    {
        [SerializeField]
        private WeightedPlatform m_connectedTo;
        [SerializeField, HorizontalGroup("StartPosition")]
        private Vector3 m_startingPosition;
        [SerializeField, HorizontalGroup("ActivatedPosition")]
        private Vector3 m_activatedPostion;

        private void Awake()
        {
            transform.position = Vector3.Lerp(m_startingPosition, m_activatedPostion, m_connectedTo.lerpValue);
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(m_startingPosition, m_activatedPostion, m_connectedTo.lerpValue);
        }

#if UNITY_EDITOR
        [Button("Use Current Position"), HorizontalGroup("StartPosition")]
        private void SetCurrentPositionAsStartPosition() => m_startingPosition = transform.position;

        [Button("Use Current Position"), HorizontalGroup("ActivatedPosition")]
        private void SetCurrentPositionAsActivatedPosition() => m_activatedPostion = transform.position;
#endif

    }
}