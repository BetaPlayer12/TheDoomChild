﻿// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(DialogueClip))]
    [TrackBindingType(typeof(PlayableDirector))]
    public class DialogueTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in GetClips())
            {
                var myAsset = clip.asset as DialogueClip;
                if (myAsset)
                {
                    myAsset.end = clip.end;
#if UNITY_EDITOR
                    var text = myAsset.template.noteEntryText;
                    var textLenght = Mathf.Min(text.Length, 17);
                    clip.displayName = myAsset.template.noteEntryText.Substring(0, textLenght) + (text.Length > 17 ? "..." : "");
#endif
                }
            }

            return ScriptPlayable<DialogueMixBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif
#endif
