using DChild.Gameplay;
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
        [Title("DChild Settings")]
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

        public override void Open()
        {
            var conversation = DialogueManager.MasterDatabase.GetConversation(DialogueManager.lastConversationStarted);
            if (conversation != null)
            {
                if (conversation.LookupBool("IsBanter"))
                {
                    conversationUIElements.mainPanel = m_banterPanel;
                    conversationUIElements.defaultPCSubtitlePanel = m_banterSubtitlePanel;
                    conversationUIElements.defaultNPCSubtitlePanel = m_banterSubtitlePanel;
                }
                else
                {
                    if (conversation.LookupBool("IsSkippable"))
                    {
                        SequenceSkipHandle.SkipExecute += OnSkipExecute;
                        GameplaySystem.gamplayUIHandle.ShowSequenceSkip();
                    }
                    conversationUIElements.mainPanel = m_dialoguePanel;
                    conversationUIElements.defaultPCSubtitlePanel = m_dialoguePCSubtitlePanel;
                    conversationUIElements.defaultNPCSubtitlePanel = m_dialogueNPCSubtitlePanel;

                    GameplaySystem.playerManager.DisableControls();
                }

            }

            base.Open();

        }

        private void OnSkipExecute()
        {
            DialogueManager.StopConversation();
            SequenceSkipHandle.SkipExecute -= OnSkipExecute;
        }

        public override void Close()
        {
            if (isInCutscene == false)
            {
                GameplaySystem.playerManager.EnableControls();
            }
            SequenceSkipHandle.SkipExecute -= OnSkipExecute;
            base.Close();
        }
    }
}