using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Menu.Trade
{
    [System.Serializable]
    public class TradeAskingPrice
    {
        [SerializeField,MinValue(0f)]
        private float m_defaultPriceModifier = 100f;
        [SerializeField, InlineEditor]
        private TradeAskingPriceData m_priceModifierData;

        public int GetAskingPrice(ItemData data)
        {
            var modifier = m_defaultPriceModifier;
            if(m_priceModifierData != null)
            {
                if(m_priceModifierData.TryGetPriceModifier(data, out float value))
                {
                    modifier = value;
                }
            }
            if(modifier == 100f)
            {
                return data.cost;
            }
            else
            {
                return Mathf.CeilToInt(data.cost * (modifier / 100f));
            }
        }
    }
}