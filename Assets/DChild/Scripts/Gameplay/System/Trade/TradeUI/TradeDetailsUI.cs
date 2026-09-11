using DChild.Gameplay.Inventories;
using DChild.Gameplay.Inventories.UI;
using DChild.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trade.UI
{
    public class TradeDetailsUI : FullItemDetailsUI
    {
        [SerializeField]
        private TextMeshProUGUI m_costTypeLabel;
        [SerializeField]
        private TextMeshProUGUI m_costLabel;
        [SerializeField]
        private TextMeshProUGUI m_countLabel;
        [SerializeField]
        private Image m_currencyIcon;
        [SerializeField]
        private Sprite m_soulEssenceIcon;
        [SerializeField]
        private Sprite m_silverCoinIcon;

        private CurrencyType m_costType;

        public void SetCostTypeToDisplay(CurrencyType costType)
        {
            m_costType = costType;
            switch (m_costType)
            {
                case CurrencyType.SoulEssence:
                    m_costTypeLabel.text = "S.E./";
                    m_currencyIcon.sprite = m_soulEssenceIcon;
                    break;
                case CurrencyType.SilverCoin:
                    m_costTypeLabel.text = "S.C./";
                    m_currencyIcon.sprite = m_silverCoinIcon;
                    break;
            }
        }

        public override void Hide()
        {
        }

        public override void Show()
        {
        }

        public override void ShowDetails(IStoredItem reference)
        {
            base.ShowDetails(reference);
            if(reference == null)
            {
                m_costLabel.text = "";
                m_countLabel.text = "";
                SetQuantityValue("");
            }
            else
            {
                m_costLabel.text = ((ITradeItem)reference).cost.GetCostOfType(m_costType).ToString();
            }
        }

        public void SetOwnedCount(int count) => SetQuantityValue(Mathf.Max(0, count).ToString());
    }
}
