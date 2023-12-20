using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemConversionHandler : SerializedMonoBehaviour
{
    [SerializeField]
    private ItemData m_item;
    [SerializeField]
    private ItemData m_completeItem;
    [SerializeField]
    private int m_limit;
    [SerializeField]
    private IShardCompletionHandle m_completionHandle;

    protected static Player m_player;
    private static UsableItemData m_pickUpItem;


    private void ConvertItemToCompletedVersion()
    {
        m_player.inventory.InventoryItemUpdate -= onPickup;
        m_player.inventory.RemoveItem(m_item, m_limit);
        ItemCompletionReward();
        m_player.inventory.InventoryItemUpdate += onPickup;
    }
    private void ItemCompletionReward()
    {
        if (m_completeItem != null)
        {
            m_player.inventory.AddItem(m_completeItem, 1);
        }

        m_completionHandle.Execute(m_player);
    }

    protected virtual void OnEnable()
    {
        var currentPlayer = GameplaySystem.playerManager.player;
        if (m_player == null || m_player == currentPlayer)
        {
            m_player = GameplaySystem.playerManager.player;
            m_player.inventory.InventoryItemUpdate += onPickup;
        }
    }

    private void onPickup(object sender, ItemEventArgs eventArgs)
    {
        if (eventArgs.data.itemName == m_item.itemName)
        {
            if (eventArgs.currentCount >= m_limit)
            {
                ConvertItemToCompletedVersion();
            }
        }
    }
}
