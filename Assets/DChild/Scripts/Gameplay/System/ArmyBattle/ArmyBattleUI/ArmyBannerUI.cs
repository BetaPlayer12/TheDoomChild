using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBannerUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_armyName;
        [SerializeField]
        private Image m_armyIcon;

        public void Display(ArmyOverviewData overviewData)
        {
            m_armyName.text = overviewData.name.ToUpper();
            m_armyIcon.sprite = overviewData.icon;
        }
    }
}