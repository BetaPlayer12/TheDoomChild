using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [SerializeField]
    public class TimelineEventClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField,HideLabel]
        public TimelineEventBehaviour info = new TimelineEventBehaviour();

        public ClipCaps clipCaps => ClipCaps.None;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimelineEventBehaviour>.Create(graph, info);
            playable.GetBehaviour();
            return playable;
        }

        public override double duration => 1;
    }
}

