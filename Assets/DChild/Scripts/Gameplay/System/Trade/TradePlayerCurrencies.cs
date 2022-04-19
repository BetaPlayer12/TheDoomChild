using TMPro;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class TradePlayerCurrencies : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_soulEssence;
        [SerializeField]
        private TextMeshProUGUI m_soulCrystals;

        public void UpdateUI(int soulEssence, int soulCrystals)
        {
            m_soulEssence.text = soulEssence.ToString();
            m_soulCrystals.text = soulCrystals.ToString();
        }
    }
}