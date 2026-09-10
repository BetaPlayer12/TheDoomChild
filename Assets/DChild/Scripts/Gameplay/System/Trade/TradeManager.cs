using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Inventories.UI;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Trade.UI;
using DChild.Menu;
using DChild.Menu.Trade;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using DChild.Gameplay.Items;
using I2.Loc;
using DChild.Localization;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Doozy._Examples.E24___Popup___with_Two_Buttons;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Trade
{
    public class TradeManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private TradeHandle m_tradeHandle;
        //[SerializeField]
        //private TradeOptionHandle m_tradeOption;
        [SerializeField]
        private TradeUIAppearanceInitializer m_appearanceInitializer;
        [SerializeField]
        private TransactionDetailsUI m_transactionDetails;
        [SerializeField]
        private TradePlayerCurrencies m_playerCurrencies;
        [SerializeField]
        private FilteredInventoryListUI<ITradeInventory> m_listUI;
        [SerializeField]
        private ItemUI m_firstSelectedItemUI;
        [SerializeField]
        private TradeDetailsUI m_itemBeingTradedUI;
        [SerializeField]
        private NPCProfileUI m_sellerProfile;

        [SerializeField]
        private InventoryFilterToggleUI[] m_filterToggles;
        [SerializeField]
        private UIButton m_tradeButton;
        [SerializeField]
        private InputActionReference m_cycleSubTabInput;


        //[SerializeField]
        //private Image m_highlight;
        //[SerializeField]
        //private UIToggle m_defaultToggle;

        [SerializeField]
        private ConfirmationHandler m_tradeConfirmation;

        // Localizations
        [TermsPopup]
        public string _localizeMessage;
        private TradeShopVariableLocalizer m_termLocalizer;
        private UIContainer m_tradeView;
        private UIContainer m_confirmationView;
        private CanvasGroup m_tradeButtonCanvasGroup;
        [SerializeField]
        private bool Localize = true;

        public void SetupTrade(ITradeInventory buyer, ITradeInventory seller, CurrencyType type)
        {
            //m_defaultToggle.SetIsOn(true);

            m_tradeHandle.SetCurrencyToTrade(type);
            m_tradeHandle.SetTraders(buyer, seller);
            //m_tradeOption.ChangeToBuyOption(true);
            m_itemBeingTradedUI.SetCostTypeToDisplay(type);
            ResetTradeUI();

            UpdateCurrencyUI();
        }

        public void SetSellerProfile(NPCProfile profile)
        {
            m_sellerProfile.Set(profile);
            m_appearanceInitializer.SetShopAppearance(profile);
        }

        public void SetSellingTradeRates(TradeAskingPrice sellingPriceRate)
        {
            m_tradeHandle.SetSellingTradeRate(sellingPriceRate);
        }

        public void Select(ItemUI item)
        {
            //if (item.reference == null) return;

            m_tradeHandle.SetItemToTrade((ITradeItem)item.reference);
            DisplayItemDetails(item.reference);
            //m_highlight.enabled = true;
            //m_highlight.rectTransform.position = item.transform.position;
            UpdateTradeInteractability();
        }

        private void UpdateTradeInteractability()
        {
            bool canTrade = m_tradeHandle.CanBuyerAffordTransaction();
            m_tradeButton.interactable = canTrade;
            m_tradeButton.SetState(canTrade ? UISelectionState.Normal : UISelectionState.Disabled);
            m_tradeButtonCanvasGroup.alpha = canTrade ? 1f : 0.5f;
            //m_tradeOption.SetInteractability(enableTradeButton);
        }

        public void SetTradeFilter(ItemCategory filter)
        {
            m_listUI.SetFilter(filter);
            InitializeTradeUI();
        }

        public void ResetTradeUI()
        {
            //m_listUI.ResetFilter();
            InitializeTradeUI();
        }

        private void SetupFilterToggles()
        {
            foreach (var toggle in m_filterToggles)
            {
                toggle.UpdateToggleVisuals();
            }
        }

        public void InitializeTradeUI()
        {
            m_listUI.Reset();
            m_listUI.SetInventoryReference(m_tradeHandle.currentSeller);
            SetupFilterToggles();
            Select(m_firstSelectedItemUI);
        }

        public void RequestConfirmTrade()
        {
            var transaction = m_tradeHandle.transactionInfo;
            var pluralization = transaction.count > 1 ? "s " : " ";
            var currencyTypeMsg = GetCurrencyTypeInString();
            if (Localize)
            {
                m_termLocalizer.TradeValueLocalize(transaction.count.ToString(), transaction.item, pluralization, currencyTypeMsg, transaction.totalCost.ToString());
                m_tradeConfirmation.RequestConfirmation(OnTradeConfirmed, null, null, true);
                return;
            }
            var message = $"Would you like to Trade {transaction.count} {transaction.item.itemName}{pluralization} for {currencyTypeMsg} {transaction.totalCost}";
            m_tradeConfirmation.RequestConfirmation(OnTradeConfirmed, "Purchase", message);
        }

        private void OnTradeConfirmed(object sender, EventActionArgs eventArgs)
        {
            m_tradeHandle.CommenceTrade();
            if (m_tradeHandle.currentItemBeingTraded.count == 0)
            {
                Select(m_firstSelectedItemUI);
            }
            else
            {
                m_tradeHandle.SetItemToTrade(m_tradeHandle.currentItemBeingTraded);
                DisplayItemDetails(m_tradeHandle.currentItemBeingTraded);
            }
            UpdateCurrencyUI();
            UpdateTradeInteractability();
        }

        private string GetCurrencyTypeInString()
        {
            switch (m_tradeHandle.currencyTypeToTrade)
            {
                case CurrencyType.SoulEssence:
                    return "S.E";

                case CurrencyType.SilverCoin:
                    return "S.C";
                default:
                    return "N/A";
            }
        }

        private void UpdateCurrencyUI()
        {
            var buyer = m_tradeHandle.currentBuyer;
            m_playerCurrencies.UpdateUI(
                buyer.GetCurrencyAmount(CurrencyType.SoulEssence),
                buyer.GetCurrencyAmount(CurrencyType.SilverCoin));
        }

        private void DisplayItemDetails(IStoredItem item)
        {
            m_itemBeingTradedUI.ShowDetails(item);
            if (item == null)
                return;

            var ownedItem = m_tradeHandle.currentBuyer.GetTradeItem(item.data);
            m_itemBeingTradedUI.SetOwnedCount(ownedItem?.count ?? 0);
        }

        private void OnCycleSubTab(InputAction.CallbackContext context)
        {
            if (!m_tradeView.isVisible || (m_confirmationView != null && !m_confirmationView.isHidden))
                return;

            float direction = context.ReadValue<float>();
            if (Mathf.Approximately(direction, 0f))
                return;

            CycleFilter(direction > 0f ? 1 : -1);
        }

        private void CycleFilter(int direction)
        {
            int currentIndex = Array.FindIndex(m_filterToggles, filter => filter.isSelected);
            if (currentIndex < 0)
                currentIndex = direction > 0 ? -1 : 0;

            for (int offset = 1; offset <= m_filterToggles.Length; offset++)
            {
                int index = (currentIndex + direction * offset + m_filterToggles.Length) % m_filterToggles.Length;
                if (!m_filterToggles[index].isAvailable)
                    continue;

                m_filterToggles[index].Select();
                return;
            }
        }
        private void Awake()
        {
            m_transactionDetails.SetTransactionReference(m_tradeHandle.transactionInfo);
            m_termLocalizer = GetComponentInChildren<TradeShopVariableLocalizer>();
            m_tradeView = GetComponent<UIContainer>();
            m_confirmationView = m_tradeConfirmation.window.GetComponentInParent<UIContainer>();
            m_tradeButtonCanvasGroup = m_tradeButton.GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            m_cycleSubTabInput.action.performed += OnCycleSubTab;
        }

        private void OnDisable()
        {
            m_cycleSubTabInput.action.performed -= OnCycleSubTab;
        }

    }

}
