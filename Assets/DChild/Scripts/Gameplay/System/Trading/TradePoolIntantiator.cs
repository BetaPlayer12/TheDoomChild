using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class TradePoolIntantiator : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_template;
        [SerializeField]
        private RectTransform m_pool;
        [SerializeField]
        private GridLayoutGroup m_autoLayout;

        private List<TradableItemUI> m_instantiatedSlots;

        public event EventAction<EventActionArgs> OnPoolUpdate;

        public List<TradableItemUI> instantiatedSlots => m_instantiatedSlots;
        public int slotCount => m_instantiatedSlots.Count;

        public void Instantiate(ITradableInventory inventory, TradePoolFilter filter)
        {
            //Generate Slots
            bool createSlots = m_instantiatedSlots.Count == 0;
            int slotCreated = 0;
            for (int i = 0; i < inventory.Count; i++)
            {
                var itemSlot = inventory.GetSlot(i);
                if (itemSlot.restrictions.canBeSold && IsPartOfFilter(itemSlot.item, filter))
                {
                    if (createSlots)
                    {
                        var slot = this.InstantiateToScene(m_template, m_pool).GetComponent<TradableItemUI>();
                        slot.Set(itemSlot);
                        m_instantiatedSlots.Add(slot);
                        slotCreated++;
                    }
                    else
                    {
                        m_instantiatedSlots[slotCreated].Set(itemSlot);
                        slotCreated++;
                        if (slotCreated == m_instantiatedSlots.Count)
                        {
                            createSlots = true;
                        }
                    }
                }
            }

            //Cleanup Excess Slots
            if (m_instantiatedSlots.Count > inventory.Count)
            {
                for (int i = m_instantiatedSlots.Count - 1; i >= inventory.Count; i--)
                {
                    Destroy(m_instantiatedSlots[i].gameObject);
                    m_instantiatedSlots.RemoveAt(i);
                }
            }

            if (m_instantiatedSlots.Count != slotCreated)
            {
                for (int i = m_instantiatedSlots.Count - 1; i >= slotCreated; i--)
                {
                    Destroy(m_instantiatedSlots[i].gameObject);
                    m_instantiatedSlots.RemoveAt(i);
                }
            }

            OnPoolUpdate?.Invoke(this, EventActionArgs.Empty);

            bool IsPartOfFilter(ItemData item, TradePoolFilter tradeFilter)
            {
                if (tradeFilter == TradePoolFilter.All)
                {
                    return true;
                }
                switch (item.category)
                {
                    case ItemCategory.Throwable:
                        return tradeFilter == TradePoolFilter.Weapons;
                    case ItemCategory.Consumable:
                        return tradeFilter == TradePoolFilter.Consumables;
                    case ItemCategory.Quest:
                        return tradeFilter == TradePoolFilter.Keys;
                    case ItemCategory.Key:
                        return tradeFilter == TradePoolFilter.Keys;
                    default:
                        return true;
                }
            }
        }

        public TradableItemUI GetTradableUI(int index) => m_instantiatedSlots[index];

        private IEnumerator AutoLayoutRoutine()
        {
            m_autoLayout.enabled = true;
            yield return null;
            m_autoLayout.enabled = false;
        }
        private void Awake()
        {
            m_instantiatedSlots = new List<TradableItemUI>();
        }
    }

}