using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Trading
{
    [CreateAssetMenu(fileName = "TradeAskingPriceData",menuName = "DChild/Database/Trade Asking Price Data")]
    public class TradeAskingPriceData : SerializedScriptableObject
    {
        [OdinSerialize, HideReferenceObjectPicker,OnValueChanged("UpdatePrice",true)]
        private Dictionary<ItemData, float> m_priceModifier = new Dictionary<ItemData, float>();

        public bool TryGetPriceModifier(ItemData data, out float value)
        {
            return m_priceModifier.TryGetValue(data, out  value);
        }

#if UNITY_EDITOR
        [SerializeField, ReadOnly, HideInInlineEditors, PropertySpace(SpaceBefore =20)]
        private Dictionary<ItemData, int> m_price;

        private void UpdatePrice()
        {
            m_price.Clear();
            foreach (var item in m_priceModifier.Keys)
            {
                m_price.Add(item, Mathf.CeilToInt(item.cost * (m_priceModifier[item] / 100f)));
            }
        }
#endif
    }
}