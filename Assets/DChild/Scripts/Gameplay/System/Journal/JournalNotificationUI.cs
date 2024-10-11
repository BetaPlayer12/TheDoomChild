using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Systems.Journal
{
    public class JournalNotificationUI : NotificationUI
    {
        [SerializeField]
        private Image m_notifinfo;

        [SerializeField]
        private TextMeshProUGUI m_itemName;

        [SerializeField]
        private TextMeshProUGUI m_itemDescription;


        public void UpdateUI(JournalData journaldata)
        {
            m_notifinfo.sprite = journaldata.notification;
            m_itemName.text = journaldata.itemName;
            m_itemDescription.text = journaldata.itemDescription;
            m_notifinfo.material = journaldata.material;
        }
    }
}
