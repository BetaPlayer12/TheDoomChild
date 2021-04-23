using DChild.Gameplay;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{

    /// <summary>
    /// Show Info Of Item in UI
    /// </summary>
    public class ItemInfoPage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField]
        private TextMeshProUGUI m_quantityLimit;
        [SerializeField]
        private Button m_useItemButton;

        private ConsumableItemData m_consumableItem;

        public void SetInfo(ItemData data)
        {
            if (data == null)
            {
                m_name.text = "Nothing";
                m_icon.sprite = null;
                m_description.text = "You have nothing, this is not a lack of something but the absence of everything.\n " +
                                    "Do not worry having nothing is fine but if you still see this when you should have something is troubling" +
                                    "Please make sure you have nothing first before saying nothing is fine";
                m_quantityLimit.text = "0";
                m_useItemButton.gameObject.SetActive(false);
            }
            else
            {
                m_name.text = data.itemName;
                m_icon.sprite = data.icon;
                m_description.text = data.description;
                m_quantityLimit.text = data.quantityLimit.ToString();
                if(data.category == ItemCategory.Consumable) 
                {
                    m_consumableItem = (ConsumableItemData)data;
                    m_useItemButton.gameObject.SetActive(true);
                }
                else
                {
                    m_useItemButton.gameObject.SetActive(false);
                }
                
            }
        }

        public void UseItem()
        {
            var player = GameplaySystem.playerManager.player;
            if (m_consumableItem.CanBeUse(player))
            {
                m_consumableItem.Use(player);
                player.inventory.AddItem(m_consumableItem, -1);
                if(player.inventory.GetCurrentAmount(m_consumableItem) == 0)
                {

                }
            }
        }

    }
}
