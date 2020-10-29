using DChild.Gameplay.Inventories;
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

        public void Instantiate(ITradableInventory inventory)
        {
            bool createSlots = m_instantiatedSlots.Count == 0;

            for (int i = 0; i < inventory.Count; i++)
            {
                if (createSlots)
                {
                    var slot = this.InstantiateToScene(m_template, m_pool).GetComponent<TradableItemUI>();
                    slot.Set(inventory.GetSlot(i));
                    m_instantiatedSlots.Add(slot);
                }
                else
                {
                    m_instantiatedSlots[i].Set(inventory.GetSlot(i));
                    if (i >= m_instantiatedSlots.Count - 1)
                    {
                        createSlots = true;
                    }
                }
            }

            if (m_instantiatedSlots.Count > inventory.Count)
            {
                for (int i = m_instantiatedSlots.Count - 1; i >= inventory.Count; i--)
                {
                    Destroy(m_instantiatedSlots[i].gameObject);
                    m_instantiatedSlots.RemoveAt(i);
                }
            }
            OnPoolUpdate?.Invoke(this, EventActionArgs.Empty);
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