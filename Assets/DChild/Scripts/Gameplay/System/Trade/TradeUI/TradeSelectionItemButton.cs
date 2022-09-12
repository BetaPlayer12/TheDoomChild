using DChild.Gameplay.Inventories;
using Doozy.Engine.UI;
using Holysoft.Event;
using Sirenix.Serialization.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trade.UI
{
    public class TradeSelectionItemButton : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_count;

        private ITradeItem m_itemDisplayed;
        private UIButton m_button;

        public event EventAction<SelectedItemEventArgs> ItemSelected;

        public void DisplayItem(ITradeItem item)
        {
            m_itemDisplayed = item;
            m_icon.sprite = item.data.icon;
            m_count.text = item.count.ToString();
        }

        public void SelectItem()
        {
            using (Cache<SelectedItemEventArgs> selectedItemEvent = Cache<SelectedItemEventArgs>.Claim())
            {
                selectedItemEvent.Value.Set(m_itemDisplayed);
                ItemSelected?.Invoke(this, selectedItemEvent.Value);
                selectedItemEvent.Release();
            }
        }
    }

}