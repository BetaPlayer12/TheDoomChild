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

        public void Display(ArmyAttackGroup group, float modifier = 1)
        {
            if (group != null)
            {
                UpdateText(m_groupLabel, group.name);
                var modifiedPower = ArmyBattleUtlity.CalculateModifiedPower(group.GetTotalPower(), modifier);
                UpdateText(m_powerLabel, modifiedPower.ToString());
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