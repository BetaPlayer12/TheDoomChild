using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trade
{
    public class MerchantWareAssigner : MonoBehaviour
    {
        [SerializeField,InlineProperty]
        private ConditionedMechantWaresData m_data;

        private IMerchantStore m_store;


        [Button]
        private void UpdateMerchanteWares()
        {
            m_store.SetWaresReference(m_data.GetAppropriateWares());
            m_store.ResetWares();
        }
    }
}