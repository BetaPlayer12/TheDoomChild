using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace DChildDebug.Cutscene
{
    [Serializable]
    public class CutsceneDialogueNoteBehaviour : PlayableBehaviour
    {
        [Tooltip("The conversation to start.")]
        [ConversationPopup]
        public string conversation;
        [Tooltip("Dialogue entry to jump to.")]
        public int entryID;
        [HideInInspector]
        public string entryText;
    }
}
