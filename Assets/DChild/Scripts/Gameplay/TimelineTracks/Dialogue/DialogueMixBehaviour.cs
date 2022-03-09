// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using DChild.UI;
using Doozy.Engine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PixelCrushers.DialogueSystem
{
    public class DialogueMixBehaviour : PlayableBehaviour
    {
        private HashSet<int> m_played = new HashSet<int>();
        private HashSet<DialogueBehaviour> m_behaviours = new HashSet<DialogueBehaviour>();
        private PlayableDirector m_director;

        private bool m_typeWriterEffectIsPlaying;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0.001f && !m_played.Contains(i))
                {
                    m_played.Add(i);
                    ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(i);
                    DialogueBehaviour input = inputPlayable.GetBehaviour();
                    m_behaviours.Add(input);
                    input.isDone = false;
                    switch (input.startBehaviour)
                    {
                        case DialogueBehaviour.StartBehaviour.StartDialogue:
                            StartDialogue(input);
                            break;
                    }
                }
                else if (inputWeight <= 0.001f && m_played.Contains(i))
                {
                    m_played.Remove(i);

                    ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(i);
                    DialogueBehaviour input = inputPlayable.GetBehaviour();
                    input.isDone = true;
                    if (m_behaviours.Contains(input))
                    {
                        m_behaviours.Remove(input);
                        PlaySequence(input.GetEndBehaviourSequence());
                    }
                }
            }

            var currentTime = m_director.time;
            foreach (var index in m_played)
            {
                ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(index);
                DialogueBehaviour input = inputPlayable.GetBehaviour();

                if (input.isWaitingForInput == false && input.isDone == false)
                {
                    var waitForInputTime = input.end - input.waitForInput;
                    if (currentTime <= input.end && currentTime >= waitForInputTime)
                    {
                        PlaySequence(input.GetWaitForInputSequence());
                        input.isWaitingForInput = true;
                        break;
                    }
                }
            }

            m_typeWriterEffectIsPlaying = DChildStandardUIContinueButtonFastForward.currentTypewriterEffect.isPlaying;
        }

        private void OnContinueDiag(GameEventMessage obj)
        {
            if (obj.HasGameEvent && obj.EventName == "ContinueDiag")
            {
                foreach (var input in m_behaviours)
                {
                    if (m_typeWriterEffectIsPlaying && input.isWaitingForInput == false)
                    {
                        m_director.time = input.end - input.waitForInput;
                        PlaySequence(input.GetWaitForInputSequence());
                        input.isWaitingForInput = true;
                        break;
                    }
                    else
                    {
                        m_director.time = input.end;
                        input.isDone = true;
                        m_behaviours.Remove(input);
                        PlaySequence(input.GetEndBehaviourSequence());
                        input.isWaitingForInput = false;
                        break;
                    }
                }
            }
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
            m_director = (playable.GetGraph().GetResolver() as PlayableDirector);
            Message.AddListener<GameEventMessage>(OnContinueDiag);
            m_played.Clear();
            m_behaviours.Clear();
        }



        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            Message.RemoveListener<GameEventMessage>(OnContinueDiag);
            m_played.Clear();
            m_behaviours.Clear();
        }
    }
}
#endif
#endif