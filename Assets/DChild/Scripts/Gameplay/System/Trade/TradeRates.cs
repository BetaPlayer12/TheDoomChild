using DChild.Gameplay.Inventories;
using DChild.Menu.Trade;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade
{

    [CreateAssetMenu(fileName = "TradeRates", menuName = "DChild/Gameplay/Trade/Trade Rates")]
    public class TradeRates : ScriptableObject
    {
        [BoxGroup("AskingPrice")]
        [SerializeField, TabGroup("AskingPrice/Tab", "Buying"), HideLabel]
        private TradeAskingPrice m_buyAskingPrice = new TradeAskingPrice();
        [SerializeField, TabGroup("AskingPrice/Tab", "Selling"), HideLabel]
        private TradeAskingPrice m_sellAskingPrice = new TradeAskingPrice();

        public TradeAskingPrice buyAskingPrice => m_buyAskingPrice;
        public TradeAskingPrice sellAskingPrice => m_sellAskingPrice;
    }
}