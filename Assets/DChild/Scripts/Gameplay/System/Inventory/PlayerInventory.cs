using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DChild.Gameplay.Inventories
{
    public class PlayerInventory : SerializedMonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private int m_soulEssence;
        [SerializeField]
        private IItemContainer m_items;
        [SerializeField]
        private IItemContainer m_soulCrystals;
        [SerializeField]
        private IItemContainer m_questItems;

        public void AddSoulEssence(int value) => m_soulEssence = Mathf.Max(m_soulEssence + value, 0);
        public void SetSoulEssence(int value) => m_soulEssence = value;
    }
}
