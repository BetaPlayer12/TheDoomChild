using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexInfoUI : MonoBehaviour
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

        public void SetInfo(BestiaryData data)
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


        public void SetAsNewInfo(bool isNew)
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

#if UNITY_EDITOR
        [ResponsiveButtonGroup, Button]
        private void MarkAsNew()
        {
            SetAsNewInfo(true);
        }
#endif
    }
}