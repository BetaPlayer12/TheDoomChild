using DChild.Gameplay.Trade;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trade
{
    public class MerchantWareAssigner : MonoBehaviour
    {
        [SerializeField, InlineProperty]
        private ConditionedMechantWaresData m_data;

        private IMerchantStore m_store;

        [Button,HideInEditorMode]
        public void UpdateMerchanteWares()
        {
            m_store.SetWares(m_data.GetAppropriateWares());
            m_store.ResetWares();
        }

        private void Awake()
        {
            m_store = GetComponent<IMerchantStore>();
            UpdateMerchanteWares();
        }

        private void Start()
        {
            UpdateMerchanteWares();
        }
    }
}