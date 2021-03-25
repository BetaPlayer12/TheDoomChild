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
        private NPCProfile m_npcData;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private MerchantInventory m_inventory;
        [BoxGroup("AskingPrice")]
        [SerializeField, TabGroup("AskingPrice/Tab", "Buying"), HideLabel]
        private TradeAskingPrice m_buyAskingPrice = new TradeAskingPrice();
        [SerializeField, TabGroup("AskingPrice/Tab", "Selling"), HideLabel]
        private TradeAskingPrice m_sellAskingPrice = new TradeAskingPrice();

        public bool showPrompt => true;

        public string promptMessage => "Trade";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.OpenTradeWindow(m_npcData,m_inventory,this);
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

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void CommenceTrade() => Interact(null);


#endif
    }
}