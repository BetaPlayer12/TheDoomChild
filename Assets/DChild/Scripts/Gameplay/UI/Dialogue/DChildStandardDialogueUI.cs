using DChild.Gameplay;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
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
                    conversationUIElements.mainPanel = m_dialoguePanel;
                    conversationUIElements.defaultPCSubtitlePanel = m_dialoguePCSubtitlePanel;
                    conversationUIElements.defaultNPCSubtitlePanel = m_dialogueNPCSubtitlePanel;

                    GameplaySystem.playerManager.DisableControls();
                }
            }

            base.Open();

        }

        public override void Close()
        {
            GameplaySystem.playerManager.EnableControls();
            base.Close();
        }
    }

}