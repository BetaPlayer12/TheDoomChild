using DChild.Gameplay;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Systems;
using DChildDebug.Cutscene;
using Holysoft.Event;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ChatMapper;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.UI
{
    public class DChildStandardDialogueUI : StandardDialogueUI
    {
        private enum DialogueType
        {
            None,
            Banter,
            Dialogue
        }
        [Title("DChild Settings")]
        [SerializeField, ReadOnly]
        private DialogueType m_currentDialogueType;
        [SerializeField]
        private UIPanel m_dialoguePanel;
        [SerializeField]
        private StandardUISubtitlePanel m_dialoguePCSubtitlePanel;
        [SerializeField]
        private StandardUISubtitlePanel m_dialogueNPCSubtitlePanel;
        [SerializeField]
        private UIPanel m_banterPanel;
        [SerializeField]
        private StandardUISubtitlePanel m_banterSubtitlePanel;

        public static bool isInCutscene;
        public static bool dialogueActive;
        private bool m_skipUIShown;

        [SerializeField]
        private float m_skipDelayDuration;

        public static bool currentConverstionIsABanter { get; private set; }

        public override void Open()
        {
            var conversation = DialogueManager.MasterDatabase.GetConversation(DialogueManager.lastConversationStarted);

            if (conversation == null)
            {
                base.Open();
                return;
            }

            if (conversation.LookupBool("IsBanter"))
            {
                HandleOpenBanter();
                base.Open();
                return;
            }

            if (isInCutscene)
            {
                HandleOpenDialogue();
                base.Open();
                return;
            }

            SequenceSkipHandle.SkipExecute += OnSkipExecute;
            SequenceSkipHandle.SetDelayDuration(m_skipDelayDuration);
            GameplaySystem.gamplayUIHandle.ToggleSequenceSkip(true);
            m_skipUIShown = true;
            GameplaySystem.minionManager?.ForbidAllFromAttackingTarget(true);

            HandleOpenDialogue();

            base.Open();
        }

        private void HandleOpenDialogue()
        {
            DialogueTime.Mode = DialogueTime.TimeMode.Realtime;

            if (m_currentDialogueType != DialogueType.Dialogue)
            {
                DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Always;
                conversationUIElements.mainPanel = m_dialoguePanel;
                conversationUIElements.defaultPCSubtitlePanel = m_dialoguePCSubtitlePanel;
                conversationUIElements.defaultNPCSubtitlePanel = m_dialogueNPCSubtitlePanel;

                ResetConversationUIElements();

                m_currentDialogueType = DialogueType.Dialogue;
            }

            dialogueActive = true;
            //GameplaySystem.playerManager.DisableControls();

            currentConverstionIsABanter = false;
        }

        private void HandleOpenBanter()
        {
            DialogueTime.Mode = DialogueTime.TimeMode.Gameplay;

            if (m_currentDialogueType != DialogueType.Banter)
            {
                DialogueManager.displaySettings.subtitleSettings.continueButton = DisplaySettings.SubtitleSettings.ContinueButtonMode.Never;
                conversationUIElements.mainPanel = m_banterPanel;
                conversationUIElements.defaultPCSubtitlePanel = m_banterSubtitlePanel;
                conversationUIElements.defaultNPCSubtitlePanel = m_banterSubtitlePanel;

                ResetConversationUIElements();

                m_currentDialogueType = DialogueType.Banter;
            }

            currentConverstionIsABanter = true;
        }

        private void ResetConversationUIElements()
        {
            conversationUIElements.ClearAllSubtitleText();
            conversationUIElements.ClearCaches();
            conversationUIElements.Initialize();
        }

        private void OnSkipExecute()
        {
            GameplaySystem.gamplayUIHandle.ToggleSequenceSkip(false);
            DialogueManager.StopConversation();
            dialogueActive = false;
            SequenceSkipHandle.SkipExecute -= OnSkipExecute;
        }

        public override void Close()
        {
            if (isInCutscene == false)
            {
                // This should be adjusted more as sometimes this can give unneccessary effects to summoned minions
                GameplaySystem.minionManager?.ForbidAllFromAttackingTarget(true);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (BaseGameplayUIHandle.Instance == null)
                {
                    GameplaySystem.playerManager?.EnableControls();
                }
#else
                GameplaySystem.playerManager?.EnableControls();
#endif
                if (m_skipUIShown)
                {
                    GameplaySystem.gamplayUIHandle.ToggleSequenceSkip(false);
                    SequenceSkipHandle.SkipExecute -= OnSkipExecute;
                }

            }
            if (DialogueTime.Mode != DialogueTime.TimeMode.Realtime)
                DialogueTime.Mode = DialogueTime.TimeMode.Realtime;

            base.Close();
            dialogueActive = false;
        }
    }
}
