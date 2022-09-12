using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.UI.Map;
using DChild.Menu;
using Doozy.Engine;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class WorldMapHandler : MonoBehaviour
    {
        [SerializeField]
        private ConfirmationHandler m_confirmation;
        [SerializeField, ValueDropdown("GetMapLocationButtons", IsUniqueList = true)]
        private MapLocationButton[] m_locationButtons;

        private LocationData m_transferingTo;

        public void SetFromLocation(Location from)
        {
            foreach (var button in m_locationButtons)
            {
                button.HighlightPlayerIndicator(button.location == from);
            }
        }

        public void SetFromLocation(LocationData locationData) => SetFromLocation(locationData.location);

        public void RequestTransfer(MapLocationButton button)
        {
            m_transferingTo = button.locationData;
            m_confirmation.RequestConfirmation(OnAccept, button.GetTransferMessage());
        }

        private void OnAccept(object sender, EventActionArgs eventArgs)
        {
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;
            character.transform.position = m_transferingTo.position;

            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;
            SetFromLocation(m_transferingTo.location);


            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            LoadingHandle.LoadingDone += OnLoadingDone;
            GameplaySystem.ResumeGame();
            GameSystem.LoadZone(m_transferingTo.scene, true);

            //Force Save for the Demo Delete this after proper saving is done
            GameplaySystem.campaignSerializer.slot.UpdateLocation(m_transferingTo.sceneInfo, m_transferingTo.location, m_transferingTo.position);
        }

        private void OnLoadingDone(object sender, EventActionArgs eventArgs)
        {
            LoadingHandle.LoadingDone -= OnLoadingDone;
            GameEventMessage.SendEvent("Location Transfer");
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = false;
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

        private IEnumerable GetMapLocationButtons() => FindObjectsOfType<MapLocationButton>();

        private void Awake()
        {
            for (int i = 0; i < m_locationButtons.Length; i++)
            {
                m_locationButtons[i].HighlightAvailabilityIndicator(true);
            }
        }
    }
}