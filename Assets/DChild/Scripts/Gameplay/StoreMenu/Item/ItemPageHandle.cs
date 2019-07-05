using UnityEngine;

namespace DChild.Menu.Item
{
    public class ItemPageHandle : MonoBehaviour
    {
        [SerializeField]
        private ItemInfoPage m_info;

        public void Select(ItemSlot slot)
        {
            m_info.SetInfo(slot.data);
        }
    }
}
