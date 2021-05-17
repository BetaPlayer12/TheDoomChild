using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Doozy.Engine;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay
{
    public class GameplaySignalHandle : MonoBehaviour
    {
        public void DoCinematicUIMode(bool value)
        {
            GameEventMessage.SendEvent(value ? "Cinematic Start" : "Cinematic End");
        }

        public void MoveAudioListenerToPlayer()
        {
            GameplaySystem.audioListener.AttachToPlayer();
        }

        public void MoveAudioListenerToCamera()
        {
            GameplaySystem.audioListener.AttachToCamera();
        }

        public void OverridePlayerControl()
        {
            GameplaySystem.playerManager.DisableControls();
        }

        public void StopPlayerControlOverride()
        {
            GameplaySystem.playerManager.EnableControls();
        }

        public void MakePlayerInvulnerable(bool value)
        {
            GameplaySystem.playerManager.player.damageableModule.SetHitboxActive(value);
        }

        public void ShowDialogue(bool value)
        {
            GameEventMessage.SendEvent(value ? "Dialogue Start" : "Dialogue End");
        }

        public void TransferPlayerTo(LocationData locationData)
        {
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;
            character.transform.position = locationData.position;

            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;


            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.ResumeGame();
            GameSystem.LoadZone(locationData.scene, true, OnTransferPlayerDone);
        }

        private void OnTransferPlayerDone()
        {
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;

            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = false;
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }
    }
}
