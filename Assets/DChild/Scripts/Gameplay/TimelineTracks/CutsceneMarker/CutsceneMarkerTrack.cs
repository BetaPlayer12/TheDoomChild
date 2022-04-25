using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChildDebug.Cutscene
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(CutsceneMarkerClip))]
    [TrackBindingType(typeof(PlayableDirector))]
    public class CutsceneMarkerTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in GetClips())
            {
                var myAsset = clip.asset as CutsceneMarkerClip;
                if (myAsset)
                {
                    myAsset.clipStart = clip.start;
                    myAsset.clipEnd = clip.end;
                }
            }

            return ScriptPlayable<CutsceneMarkerMixBehaviour>.Create(graph, inputCount);
        }
    }
}
