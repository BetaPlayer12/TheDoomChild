// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{
    public class PlaySequenceShortcutClip : PlayableAsset, ITimelineClipAsset
    {
        public PlaySequenceShortcutBehaviour template = new PlaySequenceShortcutBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PlaySequenceShortcutBehaviour>.Create(graph, template);
            PlaySequenceShortcutBehaviour clone = playable.GetBehaviour();
            clone.m_reference = owner.name;
            return playable;
        }
    }
}
#endif
#endif
