// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PixelCrushers.DialogueSystem
{
    [System.Serializable]
    public class DialogueBehaviour : PlayableBehaviour
    {
        public enum StartBehaviour
        {
            None,
            StartDialogue,
            ChangePanel,
        }

        public enum EndBehaviour
        {
            None,
            EndConversation
        }

        [SerializeField]
        private DialogueDatabase m_referenceDatabase;

        public StartBehaviour startBehaviour;

        [Tooltip("The conversation to start.")]
        [ConversationPopup(true), ShowIf("@startBehaviour == StartBehaviour.StartDialogue"), Indent]
        public string conversation;

        [Tooltip("Jump to a specific dialogue entry instead of starting from the conversation's START node."), ShowIf("@startBehaviour == StartBehaviour.StartDialogue"), Indent]
        public bool jumpToSpecificEntry;

        [Tooltip("Dialogue entry to jump to."), ShowIf("@startBehaviour == StartBehaviour.StartDialogue && jumpToSpecificEntry"), Indent, LabelText("       "), DisableIf("jumpToSpecificEntry")]
        public int entryID;

        public EndBehaviour endBehaviour;
        public double waitForInput = 0.5f;


        [Title("Note")]
#if UNITY_EDITOR
        [ConversationPopup(true)]
        public string noteConversation;
        [Tooltip("Dialogue entry to jump to."), LabelText("       "), DisableIf("@noteConversation != null")]
        public int noteEntryID;
        [ShowIf("@noteConversation != null"), DisableIf("@noteConversation != null")]
        public string noteEntryText;
#endif
        [HideInInspector]
        public string playableReference;
        [HideInInspector]
        public double end;
        [HideInInspector]
        public bool isWaitingForInput;
        [HideInInspector]
        public bool isDone;



        public string GetEndBehaviourSequence()
        {
            switch (endBehaviour)
            {
                case EndBehaviour.None:
                    return "required SetDialogueInput(true);" +
                       $"required Timeline(speed, {playableReference},1)";
                case EndBehaviour.EndConversation:
                    return "required SetDialogueInput(true);" +
                         $"required Timeline(speed, {playableReference},1);" +
                         "required StopConversation();" +
                         "required SetDialoguePanel(false)";
            }
            return "";
        }

        public string GetWaitForInputSequence()
        {
            return "required SetDialogueInput(true);" +
                        $"required Timeline(speed, {playableReference},0);" +
                        $"required Timeline(speed, {playableReference},1)@Message(ContinueDiag);" +
                        "required SetDialogueInput(false)@Message(ContinueDiag)";
        }
    }
}
#endif
#endif
