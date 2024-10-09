using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class ItemNotifier : MonoBehaviour
    {
        [SerializeField]
        private List<ItemData> m_itemToMonitor;
        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            /*temp fix*/
            if (m_itemToMonitor.Contains(eventArgs.data) && eventArgs.countModification > 0)
            {
                GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(eventArgs.data);
            }
        }

        private void OnEnable()
        {
            var inventory = GameplaySystem.playerManager.player.inventory;
            inventory.InventoryItemUpdate += OnItemUpdate;
        }

        private void OnDisable()
        {
            var inventory = GameplaySystem.playerManager.player.inventory;
            inventory.InventoryItemUpdate -= OnItemUpdate;
        }
    }

}