using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Doozy.Engine;
using Holysoft.Event;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.UI.Map
{
    public class MapLocationButton : MonoBehaviour
    {
        [SerializeField,HideInPrefabAssets]
        private LocationData m_location;

        public void AttemptLocationTransfer()
        {
            GameSystem.RequestConfirmation(OnAccept, $"Travel to {m_location.location.ToString()}");
        }

        private void OnAccept(object sender, EventActionArgs eventArgs)
        {
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;
            character.transform.position = m_location.position;

            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;

            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.ResumeGame();
            GameSystem.LoadZone(m_location.scene, true, OnSceneDone);

        }

        private void OnSceneDone()
        {
            GameEventMessage.SendEvent("Location Transfer");
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = false;
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

        private void OnValidate()
        {
            if(m_location != null)
            {
                gameObject.name = "TravelButton - " + m_location.location.ToString();
            }
        }
    }
}