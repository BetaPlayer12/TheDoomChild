using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public class PlayerBehaviourLock : IStatusEffectModule
    {
        [SerializeField]
        private PlayerBehaviour[] m_behaviourToLock;

        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
            if (character == GameplaySystem.playerManager.player.character)
            {
                var behaviourModule = GameplaySystem.playerManager.player.behaviourModule;
                for (int i = 0; i < m_behaviourToLock.Length; i++)
                {
                    behaviourModule.SetModuleActive(m_behaviourToLock[i], false);
                }
            }
        }

        public void Stop(Character character)
        {
            if (character == GameplaySystem.playerManager.player.character)
            {
                var behaviourModule = GameplaySystem.playerManager.player.behaviourModule;
                for (int i = 0; i < m_behaviourToLock.Length; i++)
                {
                    behaviourModule.SetModuleActive(m_behaviourToLock[i], true);
                }
            }
        }
    }
}