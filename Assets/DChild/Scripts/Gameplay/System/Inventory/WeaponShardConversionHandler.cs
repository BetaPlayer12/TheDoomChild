using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShardConversionHandler : SerializedMonoBehaviour
{
    [SerializeField]
    private ItemData m_item;
    [SerializeField]
    private IShardCompletionHandle m_completionHandle;

    protected static Player m_player;
    private static UsableItemData m_pickUpItem;

    private void ItemConversionReward()
    {
        m_player.inventory.RemoveItem(m_item);
        m_completionHandle.Execute(m_player);
    }

    public void CommenceUpgrade()
    {
        Debug.Log("It do da upgrade");
        ItemConversionReward();
    }
   
}
