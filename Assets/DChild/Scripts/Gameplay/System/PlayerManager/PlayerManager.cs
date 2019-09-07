using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Inputs;
using Holysoft;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface IPlayerManager
    {
        Player player { get; }
        IAutoReflexHandler autoReflex { get; }
        void Register(Player player);
        PlayerCharacterOverride OverrideCharacterControls();
        void StopCharacterControlOverride();
    }

    public class PlayerManager : MonoBehaviour, IGameplaySystemModule, IPlayerManager
    {
        [SerializeField, BoxGroup("Player Data")]
        private Player m_player;
        [SerializeField]
        private PlayerInput m_input;
        [SerializeField]
        private PlayerCharacterOverride m_overrideController;

        [SerializeField]
        private AutoReflexHandler m_autoReflex;

        public Player player => m_player;
        public IAutoReflexHandler autoReflex => m_autoReflex;

        public void DisableInput() => m_input?.Disable();
        public void EnableInput() => m_input?.Enable();

        public PlayerCharacterOverride OverrideCharacterControls()
        {
            m_player.controller.Disable();
            m_overrideController.enabled = true;
            return m_overrideController;
        }

        public void StopCharacterControlOverride()
        {
            m_overrideController.enabled = false;
            m_player.controller.Enable();
        }

        public void Register(Player player)
        {
            m_player = player;
            m_input = m_player.GetComponent<PlayerInput>();
        }

        private void Start()
        {
            if (m_player)
            {
                Register(m_player);
            }
            //m_autoReflex.Initialize();
        }

        private void Update()
        {
            //m_autoReflex.Update(Time.deltaTime);
        }
    }
}