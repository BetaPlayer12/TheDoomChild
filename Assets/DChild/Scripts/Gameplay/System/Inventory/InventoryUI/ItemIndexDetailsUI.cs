using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class ItemIndexDetailsUI : ItemDetailsUI
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_countText;

        private Canvas m_canvas;

        public override void ShowDetails(IStoredItem reference)
        {
            m_icon.sprite = reference.data.icon;
            m_countText.text = reference.count.ToString();
        }

        public override void Show()
        {
            m_canvas.enabled = true;
        }

        public override void Hide()
        {
            m_canvas.enabled = false;
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}