using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class PlayerTeleportMarker : MonoBehaviour
    {
        public void TeleportPlayerHere()
        {
            GameplaySystem.playerManager.player.character.transform.position = transform.position;
        }
    }

}