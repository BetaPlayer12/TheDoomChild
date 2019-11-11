using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeHandler : MonoBehaviour
{
    
    private ITradableInventory m_playerInventory;
    private ITradableInventory m_merchantInventory;

    public void BuyItem(ItemData Item)
    {
        if (Item.cost <= m_playerInventory.soulEssence)
        {
            m_playerInventory.AddSoulEssence(-Item.cost);
            m_playerInventory.items.AddItem(Item, 1);
            m_merchantInventory.items.AddItem(Item, -1);
        }
    }

    public void SellItem(ItemData Item)
    {
        if (m_playerInventory.items.GetCurrentAmount(Item) > 0)
        {
            m_playerInventory.AddSoulEssence(Item.cost);
            m_playerInventory.items.AddItem(Item, -1);
            m_merchantInventory.items.AddItem(Item, 1);           
        }
    }

    public void InputMerchantData(ITradableInventory MerchItemContainer)
    {
        m_merchantInventory = MerchItemContainer;
    }

    public void InputPlayerData(ITradableInventory Player)
    {
        m_playerInventory = Player;
    }

}

