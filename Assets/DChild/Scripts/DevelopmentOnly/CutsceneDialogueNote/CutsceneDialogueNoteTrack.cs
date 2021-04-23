using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChildDebug.Cutscene
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(CutsceneDialogueNoteClip))]
    public class CutsceneDialogueNoteTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in m_Clips)
            {
                var source = clip.asset as CutsceneDialogueNoteClip;
                var text = source.template.entryText;
                var textLenght = Mathf.Min(text.Length, 17);

                clip.displayName = source.template.entryText.Substring(0, textLenght) + (text.Length > 17 ? "..." : "");
            }
            return ScriptPlayable<CutsceneDialogueNoteBehaviour>.Create(graph, inputCount);
        }
    }
}
