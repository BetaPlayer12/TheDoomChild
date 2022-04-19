using TMPro;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class FilteredTradePool : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_filterName;

        public void ApplyFilter(TradePoolFilter filter)
        {
            m_filterName.text = filter.ToString();
        }
    }
}