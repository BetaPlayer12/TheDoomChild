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
        private uint m_seed;

        [SerializeField]
        private HandleType m_action;

        [HideInInspector]
        public double clipStart;
        [HideInInspector]
        public double clipEnd;

        public HandleType action => m_action;
        public uint seed => m_seed;
    }
}