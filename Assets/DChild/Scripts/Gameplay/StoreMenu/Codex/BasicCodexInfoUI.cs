using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Codex
{
    public class BasicCodexInfoUI : CodexIndexInfoUI<ICodexIndexInfo>
    {
        [SerializeField]
        private Image m_background;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_unknownBackground;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_normalBackground;
        [SerializeField, FoldoutGroup("Backgrounds")]
        private Sprite m_newCreaureBackground;


        [SerializeField, FoldoutGroup("IndexInfoUI")]
        private TextMeshProUGUI m_infoLabel;
        [SerializeField, FoldoutGroup("IndexInfoUI")]
        private Image m_infoImage;
        [SerializeField, FoldoutGroup("IndexInfoUI")]
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

        public override void SetInfo(ICodexIndexInfo data)
        {
            if (data == null)
            {
                m_infoLabel.text = "Nothing";
                m_infoImage.sprite = null;
            }
            else
            {
                m_infoLabel.text = data.indexName;
                m_infoImage.sprite = data.indexImage;
            }
        }

#if UNITY_EDITOR
        [ResponsiveButtonGroup, Button]
        private void MarkAsNew()
        {
            SetAsNewInfo(true);
        }
#endif
    }
}