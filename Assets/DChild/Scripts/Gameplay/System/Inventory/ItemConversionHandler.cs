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

    private int m_currentHeld = 0;
    protected static Player m_player;
    private static UsableItemData m_pickUpItem;

    public void VerifyItem(ItemData m_pickup)
    {
        if (m_pickup.itemName == m_item.itemName)
        {
            m_currentHeld = m_currentHeld + 1;
            VerifyItemLimit();
        }
    }
    public void VerifyItemLimit()
    {
        if (m_currentHeld >= m_limit)
        {
            ItemCompletionReward();
            m_player.inventory.AddItem(m_item, -m_limit);

        }
    }
    public void ItemCompletionReward()
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
        VerifyItem(eventArgs.data);
    }
}
