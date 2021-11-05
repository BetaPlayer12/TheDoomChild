using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemCooldownUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_cooldownIndicator;

        public void DisplayCooldown(float timerPercentage)
        {
            m_cooldownIndicator.fillAmount = timerPercentage;
        }
    }
}
