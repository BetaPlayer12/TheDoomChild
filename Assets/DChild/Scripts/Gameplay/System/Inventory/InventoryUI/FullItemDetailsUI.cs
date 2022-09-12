using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class FullItemDetailsUI : ItemDetailsUI
    {
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField,BoxGroup("Optional")]
        private TextMeshProUGUI m_quantityLimit;

        private Canvas m_canvas;

        public override void Hide()
        {
            m_canvas.enabled = false;
        }

        public override void Show()
        {
            m_canvas.enabled = true;
        }

        public override void ShowDetails(IStoredItem reference)
        {
            var data = reference.data;
            if (data == null)
            {
                m_name.text = "Nothing";
                m_icon.sprite = null;
                m_description.text = "You have nothing, this is not a lack of something but the absence of everything.\n " +
                                    "Do not worry having nothing is fine but if you still see this when you should have something is troubling" +
                                    "Please make sure you have nothing first before saying nothing is fine";
                if (m_quantityLimit != null)
                {
                    m_quantityLimit.text = "0";
                }
            }
            else
            {
                m_name.text = data.itemName;
                m_icon.sprite = data.icon;
                m_description.text = data.description;
                if (m_quantityLimit != null)
                {
                    m_quantityLimit.text = data.quantityLimit.ToString();
                }
            }
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}