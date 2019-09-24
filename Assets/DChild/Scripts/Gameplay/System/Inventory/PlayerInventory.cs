using DChild.Gameplay.Systems;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
namespace DChild.Gameplay.Inventories
{
    public class PlayerInventory : SerializedMonoBehaviour, ICurrency
    {
        [SerializeField, MinValue(0)]
        private int m_soulEssence;
        [SerializeField]
        private IItemContainer m_items;
        [SerializeField]
        private IItemContainer m_soulCrystals;
        [SerializeField]
        private IItemContainer m_questItems;

        public int amount => m_soulEssence;

        public event EventAction<CurrencyUpdateEventArgs> OnAmountSet;
        public event EventAction<CurrencyUpdateEventArgs> OnAmountAdded;

        public void AddSoulEssence(int value)
        {
            m_soulEssence = Mathf.Max(m_soulEssence + value, 0);
            OnAmountAdded?.Invoke(this, new CurrencyUpdateEventArgs(value));
        }

        public void SetSoulEssence(int value)
        {
            m_soulEssence = value;
            OnAmountSet?.Invoke(this, new CurrencyUpdateEventArgs(value));
        }
    }
}
