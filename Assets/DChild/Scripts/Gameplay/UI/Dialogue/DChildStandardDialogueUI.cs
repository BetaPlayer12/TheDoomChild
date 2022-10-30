﻿using DChild.Gameplay;
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
        public static bool dialogueActive;
        private bool m_skipUIShown;

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
                        GameplaySystem.gamplayUIHandle.ShowSequenceSkip(true);
                        m_skipUIShown = true;
                    }
                    conversationUIElements.mainPanel = m_dialoguePanel;
                    conversationUIElements.defaultPCSubtitlePanel = m_dialoguePCSubtitlePanel;
                    conversationUIElements.defaultNPCSubtitlePanel = m_dialogueNPCSubtitlePanel;

                    dialogueActive = true;
                    GameplaySystem.playerManager.DisableControls();

                    if (!isInCutscene)
                    {
                        CombatAIManager.instance?.ForbidAllFromAttackTarget(true);
                    }
                }

            }

            base.Open();

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
                CombatAIManager.instance?.ForbidAllFromAttackTarget(false);
                GameplaySystem.playerManager.EnableControls();
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