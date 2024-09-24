using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Journal
{
    public class JournalNotification : MonoBehaviour
    {
        [SerializeField]
        private GameplayInput m_gameplayinput;
        [SerializeField]
        private JournalNotificationUI m_ui;
        [SerializeField, MinValue(0.1f)]
        private float m_overrideInputDuration = 1f;

        public void UpdateNotification(JournalData journaldata)
        {
            m_ui.UpdateUI(journaldata);
            //m_gameplayinput.OverrideNewInfoNotif(m_overrideInputDuration);
            //GameplaySystem.gamplayUIHandle.notificationManager.ShowJournalUpdateNotification();
        }
    }
}
