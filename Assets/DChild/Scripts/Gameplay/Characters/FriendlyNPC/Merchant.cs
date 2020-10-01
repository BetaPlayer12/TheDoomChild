using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using DChild.Menu.Trading;
using Doozy.Engine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.NPC
{
    public class Merchant : SerializedMonoBehaviour, IButtonToInteract, ITraderAskingPrice
    {
        [SerializeField]
        private Transform m_prompt;
        [SerializeField]
        private MerchantInventory m_inventory;
        [BoxGroup("AskingPrice")]
        [SerializeField, TabGroup("AskingPrice/Tab", "Buying"), HideLabel]
        private TradeAskingPrice m_buyAskingPrice = new TradeAskingPrice();
        [SerializeField, TabGroup("AskingPrice/Tab", "Selling"), HideLabel]
        private TradeAskingPrice m_sellAskingPrice = new TradeAskingPrice();

        public bool showPrompt => true;

        public string promptMessage => "Trade";

        public Vector3 promptPosition => m_prompt.position;

        public void Interact(Character character)
        {
            GameplaySystem.uiModeHandle.OpenTradeWindow(m_inventory,this);
        }

        public void ResetWares() => m_inventory.ResetWares();

        public void AddToWares(ItemData item, int count = 1) => m_inventory.AddToWares(item, count);

        public int GetAskingPrice(ItemData data, TradeType tradeType)
        {
            if (tradeType == TradeType.Buy)
            {
                return m_buyAskingPrice.GetAskingPrice(data);
            }
            else
            {
                return m_sellAskingPrice.GetAskingPrice(data);
            }
        }

        private void Awake()
        {
            ResetWares();
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void CommenceTrade() => Interact(null);


#endif
    }
}