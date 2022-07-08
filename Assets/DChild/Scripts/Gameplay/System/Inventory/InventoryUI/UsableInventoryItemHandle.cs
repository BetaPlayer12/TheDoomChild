using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Items;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class UsableInventoryItemHandle : MonoBehaviour
    {
        [SerializeField]
        private Button m_useItemButton;

        private Player m_player;
        private PlayerInventory m_inventory;
        private UsableItemData m_item;

        public event EventAction<EventActionArgs> AllItemCountConsumed;

        public void Show()
        {
            m_useItemButton.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_useItemButton.gameObject.SetActive(false);
        }

        public void HandleUsageOfItem(ItemData itemData)
        {
            m_item = (UsableItemData)itemData;
        }

        public void UseItemOnPlayer()
        {
            if (m_item.CanBeUse(m_player))
            {
                m_item.Use(m_player);
                m_inventory.RemoveItem(m_item);
                if (m_inventory.GetCurrentAmount(m_item) == 0)
                {
                    AllItemCountConsumed?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        private void Awake()
        {
            m_player = GameplaySystem.playerManager.player;
            //m_inventory = m_player.inventory;
        }
    }
}