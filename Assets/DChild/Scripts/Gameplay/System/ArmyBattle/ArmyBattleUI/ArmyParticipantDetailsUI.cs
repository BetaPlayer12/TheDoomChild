using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyParticipantDetailsUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyBannerUI m_playerBanner;
        [SerializeField]
        private ArmyBannerUI m_enemyBanner;

        public void Display(ArmyController player, ArmyController enemy)
        {
            m_playerBanner.Display(player.controlledArmy.overview);
            m_enemyBanner.Display(enemy.controlledArmy.overview);
        }
    }
}