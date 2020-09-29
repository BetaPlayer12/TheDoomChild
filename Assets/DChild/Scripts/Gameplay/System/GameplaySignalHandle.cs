using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay
{
    public class GameplaySignalHandle : MonoBehaviour
    {
        public void DoCinematicUIMode(bool value)
        {
            GameEventMessage.SendEvent(value ? "Cinematic Start" : "Cinematic End");
        }

        public void OverridePlayerControl()
        {
            GameplaySystem.playerManager.DisableControls();
        }

        public void StopPlayerControlOverride()
        {
            GameplaySystem.playerManager.EnableControls();
        }
    }
}