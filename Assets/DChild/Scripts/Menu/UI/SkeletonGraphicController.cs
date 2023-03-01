using Spine.Unity;
using UnityEngine;

namespace DChild.UI
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class SkeletonGraphicController : MonoBehaviour
    {
        [SerializeField, SpineAnimation]
        private string m_animationToPlay;
        [SerializeField, Min(0)]
        private float m_delay;
        [SerializeField]
        private bool m_loop;
        [SerializeField]
        private bool m_startAsFrozen;
        private SkeletonGraphic m_source;

        public void Play()
        {
            var track = m_source.AnimationState.SetAnimation(0, m_animationToPlay, m_loop);
            track.Delay = m_delay;
        }

        public void Stop()
        {
            m_source.AnimationState.SetEmptyAnimation(0, 0);
        }

        public void Freeze()
        {
            m_source.freeze = true;
        }

        public void Unfreeze()
        {
            m_source.freeze = false;
        }

        public void Reset()
        {
            Stop();
            Play();
            Freeze();
        }

        private void Awake()
        {
            m_source = GetComponent<SkeletonGraphic>();
            if (m_startAsFrozen)
            {
                Play();
                Freeze();
            }
        }
    }

}