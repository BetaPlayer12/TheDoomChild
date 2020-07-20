using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{

    [System.Serializable]
    public class DynamicInterpolationBehaviour : PlayableBehaviour
    {
        [SerializeField]
        private bool m_moveX;
        [SerializeField]
        private bool m_moveY;
        [SerializeField]
        private Vector2 m_destination;
        [SerializeField]
        private AnimationCurve m_interpolation;

        [HideInInspector]
        public double clipStart;
        [HideInInspector]
        public double clipEnd;

        public bool moveX => m_moveX;
        public bool moveY => m_moveY;
        public Vector2 destination => m_destination;
        public AnimationCurve interpolation => m_interpolation;
    }
}

