using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.UI;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class PlayerStatUIHandler : MonoBehaviour
    {
        [SerializeField]
        private CappedStatUI m_healthUI;
        [SerializeField]
        private CappedStatUI m_magicUI;

        public void ConnectTo(IPlayer player)
        {
            //m_healthUI.MonitorInfoOf(player.health);
            //m_magicUI.MonitorInfoOf(player.magic);
        }
    }
}