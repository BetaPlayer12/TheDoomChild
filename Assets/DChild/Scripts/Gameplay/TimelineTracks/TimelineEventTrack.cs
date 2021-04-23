using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [TrackClipType(typeof(TimelineEventClip))]
    [ExcludeFromPreset]
    public class TimelineEventTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var scriptPlayable = ScriptPlayable<TimelineEventMixerBehaviour>.Create(graph, inputCount);
            return scriptPlayable;
        }
    }
}

