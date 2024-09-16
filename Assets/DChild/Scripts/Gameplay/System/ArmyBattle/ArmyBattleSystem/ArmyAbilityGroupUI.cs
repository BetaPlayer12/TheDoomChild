using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAbilityGroupUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_groupLabel;
        [SerializeField]
        private TextMeshProUGUI m_descriptionLabel;

        public void Display(ArmyAbilityGroup group)
        {
            if (group != null)
            {
                UpdateText(m_groupLabel, group.name);
                //UpdateText(m_descriptionLabel, group.description);
            }
            else
            {
                UpdateText(m_groupLabel, "Name Of Group");
                UpdateText(m_descriptionLabel, "Description:");
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