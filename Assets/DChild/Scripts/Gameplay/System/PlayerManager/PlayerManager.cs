using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using DChild.Inputs;
using DChild.Visuals;
using DChild.Temp;
using Holysoft;
using Holysoft.Collections;
using Holysoft.Event;
using PlayerNew;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    public interface IPlayerManager
    {
        Player player { get; }
        IAutoReflexHandler autoReflex { get; }
        PlayerCharacterOverride OverrideCharacterControls();

        bool IsPartOfPlayer(GameObject gameObject);
        bool IsPartOfPlayer(GameObject gameObject, out IPlayer player);

        void StopCharacterControlOverride();
        void DisableControls();
        void EnableControls();
        void EnableIntroControls();
        void DisableIntroControls();
        void EnableIntroAction(List<IntroActions> action);
        void SyncVisualsWith(SpineSyncer spineSyncer);
        IEnumerator PlayerActionChange(Action<PlayerInput> Callback);

        void ReturnPlayerToOrginalScene();
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
        private PlayerInput m_playerInput;

        [SerializeField]
        private AutoReflexHandler m_autoReflex;

        private CollisionRegistrator m_collisionRegistrator;
        private InteractableDetector m_interactableDetector;

        private Scene m_playerOriginalScene;
        private Transform m_playerOriginalParent;

        public Player player => m_player;

        public GameplayInput gameplayInput => m_gameplayInput;
        public IAutoReflexHandler autoReflex => m_autoReflex;

        public void SyncVisualsWith(SpineSyncer spineSyncer)
        {
            player.character.GetComponent<PlayerSpineSyncer>().SyncWith(spineSyncer);
        }

        public void DisableInput()
        {
            m_gameplayInput?.SetStoreInputActive(false);
            m_gameplayInput?.ToggleUINavigationInput(true);
            m_characterInput?.Disable();
        }

        public void EnableInput()
        {
            m_gameplayInput?.SetStoreInputActive(true);
            m_gameplayInput?.ToggleUINavigationInput(false);
            m_characterInput?.Enable();
        }

        public void FreezePlayerPosition(bool freezePlayerPosition)
        {
            var physics = m_player.character.GetComponent<Rigidbody2D>();
            if (freezePlayerPosition)
            {
                physics.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                physics.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        public PlayerCharacterOverride OverrideCharacterControls()
        {
            m_gameplayInput?.SetStoreInputActive(false);
            m_characterInput?.Disable();
            m_player.controller.Disable();
            m_player.controller.Enable();
            m_overrideController.enabled = true;
            m_player.state.allowExtendedIdle = false;
            return m_overrideController;
        }

        public bool IsPartOfPlayer(GameObject gameObject)
        {
            if (gameObject.TryGetComponentInParent(out PlayerControlledObject playerObject))
            {
                return true;
            }
            return false;
        }

        public bool IsPartOfPlayer(GameObject gameObject, out IPlayer player)
        {
            var isPartOfPlayer = IsPartOfPlayer(gameObject);
            player = isPartOfPlayer ? m_player : null;
            return isPartOfPlayer;
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

        public void EnableIntroControls()
        {
            m_player.introController.EnableIntroControls();
        }

        public void DisableIntroControls()
        {
            m_player.introController.DisableIntroControls();
        }

        public void EnableIntroAction(List<IntroActions> action)
        {
            m_player.introController.EnableIntroAction(action);
        }

        public void ClearCache()
        {
            m_collisionRegistrator?.ClearCache();
            m_interactableDetector?.ClearAllInteractableReferences();
        }

        public void StopCharacterControlOverride()
        {
            m_overrideController.enabled = false;
            m_gameplayInput?.SetStoreInputActive(true);
            m_characterInput?.Enable();
            m_player.controller.Enable();
            m_player.state.allowExtendedIdle = true;
        }

        public void ForceCharacterControlOverride()
        {
            OverrideCharacterControls();
        }

        public void ReturnPlayerToOrginalScene()
        {
            m_player.character.transform.parent = m_playerOriginalParent;
            SceneManager.MoveGameObjectToScene(m_player.character.gameObject, m_playerOriginalScene);
        }

        public void Initialize()
        {
            var character = m_player.character;
            m_collisionRegistrator = character.GetComponentInChildren<CollisionRegistrator>();
            m_interactableDetector = character.GetComponentInChildren<InteractableDetector>();

            m_player.Initialize();
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            m_respawnDelay.CountdownEnd += OnRespawnPlayer;
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (eventArgs.IsPartOfTheUpdate(SerializationScope.Player) && m_player)
            {
                m_player.SetPosition(eventArgs.slot.spawnPosition);
                m_player.LoadData(eventArgs.slot.characterData);
            }
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (eventArgs.IsPartOfTheUpdate(SerializationScope.Player) && m_player)
            {
                eventArgs.slot.UpdateCharacterData(m_player.SaveData());
            }
        }
        private void OnPlayerDeath(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.gamplayUIHandle.ShowGameOverScreen();
            // m_input.Disable();
            //  m_player.controller.Disable();
            player.statusEffectReciever.RemoveAllActiveStatusEffects();
            m_playerIsDead = true;
            m_respawnDelay.Reset();
        }
        private void OnRespawnPlayer(object sender, EventActionArgs eventArgs)
        {
            ReturnPlayerToOrginalScene();
            GameplaySystem.campaignSerializer.Load(SerializationScope.Gameplay, false);
            GameplaySystem.LoadGame(GameplaySystem.campaignSerializer.slot, Menu.LoadingHandle.LoadType.Smart);
            m_player.Revitilize();
            m_player.Reset();
            //GameplaySystem.campaignSerializer.Load(true);
            m_playerIsDead = false;
        }
        public IEnumerator PlayerActionChange(Action<PlayerInput> CallBack)
        {

            m_playerInput.enabled = false;
            yield return null;
            CallBack(m_playerInput);
            m_playerInput.enabled = true;
            yield return null;

        }

        private void Start()
        {
            if (m_player)
            {
                m_player = player;
                m_player.OnDeath += OnPlayerDeath;

                var playerCharacter = m_player.character;
                m_playerOriginalScene = playerCharacter.gameObject.scene;
                m_playerOriginalParent = playerCharacter.transform.parent;
            }
            //m_autoReflex.Initialize();
        }

        private void Update()
        {
            m_playerInput = m_characterInput.GetComponent<PlayerInput>();
            //m_autoReflex.Update(Time.deltaTime);
            if (m_playerIsDead)
            {
                m_respawnDelay.Tick(Time.deltaTime);
            }
        }


    }
}