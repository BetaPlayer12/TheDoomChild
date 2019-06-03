using UnityEngine;
using UnityEngine.Playables;

namespace Holysoft.UI.Timeline
{
    public class UIAnimationPlayable : PlayableBehaviour
    {
        private UIAnimation m_animation;

        public void Initialize(UIAnimation animation)
        {
            m_animation = animation;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            m_animation?.Play();
        }

        public override void OnGraphStop(Playable playable)
        {
            m_animation?.Stop();
        }
    }
}