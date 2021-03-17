using System;
using System.Collections;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Inputs;
using Doozy.Engine;
using Holysoft;
using Holysoft.Collections;
using Holysoft.Event;
using PlayerNew;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface IPlayerManager
    {
        Player player { get; }
        IAutoReflexHandler autoReflex { get; }
        PlayerCharacterOverride OverrideCharacterControls();

        void StopCharacterControlOverride();
        void DisableControls();
        void EnableControls();
    }

    public class PlayerManager : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable, IPlayerManager
    {
        [SerializeField, BoxGroup("Player Data")]
        private Player m_player;
        [SerializeField]
        private GameplayInput m_gameplayInput;
        [SerializeField]
        private InputTranslator m_characterInput;
        [SerializeField]
        private PlayerCharacterOverride m_overrideController;
        [SerializeField]
        private CountdownTimer m_respawnDelay;
        private bool m_playerIsDead;

        [SerializeField]
        private AutoReflexHandler m_autoReflex;

        private CollisionRegistrator m_collisionRegistrator;
       
        public Player player => m_player;

        public GameplayInput gameplayInput => m_gameplayInput;
        public IAutoReflexHandler autoReflex => m_autoReflex;

        public void DisableInput()
        {
            m_gameplayInput?.SetStoreInputActive(false);
            m_characterInput?.Disable();
        }

        public void EnableInput()
        {
            m_gameplayInput?.SetStoreInputActive(true);
            m_characterInput?.Enable();
        }

        public PlayerCharacterOverride OverrideCharacterControls()
        {
            m_gameplayInput?.SetStoreInputActive(false);
            m_characterInput?.Disable();
            m_player.controller.Disable();
            m_player.controller.Enable();
            m_overrideController.enabled = true;
            return m_overrideController;
        }

        public void DisableControls()
        {
            m_gameplayInput?.SetStoreInputActive(false);
            m_characterInput?.Disable();
            m_player.controller.Disable();
        }

        public void EnableControls()
        {
            m_gameplayInput?.SetStoreInputActive(true);
            m_characterInput?.Enable();
            m_player.controller.Enable();
        }

        public void ClearCache() => m_collisionRegistrator?.ClearCache();

        public void StopCharacterControlOverride()
        {
            m_overrideController.enabled = false;
            m_gameplayInput?.SetStoreInputActive(true);
            m_characterInput?.Enable();
            m_player.controller.Enable();
        }

        public void ForceCharacterControlOverride()
        {
            OverrideCharacterControls();
        }

        public void Initialize()
        {
            m_collisionRegistrator = m_player.character.GetComponentInChildren<CollisionRegistrator>();
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            m_respawnDelay.CountdownEnd += OnRespawnPlayer;
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (m_player)
            {
                m_player.SetPosition(eventArgs.slot.spawnPosition);
                m_player.LoadData(eventArgs.slot.characterData);
            }
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (m_player)
            {
                eventArgs.slot.UpdateCharacterData(m_player.SaveData());
            }
        }
        private void OnPlayerDeath(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.gamplayUIHandle.ShowGameOverScreen(true);
            // m_input.Disable();
            //  m_player.controller.Disable();
            m_playerIsDead = true;
            m_respawnDelay.Reset();
        }
        private void OnRespawnPlayer(object sender, EventActionArgs eventArgs)
        {
            //GameplaySystem.LoadGame(GameplaySystem.campaignSerializer.slot, Menu.LoadingHandle.LoadType.Smart);
            //GameplaySystem.campaignSerializer.Load();
            //m_playerIsDead = false;
        }

        private void Start()
        {
            if (m_player)
            {
                m_player = player;
                m_player.OnDeath += OnPlayerDeath;
            }
            //m_autoReflex.Initialize();
        }

        private void Update()
        {
            //m_autoReflex.Update(Time.deltaTime);
            if (m_playerIsDead)
            {
                m_respawnDelay.Tick(Time.deltaTime);
            }
        }
    }
}