using DChild.Menu.Bestiary;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Codex.Bestiary
{

    public class BestiaryCodexIndexInfoUI : CodexIndexInfoUI<BestiaryData>
    {
        [SerializeField]
        private Image m_background;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_unknownBackground;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_normalBackground;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_newCreaureBackground;


        [SerializeField, FoldoutGroup("CreatureInfoUI")]
        private TextMeshProUGUI m_creatureLabel;
        [SerializeField, FoldoutGroup("CreatureInfoUI")]
        private Image m_creatureImage;
        [SerializeField, FoldoutGroup("CreatureInfoUI")]
        private Image m_newStamp;

        public override void SetAsNewInfo(bool isNew)
        {
            m_newStamp.enabled = isNew;
            if (isNew)
            {
                m_background.sprite = m_newCreaureBackground;
            }
            //else
            //{
            //    m_background.sprite = m_normalBackground;
            //}
        }

        public override void SetInfo(BestiaryData data)
        {
            if (data == null)
            {
                m_creatureLabel.text = "Nothing";
                m_creatureImage.sprite = null;
            }
            else
            {
                m_creatureLabel.text = data.creatureName;
                m_creatureImage.sprite = data.indexImage;
            }
        }
    }

}