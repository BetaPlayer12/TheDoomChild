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
        WholeNumber soulEssence { get; }
        IAutoReflexHandler autoReflex { get; }
        void Register(Player player);
    }

    public class PlayerManager : MonoBehaviour, IGameplaySystemModule, IPlayerManager
    {
        [SerializeField, BoxGroup("Player Data")]
        private Player m_player;
        [SerializeField, BoxGroup("Player Data")]
        private WholeNumber m_soulEssence;
        private PlayerInput m_input;

        [SerializeField, HideInInspector]
        private PlayerStatUIHandler m_uiHandler;
        [SerializeField]
        private AutoReflexHandler m_autoReflex;

        public Player player => m_player;
        public WholeNumber soulEssence => m_soulEssence;
        public IAutoReflexHandler autoReflex => m_autoReflex;

        public void DisableInput() => m_input?.Disable();
        public void EnableInput() => m_input?.Enable();

        public void Register(Player player)
        {
            m_player = player;
            m_input = m_player.GetComponent<PlayerInput>();
            m_uiHandler.ConnectTo(player);
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

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_uiHandler, ComponentUtility.ComponentSearchMethod.Child);
        }
    }
}