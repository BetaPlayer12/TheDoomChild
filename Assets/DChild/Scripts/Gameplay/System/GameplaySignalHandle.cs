using UnityEngine;

namespace DChild.Gameplay
{
    public class GameplaySignalHandle : MonoBehaviour
    {
        public void OverridePlayerControl()
        {
            GameplaySystem.playerManager.OverrideCharacterControls();
        }

        public void StopPlayerControlOverride()
        {
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }
    }
}