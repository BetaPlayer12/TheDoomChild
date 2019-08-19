using DChild.Gameplay.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
    [RequireComponent(typeof(Canvas))]
    public class ItemSlotInfo : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_countText;

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

        public void SetInfo(ItemSlot slot)
        {
            m_icon.sprite = slot.item.icon;
            m_countText.text = slot.count.ToString();
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}
