// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using Doozy.Engine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PixelCrushers.DialogueSystem
{
    public class DialogueMixBehaviour : PlayableBehaviour
    {
        private HashSet<int> played = new HashSet<int>();
        private HashSet<DialogueBehaviour> behaviours = new HashSet<DialogueBehaviour>();
        private PlayableDirector director;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0.001f && !played.Contains(i))
                {
                    played.Add(i);
                    ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(i);
                    DialogueBehaviour input = inputPlayable.GetBehaviour();

                    switch (input.startBehaviour)
                    {
                        case DialogueBehaviour.StartBehaviour.StartDialogue:
                            StartDialogue(input);
                            break;
                    }
                }
                else if (inputWeight <= 0.001f && played.Contains(i))
                {
                    played.Remove(i);

                    ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(i);
                    DialogueBehaviour input = inputPlayable.GetBehaviour();
                    PlaySequence(input.GetEndBehaviourSequence());
                }
            }

            var currentTime = director.time;
            foreach (var index in played)
            {
                ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(index);
                DialogueBehaviour input = inputPlayable.GetBehaviour();

                if (input.m_isWaitingForInput == false)
                {
                    var waitForInputTime = input.end - input.waitForInput;
                    if (currentTime <= input.end && currentTime >= waitForInputTime)
                    {
                        PlaySequence(GetWaitForInputSequence());
                        input.m_isWaitingForInput = true;
                        break;
                    }
                }
            }
        }

        private void OnContinueDiag()
        {
            foreach (var input in behaviours)
            {
                if (input.m_isWaitingForInput == false)
                {
                    director.time = input.end- input.waitForInput;
                    PlaySequence(GetWaitForInputSequence());
                    input.m_isWaitingForInput = true;
                    break;
                }
                else
                {
                    director.time = input.end;
                    input.m_isWaitingForInput = false;
                    break;
                }
            }
        }

        private string GetWaitForInputSequence()
        {
            return "";
        }

        private void PlaySequence(string sequence)
        {
            if (Application.isPlaying)
            {
                DialogueManager.PlaySequence(sequence, null, null);
            }
            else
            {
                PreviewUI.ShowMessage(sequence, 3, -1);
            }
        }

        private void StartDialogue(DialogueBehaviour input)
        {
            if (Application.isPlaying)
            {
                if (input.jumpToSpecificEntry && input.entryID > 0)
                {
                    DialogueManager.StartConversation(input.conversation, null, null, input.entryID);
                }
                else
                {
                    DialogueManager.StartConversation(input.conversation, null, null);
                }
            }
            else
            {
                var message = "Conversation (" + DialogueActor.GetActorName(null) + "->" + DialogueActor.GetActorName(null) + "): " + input.conversation;
                PreviewUI.ShowMessage(message, 2, 0);
            }
        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
            GameEventMessage.AddListener("ContinueDiag", OnContinueDiag);
            played.Clear();
        }


        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            GameEventMessage.RemoveListener("ContinueDiag", OnContinueDiag);
            played.Clear();
        }
    }
}
#endif
#endif
