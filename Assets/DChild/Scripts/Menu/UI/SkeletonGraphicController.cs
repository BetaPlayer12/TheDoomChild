using Spine.Unity;
using UnityEngine;

namespace DChild.UI
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class SkeletonGraphicController : MonoBehaviour
    {
        [SerializeField, SpineAnimation]
        private string m_animationToPlay;
        [SerializeField]
        private bool m_loop;
        [SerializeField]
        private bool m_startAsFrozen;
        private SkeletonGraphic m_source;

        public void Play()
        {
            m_source.AnimationState.SetAnimation(0, m_animationToPlay, m_loop);
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

        private void Awake()
        {
            m_source = GetComponent<SkeletonGraphic>();
            if (m_startAsFrozen)
            {
                Freeze();
            }
        }
    }

}