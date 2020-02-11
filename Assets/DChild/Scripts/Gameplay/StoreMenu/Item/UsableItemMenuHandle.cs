using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Item
{
    public class UsableItemMenuHandle : MonoBehaviour
    {
        [SerializeField]
        private Player m_player;
        private PlayerInventory m_inventory;
        private UsableItemData m_item;

        public void SetItem(UsableItemData item)
        {
            m_item = item;
        }

        public void UseItemOnPlayer()
        {
            if (m_inventory.GetCurrentAmount(m_item) > 0)
            {
                m_item.Use(m_player);
                m_inventory.RemoveItem(m_item);
            }
        }

        private void Awake()
        {
            m_inventory = m_player.inventory;
        }
    }
}
