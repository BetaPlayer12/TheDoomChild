// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{
    [SerializeField]
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private DialogueBehaviour m_template;

        public double end;

        public DialogueBehaviour template => m_template;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, m_template);
            DialogueBehaviour clone = playable.GetBehaviour();
            clone.end = end;
            return playable;
        }
    }
}
#endif
#endif
