using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    [RequireComponent(typeof(Canvas))]
    public class BestiaryIndexInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_creatureLabel;
        [SerializeField]
        private Image m_creatureImage;

        private Canvas m_canvas;

        public void Show()
        {
#if UNITY_EDITOR
            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
            }
#endif
            m_canvas.enabled = true;
        }

        public void Hide()
        {
#if UNITY_EDITOR
            if (m_canvas == null)
            {
                m_canvas = GetComponent<Canvas>();
            }
#endif
            m_canvas.enabled = false;
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

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}