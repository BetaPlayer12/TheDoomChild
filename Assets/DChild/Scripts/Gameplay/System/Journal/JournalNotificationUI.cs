using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Systems.Journal
{
    public class JournalNotificationUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_notifinfo;

        public void UpdateUI(JournalData journaldata)
        {
            m_notifinfo.sprite = journaldata.notification;
        }
    }
}
