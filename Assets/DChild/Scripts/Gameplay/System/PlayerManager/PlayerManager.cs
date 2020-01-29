using System;
using System.Collections;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Inputs;
using Doozy.Engine;
using Holysoft;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface IPlayerManager
    {
        Player player { get; }
        WholeNumber soulEssence { get; }
        IAutoReflexHandler autoReflex { get; }
<<<<<<< HEAD
        void Register(Player player);
=======
        PlayerCharacterOverride OverrideCharacterControls();
        void StopCharacterControlOverride();
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
    }

    public class PlayerManager : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable, IPlayerManager
    {
        [SerializeField, BoxGroup("Player Data")]
        private Player m_player;
        [SerializeField, BoxGroup("Player Data")]
        private WholeNumber m_soulEssence;
        private PlayerInput m_input;
<<<<<<< HEAD
=======
        [SerializeField]
        private PlayerCharacterOverride m_overrideController;
        [SerializeField]
        private CountdownTimer m_respawnDelay;
        private bool m_playerIsDead;
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23

        [SerializeField]
        private AutoReflexHandler m_autoReflex;

        public Player player => m_player;
        public WholeNumber soulEssence => m_soulEssence;
        public IAutoReflexHandler autoReflex => m_autoReflex;

        public void DisableInput() => m_input?.Disable();
        public void EnableInput() => m_input?.Enable();

<<<<<<< HEAD
        public void Register(Player player)
=======
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

        public void Initialize()
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        {
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
            GameEventMessage.SendEvent("Game Over");
            m_playerIsDead = true;
            m_respawnDelay.Reset();
        }
        private void OnRespawnPlayer(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.LoadGame(GameplaySystem.campaignSerializer.slot);
            GameplaySystem.campaignSerializer.Load();
            m_playerIsDead = false;
        }

        private void Start()
        {
            if (m_player)
            {
                m_player = player;
                m_input = m_player.GetComponent<PlayerInput>();
                m_player.OnDeath += OnPlayerDeath;
            }
            //m_autoReflex.Initialize();
        }
<<<<<<< HEAD
=======

        private void Update()
        {
            //m_autoReflex.Update(Time.deltaTime);
            if (m_playerIsDead)
            {
                m_respawnDelay.Tick(Time.deltaTime);
            }
        }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
    }
}