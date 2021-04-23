using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemSlot : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;

        public void UpdateSlot(ItemSlot slot)
        {
            if (slot == null)
            {
                m_icon.sprite = null;
            }
            else
            {
                m_icon.sprite = slot.item.icon;
            }
        }
    }
}
