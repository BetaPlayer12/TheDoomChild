// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace PixelCrushers.DialogueSystem
{
    [Serializable]
    public class PlaySequenceShortcutBehaviour : PlayableBehaviour
    {
        public enum SequenceShortcut
        {
            WaitForInput,
            StartDialogue,
            EndDiagAfterInput
        }

        public SequenceShortcut m_sequence;
        [HideInInspector]
        public string m_reference;

        private bool m_isExecuted;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_isExecuted)
                return;

            string sequence = GetSequence();

            if (Application.isPlaying)
            {
                DialogueManager.PlaySequence(sequence, null, null);
            }
            else
            {
                PreviewUI.ShowMessage(sequence, 3, -1);
            }

            m_isExecuted = true;
        }



        private string GetSequence()
        {
            switch (m_sequence)
            {
                case SequenceShortcut.WaitForInput:
                    return "required SetDialogueInput(true);" +
                        $"required Timeline(speed, {m_reference},0);" +
                        $"required Timeline(speed, {m_reference},1)@Message(ContinueDiag);" +
                        "required SetDialogueInput(false)@Message(ContinueDiag)";
                case SequenceShortcut.StartDialogue:
                    return "required SetDialoguePanel(true);" +
                           "required SetDialogueInput(false)";
                case SequenceShortcut.EndDiagAfterInput:
                    return "required SetDialogueInput(true);" +
                        $"required Timeline(speed, {m_reference},0);" +
                        $"required Timeline(speed, {m_reference},1)@Message(ContinueDiag);" +
                        "required StopConversation()@Message(ContinueDiag);";
            }
            return "";
        }
    }
}
#endif
#endif
