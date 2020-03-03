using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_creatureLabel;
        [SerializeField]
        private Image m_creatureImage;

        public void Show()
        {
            m_creatureLabel.enabled = true;
            m_creatureImage.enabled = true;
        }

        public void Hide()
        {
            m_creatureLabel.enabled = false;
            m_creatureImage.enabled = false;
        }

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
    }
}