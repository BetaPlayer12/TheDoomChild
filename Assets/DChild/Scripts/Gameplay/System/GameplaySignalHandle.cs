using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Doozy.Engine;
using Holysoft.Event;
using Sirenix.OdinInspector;
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
    }
}
