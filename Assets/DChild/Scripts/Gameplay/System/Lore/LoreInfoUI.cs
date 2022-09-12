using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Systems.Lore
{
    public class LoreInfoUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_topicImage;
        [SerializeField]
        private TextMeshProUGUI m_alphabethLabel;
        [SerializeField]
        private TextMeshProUGUI m_baybayinLabel;
        [SerializeField]
        private TextMeshProUGUI m_messageLabel;
        [SerializeField]
        private TextMeshProUGUI m_authorLabel;

        public void SetInfo(LoreData loreData)
        {
            m_topicImage.sprite = loreData.topic;
            m_alphabethLabel.text = loreData.alphabethName;
            m_baybayinLabel.text = loreData.baybayinName;
            m_messageLabel.text = loreData.message;
            m_authorLabel.text = loreData.author;
        }
    }
}