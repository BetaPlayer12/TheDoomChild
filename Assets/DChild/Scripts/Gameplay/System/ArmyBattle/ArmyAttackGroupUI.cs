using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackGroupUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_groupLabel;
        [SerializeField]
        private TextMeshProUGUI m_powerLabel;

        public void Display(ArmyAttackGroup group)
        {
            UpdateText(m_groupLabel, group.groupName);
            UpdateText(m_powerLabel, group.GetTotalPower().ToString());
        }

        private void UpdateText(TextMeshProUGUI label, string value)
        {
            if (label != null)
            {
                label.text = value;
            }
        }
    }
}