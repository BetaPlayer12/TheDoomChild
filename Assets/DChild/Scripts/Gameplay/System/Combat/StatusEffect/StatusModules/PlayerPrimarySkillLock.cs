using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{

    [System.Serializable]
    public class PlayerPrimarySkillLock : IStatusEffectModule
    {
        [SerializeField]
        private PrimarySkill[] m_skillToLock;

        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
            if (character == GameplaySystem.playerManager.player.character)
            {
                var behaviourModule = GameplaySystem.playerManager.player.behaviourModule;
                for (int i = 0; i < m_skillToLock.Length; i++)
                {
                    behaviourModule.SetModuleActive(m_skillToLock[i], false);
                }
            }
        }

        public void Stop(Character character)
        {
            if (character == GameplaySystem.playerManager.player.character)
            {
                var behaviourModule = GameplaySystem.playerManager.player.behaviourModule;
                for (int i = 0; i < m_skillToLock.Length; i++)
                {
                    behaviourModule.SetModuleActive(m_skillToLock[i], true);
                }
            }
        }
    }
}