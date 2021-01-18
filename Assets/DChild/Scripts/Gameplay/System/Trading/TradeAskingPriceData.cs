using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Menu.Trading
{
    [CreateAssetMenu(fileName = "TradeAskingPriceData", menuName = "DChild/Database/Trade Asking Price Data")]
    public class TradeAskingPriceData : SerializedScriptableObject
    {
        [OdinSerialize, HideReferenceObjectPicker, OnValueChanged("UpdatePrice", true)]
        private Dictionary<ItemData, float> m_priceModifier = new Dictionary<ItemData, float>();

        public bool TryGetPriceModifier(ItemData data, out float value)
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
                m_price.Add(item, Mathf.CeilToInt(item.cost * (m_priceModifier[item] / 100f)));
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
                    m_priceModifier.Add(item, 100f);
                    EditorUtility.SetDirty(this);
                }
            }
            UpdatePrice();
        }
#endif
    }
}