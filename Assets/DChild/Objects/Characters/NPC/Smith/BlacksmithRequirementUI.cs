using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.UI
{
    public class BlacksmithRequirementUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_currentCount;

        [SerializeField] private Image m_background;
        [SerializeField] private Image m_requirementIcon;

        [BoxGroup("Sprites"), SerializeField] private Sprite m_missingSprite;
        [BoxGroup("Sprites"), SerializeField] private Sprite m_insufficientSprite;
        [BoxGroup("Sprites"), SerializeField] private Sprite m_completeSprite;

        private void UpdateBackground(int current, int required)
        {
            m_requirementIcon.color = current < required
                ? new Color32(77, 77, 77, 255)
                : Color.white;

            Sprite targetSprite;

            if (current <= 0)
            {
                targetSprite = m_missingSprite;
            }
            else if (current < required)
            {
                targetSprite = m_insufficientSprite;
            }
            else
            {
                targetSprite = m_completeSprite;
            }

            if (m_background.sprite != targetSprite)
            {
                m_background.sprite = targetSprite;
            }
        }

        public void SetIcon(Sprite value) => m_requirementIcon.sprite = value;
        public void SetLabel(int current, int required)
        {
            m_currentCount.text = $"{current} of {required}";
        }

        public void SetDynamicVisuals(ItemData item, int inventoryQuantity, int required)
        {
            SetIcon(item.icon);
            SetLabel(inventoryQuantity, required);
            UpdateBackground(inventoryQuantity, required);
        }    

    }

}
