using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemLoot : Loot
    {
        [SerializeField]
        private ItemData m_data;

        public void SetData(ItemData data)
        {
            m_data = data;
        }

        protected override void ApplyPickUp()
        {
            base.ApplyPickUp();
            if (m_pickedBy.inventory.HasSpaceFor(m_data))
            {
                m_pickedBy.inventory.AddItem(m_data);
            }
            else if (m_data is UsableItemData)
            {
                ((UsableItemData)m_data).Use(m_pickedBy);
            }
        }
    }
}
