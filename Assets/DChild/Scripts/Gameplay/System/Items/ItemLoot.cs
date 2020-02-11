using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemLoot : Loot
    {
        [SerializeField, OnValueChanged("OnDataChange")]
        private ItemData m_data;

#if UNITY_EDITOR
        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        private void OnDataChange()
        {
            m_spriteRenderer.sprite = m_data.icon;
            gameObject.name = m_data.name.Replace(" ", string.Empty) + "Loot";
        }
#endif

        public void SetData(ItemData data)
        {
            m_data = data;
#if UNITY_EDITOR
            OnDataChange();
#endif
        }

        public override void PickUp(IPlayer player)
        {
            m_pickedBy = player;
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
            DisableEnvironmentCollider();
        }
    }
}
