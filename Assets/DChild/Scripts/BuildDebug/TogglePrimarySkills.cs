using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class TogglePrimarySkills : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private PrimarySkill m_skill;
        [SerializeField]
        private PlayerModuleActivator m_skills;
        public bool value => m_skills.IsModuleActive(m_skill);

        [Button]
        public void ToggleOn()
        {
            GameplaySystem.playerManager.player.skills.SetSkillStatus(m_skill, true);
        }

        
        [Button]
        public void ToggleOff()
        {
            GameplaySystem.playerManager.player.skills.SetSkillStatus(m_skill, false);
        }
    }
}
