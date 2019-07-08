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
            m_name.text = data.itemName;
            m_icon.sprite = data.icon;
            m_description.text = data.description;
            m_quantityLimit.text = data.quantityLimit.ToString();
        }
    }
}
