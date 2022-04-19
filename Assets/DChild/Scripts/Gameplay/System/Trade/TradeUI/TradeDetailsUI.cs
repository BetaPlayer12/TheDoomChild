using DChild.Gameplay.Inventories;
using DChild.Gameplay.Inventories.UI;
using System;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Trade.UI
{
    public class TradeDetailsUI : FullItemDetailsUI
    {
        [SerializeField]
        private TextMeshProUGUI m_costLabel;
        [SerializeField]
        private TextMeshProUGUI m_countLabel;

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
            }
            else
            {
                m_costLabel.text = ((ITradeItem)reference).cost.ToString();
                m_countLabel.text = reference.hasInfiniteCount? "99" : reference.count.ToString();
            }
        }
    }
}
