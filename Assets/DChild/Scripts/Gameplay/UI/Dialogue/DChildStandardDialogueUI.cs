using DChild.Gameplay;
using DChild.Gameplay.Characters.AI;
using DChildDebug.Cutscene;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
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

        public static bool currentConverstionIsABanter { get; private set; }

        public override void Open()
        {
            var conversation = DialogueManager.MasterDatabase.GetConversation(DialogueManager.lastConversationStarted);
            if (conversation != null)
            {
                if (conversation.LookupBool("IsBanter"))
                {
                    HandleOpenBanter();
                }
                else
                {
                    if (conversation.LookupBool("IsSkippable"))
                    {
                        SequenceSkipHandle.SkipExecute += OnSkipExecute;
                        GameplaySystem.gamplayUIHandle.ShowSequenceSkip(true);
                        m_skipUIShown = true;
                    }

                    HandleOpenDialogue();

                    if (!isInCutscene)
                    {
                        GameplaySystem.minionManager?.ForbidAllFromAttackingTarget(true);
                    }
                }
            }

            base.Open();


        }

        private void HandleOpenDialogue()
        {
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
            GameplaySystem.playerManager.DisableControls();

            currentConverstionIsABanter = false;
        }

        private void HandleOpenBanter()
        {
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
            GameplaySystem.gamplayUIHandle.ShowSequenceSkip(false);
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

                GameplaySystem.playerManager?.EnableControls();
                if (m_skipUIShown)
                {
                    GameplaySystem.gamplayUIHandle.ShowSequenceSkip(false);
                    SequenceSkipHandle.SkipExecute -= OnSkipExecute;
                }

            }
            base.Close();
            dialogueActive = false;


        }
    }
}