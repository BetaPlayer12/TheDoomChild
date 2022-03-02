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

        public StartBehaviour startBehaviour;

        [Tooltip("The conversation to start.")]
        [ConversationPopup, ShowIf("@startBehaviour == StartBehaviour.StartDialogue"), Indent]
        public string conversation;

        [Tooltip("Jump to a specific dialogue entry instead of starting from the conversation's START node."), ShowIf("@startBehaviour == StartBehaviour.StartDialogue"), Indent]
        public bool jumpToSpecificEntry;

        [Tooltip("Dialogue entry to jump to."), ShowIf("@startBehaviour == StartBehaviour.StartDialogue"), Indent]
        public int entryID;

        public EndBehaviour endBehaviour;
        public double waitForInput = 0.5f;

        [HideInInspector]
        public string entryText;
        [HideInInspector]
        public double end;
        [HideInInspector]
        public bool m_isWaitingForInput;

        public string GetEndBehaviourSequence()
        {
            return "";
        }
    }
}
#endif
#endif
