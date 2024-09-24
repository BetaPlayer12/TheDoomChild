using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Codex.Tutorial
{
    public class TutorialCodexInfoUI : CodexInfoUI<TutorialCodexData>
    {
        [SerializeField]
        private TextMeshProUGUI m_alphabetName;
        [SerializeField]
        private TextMeshProUGUI m_baybayinName;
        [SerializeField]
        private Image m_creatureImage;
        [SerializeField]
        private Image m_sketchImage;
        [SerializeField]
        private TextMeshProUGUI m_description;



        private string creatureNameText
        {
            set
            {
                m_alphabetName.text = value;
                m_baybayinName.text = value;
            }
        }

        protected override void UpdateInfo()
        {
            if (m_showDataOf == null)
            {
                creatureNameText = "";
                SetImage(m_creatureImage, null);
                SetImage(m_sketchImage, null);
                m_description.text = "";
            }
            else
            {
                creatureNameText = m_showDataOf.name;
                SetImage(m_creatureImage, m_showDataOf.indexImage);
                SetImage(m_sketchImage, m_showDataOf.indexImage);
                m_description.text = m_showDataOf.description;

            }
        }



        private void SetImage(Image image, Sprite sprite)
        {
            if (sprite == null)
            {
                image.color = Color.clear;
                image.sprite = sprite;
            }
            else
            {
                image.color = Color.white;
                image.sprite = sprite;
            }
        }
    }

}
