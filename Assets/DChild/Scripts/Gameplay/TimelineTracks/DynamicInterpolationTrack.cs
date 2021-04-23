using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [TrackClipType(typeof(DynamicInterpolationClip))]
    [TrackBindingType(typeof(Transform))]
    [ExcludeFromPreset]
    public class DynamicInterpolationTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {

            var clips = GetClips();
            foreach (var clip in clips)
            {
                var validClip = clip.asset as DynamicInterpolationClip;
                if (validClip)
                {
                    validClip.clipStart = clip.start;
                    validClip.clipEnd = clip.end;
                }
            }


            var scriptPlayable = ScriptPlayable<DynamicInterpolationMixerBehaviour>.Create(graph, inputCount);
            return scriptPlayable;
        }
    }
}

