using DChild.Gameplay.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
    [RequireComponent(typeof(Canvas))]
    public class ItemSlotInfo : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;

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

        public void SetInfo(ItemData data)
        {
            m_icon.sprite = data.icon;
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}
