using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
namespace DChild.Gameplay.Inventories
{


    public class PlayerInventory : SerializedMonoBehaviour, ICurrency
    {
        [SerializeField]
        private ItemList m_itemList;


        [SerializeField, MinValue(0), BoxGroup("Inventory")]
        private int m_soulEssence;
        [SerializeField, BoxGroup("Inventory")]
        private IItemContainer m_items;
        [SerializeField, BoxGroup("Inventory")]
        private IItemContainer m_soulCrystals;
        [SerializeField, BoxGroup("Inventory")]
        private IItemContainer m_questItems;

        public int amount => m_soulEssence;

        public event EventAction<CurrencyUpdateEventArgs> OnAmountSet;
        public event EventAction<CurrencyUpdateEventArgs> OnAmountAdded;

        public PlayerInventoryData Save()
        {
            return new PlayerInventoryData(m_soulEssence,
                                            m_items.Save(), m_soulCrystals.Save(), m_questItems.Save());
        }

        public void Load(PlayerInventoryData data)
        {
            m_soulEssence = data.soulEssence;
            Load(m_items, data.items);
            Load(m_soulCrystals, data.soulCrystals);
            Load(m_questItems, data.questItems);
        }

        public void AddSoulEssence(int value)
        {
            m_soulEssence = Mathf.Max(m_soulEssence + value, 0);
            OnAmountAdded?.Invoke(this, new CurrencyUpdateEventArgs(value));
        }

        public void SetSoulEssence(int value)
        {
            m_soulEssence = value;
            OnAmountSet?.Invoke(this, new CurrencyUpdateEventArgs(value));
        }

        public void AddItem(ItemData item)
        {
            //TODO: Items are not yet categorized
            m_items.AddItem(item, 1);
        }

        public void RemoveItem(ItemData item)
        {
            //TODO: Items are not yet categorized
            m_items.AddItem(item, -1);
        }

        public int GetCurrentAmount(ItemData item) => m_items.GetCurrentAmount(item);

        public bool HasSpaceFor(ItemData item) => m_items.HasSpaceFor(item);

        private void Load(IItemContainer itemContainer, ItemContainerSaveData saveData)
        {
            itemContainer.ClearList();
            var itemDatas = saveData.datas;
            for (int i = 0; i < itemDatas.Length; i++)
            {
                var itemData = itemDatas[i];
                m_itemList.GetInfo(itemData.ID);
                itemContainer.AddItem(m_itemList.GetInfo(itemData.ID), itemData.count);
            }
        }
    }
}
