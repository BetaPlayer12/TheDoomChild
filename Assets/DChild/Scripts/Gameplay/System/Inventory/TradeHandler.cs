
using DChild.Gameplay.Items;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class TradeHandler : MonoBehaviour
    {
        private ITradableInventory m_buyer;
        private ITradableInventory m_seller;

        public void BuyItem(ItemData item)
        {
            var cost = item.cost;
            if (m_buyer.CanAfford(cost))
            {
                m_buyer.AddSoulEssence(-cost);
                m_buyer.AddItem(item, 1);
                m_seller.AddSoulEssence(cost);
                m_seller.AddItem(item, -1);
            }
        }

        public void SellItem(ItemData item)
        {
            var cost = item.cost;
            if (m_seller.CanAfford(cost))
            {
                m_buyer.AddSoulEssence(cost);
                m_buyer.AddItem(item, -1);
                m_seller.AddSoulEssence(-cost);
                m_seller.AddItem(item, 1);
            }
        }

        public void SetSeller(ITradableInventory seller)
        {
            m_seller = seller;
        }

        public void SetBuyer(ITradableInventory buyer)
        {
            m_buyer = buyer;
        }

    }
}

