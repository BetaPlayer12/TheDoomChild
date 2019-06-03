using UnityEngine;
using UnityEngine.Playables;

namespace Holysoft.UI.Timeline
{
    public class UIAnimationAsset : PlayableAsset
    {
        public ExposedReference<UIAnimation> m_animation;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UIAnimationPlayable>.Create(graph);
            var behaviour = playable.GetBehaviour();
            var component = m_animation.Resolve(graph.GetResolver());
            behaviour.Initialize(component);
            return playable;
        }
    }
}