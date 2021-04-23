using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Item
{
    public class UsableItemMenuHandle : MonoBehaviour
    {
        [SerializeField]
        private ItemInfoPage m_infoPage;
        [SerializeField]
        private SingleFocusHandler m_highlighter;

        private Player m_player;
        private PlayerInventory m_inventory;
        private UsableItemData m_item;

        public event EventAction<EventActionArgs> AllItemCountConsumed;


        public void SetItem(UsableItemData item)
        {
            m_item = item;
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
            m_inventory = m_player.inventory;
        }
    }
}
