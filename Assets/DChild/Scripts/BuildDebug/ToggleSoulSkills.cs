using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.SoulSkills;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleSoulSkills : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField, AssetList]
        private SoulSkill m_skill;

        public bool value => GameplaySystem.playerManager.player.soulSkills.HasAcquired(m_skill);

        [Button]
        public void ToggleOn()
        {
            GameplaySystem.playerManager.player.soulSkills.AddAsAcquired(m_skill.id);
        }


        [Button]
        public void ToggleOff()
        {
            GameplaySystem.playerManager.player.soulSkills.RemoveAsActivated(m_skill);
            GameplaySystem.playerManager.player.soulSkills.RemoveAsAcquired(m_skill.id);
        }
    }
}
