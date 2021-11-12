using DChild.Gameplay.Inventories.QuickItem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemCooldown : MonoBehaviour
    {
        [SerializeField]
        private QuickItemCooldownUI m_ui;
        [SerializeField, MinValue(0)]
        private float m_itemCooldownDuration = 3;
        private float m_cooldownTimer;

        public bool isOver => m_cooldownTimer <= 0;
        private float cooldownTimePercentage => Mathf.Max(0, m_cooldownTimer / m_itemCooldownDuration);

        public void StartCooldown()
        {
            m_cooldownTimer = m_itemCooldownDuration;
            m_ui.DisplayCooldown(cooldownTimePercentage);
            enabled = true;
        }

        public void ResetCooldown()
        {
            m_cooldownTimer = 0;
            m_ui.DisplayCooldown(0);
            enabled = false;
        }

        private void LateUpdate()
        {
            m_cooldownTimer -= GameplaySystem.time.deltaTime;
            m_ui.DisplayCooldown(cooldownTimePercentage);
            if (m_cooldownTimer <= 0)
            {
                enabled = false;
            }
        }
    }
}
