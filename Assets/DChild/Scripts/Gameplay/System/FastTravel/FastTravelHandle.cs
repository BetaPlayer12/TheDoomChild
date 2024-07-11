using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using UnityEngine;

namespace DChild.Gameplay.FastTravel
{
    public class FastTravelHandle : MonoBehaviour
    {
        public void TransferPlayerTo(LocationData destination)
        {
            var playerManager = GameplaySystem.playerManager;
            var character = playerManager.player.character;
            character.transform.position = destination.position;

            var controller = GameplaySystem.playerManager.OverrideCharacterControls();
            controller.moveDirectionInput = 0;
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            collisionState.forcedCurrentGroundedness = true;

            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.ResumeGame();
            GameSystem.LoadZone(destination.sceneInfo, true, OnTransferPlayerDone);
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
