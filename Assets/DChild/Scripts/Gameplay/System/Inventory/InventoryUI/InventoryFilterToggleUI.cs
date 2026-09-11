using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public abstract class InventoryFilterToggleUI : MonoBehaviour
    {
        //[SerializeField] protected abstract FilteredInventoryListUI<T> m_attachedInventory;

        [SerializeField] protected UIToggle m_toggle;
        //[SerializeField] protected FilteredInventoryListUI<IInventory> m_inventoryUI;

        [SerializeField] protected ItemCategory m_category;
        public ItemCategory category => m_category;
        public bool isAvailable => gameObject.activeInHierarchy && m_toggle.isActiveAndEnabled && m_toggle.interactable;
        public bool isSelected => m_toggle.IsOn;

        [BoxGroup("Category Icon"), SerializeField] protected Image m_targetIcon;
        [FoldoutGroup("Category Icon/State Sprites"), SerializeField] protected Sprite m_hasItems;
        [FoldoutGroup("Category Icon/State Sprites"), SerializeField] protected Sprite m_noItems;
        [FoldoutGroup("Category Icon/State Sprites"), SerializeField] protected Sprite m_hasItemsAndSelected;

        [BoxGroup("Category Background"), SerializeField] protected Image m_targetBG;
        [FoldoutGroup("Category Background/State Sprites"), SerializeField] protected Sprite m_notSelectedBG;
        [FoldoutGroup("Category Background/State Sprites"), SerializeField] protected Sprite m_selectedBG;

        [SerializeField] protected TextMeshProUGUI m_labelPanel;

        public virtual void SelectFilter()
        {
            UpdateLabel();
        }

        public void Select() => m_toggle.SetIsOn(true);

        protected void UpdateLabel()
        {
            var updatedLabel = "";
            switch (m_category)
            {
                case ItemCategory.Key | ItemCategory.Quest:
                    updatedLabel = "Quest Items";
                    break;
                case ItemCategory.SoulEquipment:
                    updatedLabel = "Equipment";
                    break;
                case ItemCategory.All:
                    updatedLabel = "All Items";
                    break;
                default:
                    updatedLabel = $"{m_category}";
                    break;
            }

            m_labelPanel.text = updatedLabel;
        }
        public abstract bool HasItemsOfCategory();
        public void UpdateToggleVisuals()
        {
            if (!HasItemsOfCategory())
                return;

            m_targetIcon.sprite = HasItemsOfCategory()
                ? m_toggle.IsOn
                    ? m_hasItemsAndSelected
                    : m_hasItems
                : m_noItems;

            m_targetBG.sprite = m_toggle.IsOn ? m_selectedBG : m_notSelectedBG;
        }
    }
}
