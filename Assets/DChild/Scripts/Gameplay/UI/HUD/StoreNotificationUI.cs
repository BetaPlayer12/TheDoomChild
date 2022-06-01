using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class StoreNotificationUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_header;
        [SerializeField]
        private TextMeshProUGUI m_instructions;

        public void Show(StoreNotificationInfo info)
        {
            m_icon.sprite = info.icon;
            m_header.text = info.headerLabel;
            m_instructions.text = info.instructions;
        }
    }
}