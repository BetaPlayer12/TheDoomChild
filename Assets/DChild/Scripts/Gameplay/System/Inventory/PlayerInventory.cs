
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [AddComponentMenu("DChild/Gameplay/Player/Player Inventory")]
    public class PlayerInventory : SerializedMonoBehaviour, ICurrency, ITradableInventory
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

        

        public int soulEssence => m_soulEssence;

        int ICurrency.amount => m_soulEssence;
        int ITradableInventory.Count => m_items.Count;

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
            ////MiguelTest
            //Load(m_soulSkills, data.soulSkills);
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

        public void AddItem(ItemData item, uint count = 1)
        {
            var intCount = (int)count;
            //TODO: Items are not yet categorized
            if (item is SoulCrystal)
            {
                m_soulCrystals.AddItem(item, intCount);
            }
            else
            {
                switch (item.category)
                {
                    case ItemCategory.Consumable:
                        m_items.AddItem(item, intCount);
                        break;
                    case ItemCategory.Quest:
                    case ItemCategory.Key:
                        m_questItems.AddItem(item, intCount);
                        break;
                    //    //Miguel Test
                    //case ItemCategory.SoulSkill:
                    //    m_soulSkills.AddItem(item, intCount);
                    //    break;
                    default:
                        break;
                }
            }
        }

        public void RemoveItem(ItemData item, uint count = 1)
        {
            var intCount = (int)count * -1;
            //TODO: Items are not yet categorized
            if (item is SoulCrystal)
            {
                m_soulCrystals.AddItem(item, intCount);
            }
            else
            {
                switch (item.category)
                {
                    case ItemCategory.Consumable:
                        m_items.AddItem(item, intCount);
                        break;
                    case ItemCategory.Quest:
                    case ItemCategory.Key:
                        m_questItems.AddItem(item, intCount);
                        break;
                    default:
                        break;
                }
            }
        }

        public int GetCurrentAmount(ItemData item)
        {
            switch (item.category)
            {
                case ItemCategory.Consumable:
                    return m_items.GetCurrentAmount(item);
                case ItemCategory.Quest:
                case ItemCategory.Key:
                    return m_questItems.GetCurrentAmount(item);

                //    //MiguelTest
                //case ItemCategory.SoulSkill:
                //    return m_soulSkills.GetCurrentAmount(item);
                default:
                    return 0;
            }
        }

        public bool HasSpaceFor(ItemData item) => m_items.HasSpaceFor(item);

        public void AddItem(ItemData item, int count)
        {
            if (count != 0)
            {
                switch (item.category)
                {
                    case ItemCategory.Consumable:
                        m_items.AddItem(item, count);
                        break;
                    case ItemCategory.Quest:
                    case ItemCategory.Key:
                        m_questItems.AddItem(item, count);
                        break;
                        //MiguelTest
                    //case ItemCategory.SoulSkill:
                    //    m_soulSkills.AddItem(item, count);
                    //    break;
                    default:
                        break;
                }
            }
        }

        public bool CanAfford(int cost) => cost <= m_soulEssence;

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

        ItemSlot ITradableInventory.GetSlot(int index) => m_items.GetSlot(index);

    }
}
