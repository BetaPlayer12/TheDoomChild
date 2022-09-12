using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChildDebug.Cutscene
{
    [SerializeField]
    public class CutsceneMarkerClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField, HideLabel]
        private CutsceneMarkerBehaviour m_template = new CutsceneMarkerBehaviour();

        [ReadOnly]
        public double clipStart;
        [ReadOnly]
        public double clipEnd;

        public CutsceneMarkerBehaviour template => m_template;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CutsceneMarkerBehaviour>.Create(graph, m_template);
            CutsceneMarkerBehaviour clone = playable.GetBehaviour();
            clone.clipStart = clipStart;
            clone.clipEnd = clipEnd;
            return playable;
        }
    }
}
