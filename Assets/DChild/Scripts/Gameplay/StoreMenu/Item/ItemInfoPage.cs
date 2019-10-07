using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
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
            }
            else
            {
                m_name.text = data.itemName;
                m_icon.sprite = data.icon;
                m_description.text = data.description;
                m_quantityLimit.text = data.quantityLimit.ToString();
            }
        }
    }
}
