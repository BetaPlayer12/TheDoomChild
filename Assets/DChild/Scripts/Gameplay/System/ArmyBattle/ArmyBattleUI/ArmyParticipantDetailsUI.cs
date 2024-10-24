using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyParticipantDetailsUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyBannerUI m_playerBanner;
        [SerializeField]
        private TextMeshProUGUI m_playerPower;
        [SerializeField]
        private ArmyBannerUI m_enemyBanner;
        [SerializeField]
        private TextMeshProUGUI m_enemyPower;

        public void Display(ArmyController player, ArmyController enemy)
        {
            m_playerBanner.Display(player.controlledArmy.overview);
            m_enemyBanner.Display(enemy.controlledArmy.overview);

            m_playerPower.text = $"{player.controlledArmy.troopCount}";
            m_enemyPower.text = $"{enemy.controlledArmy.troopCount}";
        }
    }
}