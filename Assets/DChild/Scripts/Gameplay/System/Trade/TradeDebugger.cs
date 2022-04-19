using DChild.Gameplay.Characters.NPC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public class TradeDebugger : SerializedMonoBehaviour
    {
        [SerializeField]
        private ITradeInventory m_player;
        [SerializeField]
        private ITradeInventory m_merchant;
        [SerializeField]
        private NPCProfile m_merchantProfile;
        [SerializeField]
        private TradeRates m_rates;
        [SerializeField]
        private TradeManager m_manager;


        [Button]
        public void Commence()
        {
            m_manager.SetSellerProfile(m_merchantProfile);
            //m_manager.SetSellingTradeRates(m_rates.sellAskingPrice);
            m_manager.SetupTrade(m_player, m_merchant);
        }
    }
}