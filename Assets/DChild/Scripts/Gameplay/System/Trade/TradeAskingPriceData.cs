using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Menu.Trade
{
    [CreateAssetMenu(fileName = "TradeAskingPriceData", menuName = "DChild/Gameplay/Trade/Trade Asking Price Data")]
    public class TradeAskingPriceData : SerializedScriptableObject
    {
        [OdinSerialize, HideReferenceObjectPicker, OnValueChanged("UpdatePrice", true)]
        private Dictionary<ItemData, int> m_priceModifier = new Dictionary<ItemData, int>();

        public bool TryGetPriceModifier(ItemData data, out int value)
        {
            return m_priceModifier.TryGetValue(data, out value);
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private ItemList m_reference;

        [SerializeField, ReadOnly, HideInInlineEditors, PropertySpace(SpaceBefore = 20)]
        private Dictionary<ItemData, int> m_price;

        private void UpdatePrice()
        {
            m_price.Clear();
            foreach (var item in m_priceModifier.Keys)
            {
                if(m_priceModifier[item] < 0)
                {
                    //Use Original Price
                    m_price.Add(item,item.cost);
                }
                else
                {
                    m_price.Add(item, m_priceModifier[item]);
                }
                
            }
        }

        [Button, PropertyOrder(-1)]
        private void AddItemsToList()
        {
            var ids = m_reference.GetIDs();
            for (int i = 0; i < ids.Length; i++)
            {
                var item = m_reference.GetInfo(ids[i]);
                if (m_priceModifier.ContainsKey(item) == false)
                {
                    m_priceModifier.Add(item, -1);
                    EditorUtility.SetDirty(this);
                }
            }
            UpdatePrice();
        }
#endif
    }
}