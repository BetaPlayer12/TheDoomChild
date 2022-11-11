﻿// Recompile at 01/07/2021 2:56:36 PM


#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using DChild.UI;
using DChildDebug.Cutscene;
using DChild.Temp;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Doozy;
using Doozy.Runtime.Signals;
using UnityEngine.Events;

namespace PixelCrushers.DialogueSystem
{
    public class DialogueMixBehaviour : PlayableBehaviour
    {
        private HashSet<int> m_played = new HashSet<int>();
        private HashSet<DialogueBehaviour> m_behaviours = new HashSet<DialogueBehaviour>();
        private PlayableDirector m_director;

        private bool m_typeWriterEffectIsPlaying;
        private bool m_isCutsceneSkipped;

        private SignalReceiver m_continueReceiver;
        private SignalReceiver m_skipReceiver;

        private SignalStream m_continueStream;
        private SignalStream m_skipStream;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_isCutsceneSkipped)
                return;

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

            m_typeWriterEffectIsPlaying = DChildStandardUIContinueButtonFastForward.currentTypewriterEffect?.isPlaying ?? false;
        }

        private void OnContinueDialogue(Signal arg0)
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
                    ExecuteDialogueBehaviourEnd(input);
                }
            }
        }

        private void OnSkipDialogue(Signal arg0)
        {
            foreach (var input in m_behaviours)
            {
                ExecuteDialogueBehaviourEnd(input);
                break;
            }
        }

        private void ExecuteDialogueBehaviourEnd(DialogueBehaviour input)
        {
            m_director.time = input.end;
            input.isDone = true;
            m_behaviours.Remove(input);
            PlaySequence(input.GetEndBehaviourSequence());
            input.isWaitingForInput = false;
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
        private void OnCutsceneSkip()
        {
            m_isCutsceneSkipped = true;
            foreach (var input in m_behaviours)
            {
                PlaySequence(input.GetStopConversationSequence());
                input.isWaitingForInput = false;
            }
        }

        private void ListenToDialogueSignals()
        {
            ListenToDoozySignals(m_continueReceiver, m_continueStream, "Continue", OnContinueDialogue);
            ListenToDoozySignals(m_skipReceiver, m_skipStream, "Skip", OnSkipDialogue);
        }

        private void ListenToDoozySignals(SignalReceiver receiver, SignalStream stream, string streamName, UnityAction<Signal> signalCallback)
        {
            receiver = new SignalReceiver();
            receiver.SetOnSignalCallback(signalCallback);

            stream = SignalsService.FindStream("Dialogue", streamName);

            if(stream == null)
            {

            }

            stream.ConnectReceiver(receiver);
        }

        private void UnlistenToDialogueSignals()
        {
            m_continueStream.DisconnectReceiver(m_continueReceiver);
        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            m_director = (playable.GetGraph().GetResolver() as PlayableDirector);
            ListenToDialogueSignals();
            m_played.Clear();
            m_behaviours.Clear();
            DChildStandardDialogueUI.isInCutscene = true;
            SequenceSkipHandle.SkipExecute += OnCutsceneSkip;
            m_isCutsceneSkipped = false;
        }


        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            UnlistenToDialogueSignals();
            m_played.Clear();
            m_behaviours.Clear();
            DChildStandardDialogueUI.isInCutscene = false;
            SequenceSkipHandle.SkipExecute -= OnCutsceneSkip;
        }
    }
}
#endif
#endif