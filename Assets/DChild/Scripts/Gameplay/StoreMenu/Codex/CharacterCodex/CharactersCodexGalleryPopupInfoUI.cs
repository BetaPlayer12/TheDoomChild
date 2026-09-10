using DChild.Codex.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Codex.Characters
{
    public class CharactersCodexGalleryPopupInfoUI : CodexGalleryPopupInfoUI<CharacterCodexData>
    {
        [SerializeField]
        private TextMeshProUGUI m_alphabetName;
        [SerializeField]
        private TextMeshProUGUI m_baybayinName;
        [SerializeField]
        private Image m_creatureImage;
        [SerializeField]
        private TextMeshProUGUI m_description;

        [SerializeField]
        private TextMeshProUGUI m_doomknightRemarks;
        [SerializeField]
        private TextMeshProUGUI m_necroRemarks;

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
                m_description.text = "";
                return;
            }
            creatureNameText = m_showDataOf.characterName;
            SetImage(m_creatureImage, m_showDataOf.infoImage);

            if (m_showDataOf.specialCharacter)
            {
                m_description.text = "";
                if (m_showDataOf.doomedKnightComment)
                {
                    m_doomknightRemarks.gameObject.SetActive(true);
                    m_doomknightRemarks.text = m_showDataOf.doomedknightRemarks;
                }
                if (m_showDataOf.secondInteract)
                {
                    m_necroRemarks.gameObject.SetActive(true);
                    m_necroRemarks.text = m_showDataOf.necroRemarks;
                }
            }
            else
            {
                m_description.text = m_showDataOf.description;      
                m_doomknightRemarks.gameObject.SetActive(false);
                m_necroRemarks.gameObject.SetActive(false);
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
