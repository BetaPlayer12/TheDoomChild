using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WeightPlatformConnection : MonoBehaviour
    {
        [SerializeField]
        private WeightedPlatform m_connectedTo;
        [SerializeField]
        private AnimationCurve m_lerpTranslation = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        [SerializeField, HorizontalGroup("StartPosition")]
        private Vector3 m_startingPosition;
        [SerializeField, HorizontalGroup("ActivatedPosition")]
        private Vector3 m_activatedPostion;


        private void SyncWithConnectedPlatfrom()
        {
            var translatedLerp = m_lerpTranslation.Evaluate(m_connectedTo.lerpValue);
            transform.position = Vector3.Lerp(m_startingPosition, m_activatedPostion, translatedLerp);
        }

        private void Awake()
        {
            SyncWithConnectedPlatfrom();
        }

        private void FixedUpdate()
        {
            SyncWithConnectedPlatfrom();
        }

#if UNITY_EDITOR
        [Button("Use Current Position"), HorizontalGroup("StartPosition")]
        private void SetCurrentPositionAsStartPosition() => m_startingPosition = transform.position;

        [Button("Use Current Position"), HorizontalGroup("ActivatedPosition")]
        private void SetCurrentPositionAsActivatedPosition() => m_activatedPostion = transform.position;
#endif

    }
}