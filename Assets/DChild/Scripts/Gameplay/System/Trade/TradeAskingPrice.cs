using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Menu.Trade
{
    [System.Serializable]
    public class TradeAskingPrice
    {
        [SerializeField, MinValue(0f)]
        private float m_defaultPriceModifier = 100f;
        [SerializeField, InlineEditor]
        private TradeAskingPriceData m_priceModifierData;

        public int GetAskingPrice(ItemData data)
        {
            var modifiedPrice = -1;
            if (m_priceModifierData != null)
            {
                if (m_priceModifierData.TryGetPriceModifier(data, out int value))
                {
                    modifiedPrice = value;
                }
            }
            if (modifiedPrice < 0)
            {
                return data.cost;
            }
            else
            {
                return modifiedPrice;
            }
        }
    }
}