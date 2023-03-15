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
            if (group != null)
            {
                UpdateText(m_groupLabel, group.groupName);
                UpdateText(m_powerLabel, group.GetTotalPower().ToString());
            }
            else
            {
                UpdateText(m_groupLabel, "Name Of Group");
                UpdateText(m_powerLabel, "Power:");
            }
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