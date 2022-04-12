using DChild.Gameplay.Items;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{

    public class IndividualLootAcquiredUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_nameLabel;
        [SerializeField]
        private TextMeshProUGUI m_countLabel;

        private Canvas m_canvas;

        public void SetDetails(ItemData item, int count)
        {
            m_nameLabel.text = item.itemName;
            m_countLabel.text = $"+ {count}";
        }

        public void Show()
        {
            m_canvas.enabled = true;
        }

        public void Hide()
        {
            m_canvas.enabled = false;
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }
    }
}