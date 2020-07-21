using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{

    [System.Serializable]
    public class ParticleSystemHandleBehaviour : PlayableBehaviour
    {
        public enum HandleType
        {
            PlayWithinClip,
            StopAtClip
        }

        [SerializeField]
        private HandleType m_action;
        [SerializeField]
        private bool m_affectChildren;

        [HideInInspector]
        public double clipStart;
        [HideInInspector]
        public double clipEnd;

        public HandleType action => m_action;
        public bool affectChildren => m_affectChildren;
    }
}