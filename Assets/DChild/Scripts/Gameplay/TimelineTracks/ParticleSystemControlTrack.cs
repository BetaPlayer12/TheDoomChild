using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [TrackColor(0.4f, 0.7f, 0.6f)]
    [TrackClipType(typeof(ParticleSystemHandleClip))]
    [TrackBindingType(typeof(ParticleSystem))]
    public class ParticleSystemControlTrack : TrackAsset
    {
        public ParticleSystemHandleMixer template = new ParticleSystemHandleMixer();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            // Retrieve the reference to the track-bound particle system via the
            // director.
            var director = go.GetComponent<PlayableDirector>();
            var ps = director.GetGenericBinding(this) as ParticleSystem;

            // Create a track mixer playable and give the reference to the particle
            // system (it has to be initialized before OnGraphStart).
            var playable = ScriptPlayable<ParticleSystemHandleMixer>.Create(graph, template, inputCount);
            playable.GetBehaviour().particleSystem = ps;

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

            return playable;
        }
    }
}