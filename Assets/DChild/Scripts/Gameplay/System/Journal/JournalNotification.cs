using DChild.Gameplay.Systems;
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

        public void UpdateNotification(JournalData journaldata)
        {

        }
    }
}
