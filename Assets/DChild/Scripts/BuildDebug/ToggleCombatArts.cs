using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleCombatArts : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private CombatArt m_ability;
        public bool value => false;

        [Button]
        public void ToggleOn()
        {
            //GameplaySystem.playerManager.player.soulSkills.AddAsAcquired(m_skill.id);
        }


        [Button]
        public void ToggleOff()
        {
            //GameplaySystem.playerManager.player.soulSkills.RemoveAsActivated(m_skill);
            //GameplaySystem.playerManager.player.soulSkills.RemoveAsAcquired(m_skill.id);
        }
    }
}
