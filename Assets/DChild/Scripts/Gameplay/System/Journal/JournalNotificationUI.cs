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
        private Image m_journalItemSprite;
        [SerializeField]
        private TextMeshProUGUI m_itemName;
        [SerializeField]
        private TextMeshProUGUI m_itemDescription;

        public void UpdateUI(JournalData journaldata)
        {
            m_journalItemSprite.sprite = journaldata.ItemImage;
            m_itemName.text = journaldata.ItemName;
            m_itemDescription.text = journaldata.ItemDescription;

        }
    }
}
