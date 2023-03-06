using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class TeleportSkullTeleportHandler : MonoBehaviour
    {
        private void Start()
        {
            GameplaySystem.playerManager.player.character.transform.position = transform.position;
        }
    }
}

