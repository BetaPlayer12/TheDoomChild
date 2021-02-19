using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChildDebug.Cutscene
{
    [Serializable]
    public class CutsceneDialogueNoteClip : PlayableAsset, ITimelineClipAsset
    {
        public CutsceneDialogueNoteBehaviour template = new CutsceneDialogueNoteBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CutsceneDialogueNoteBehaviour>.Create(graph, template);
            CutsceneDialogueNoteBehaviour clone = playable.GetBehaviour();
            return playable;
        }
    }
}
