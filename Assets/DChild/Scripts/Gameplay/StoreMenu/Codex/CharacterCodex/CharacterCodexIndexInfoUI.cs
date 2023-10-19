using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Codex.Characters
{
    public class CharacterCodexIndexInfoUI : CodexIndexInfoUI<CharacterCodexData>
    {
        [SerializeField]
        private Image m_background;
        [SerializeField]
        private Sprite m_unknownBackground;
        [SerializeField]
        private Sprite m_normalBackground;
        [SerializeField]
        private Sprite m_newChracterBackground;


        [SerializeField]
        private TextMeshProUGUI m_characterLabel;
        [SerializeField]
        private Image m_chracterImage;
        [SerializeField]
        private Image m_newStamp;

        public override void SetAsNewInfo(bool isNew)
        {
            m_newStamp.enabled = isNew;
            if (isNew)
            {
                m_background.sprite = m_newChracterBackground;
            }
            //else
            //{
            //    m_background.sprite = m_normalBackground;
            //}
        }

        public override void SetInfo(CharacterCodexData data)
        {
            if (data == null)
            {
                m_characterLabel.text = "Nothing";
                m_chracterImage.sprite = null;
            }
            else
            {
                m_characterLabel.text = data.creatureName;
                m_chracterImage.sprite = data.indexImage;
            }
        }
    }
}

