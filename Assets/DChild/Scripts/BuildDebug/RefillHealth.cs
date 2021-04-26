using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class RefillHealth : MonoBehaviour
    {
        [Button]
        public void Refill()
        {
            GameplaySystem.playerManager.player.health.ResetValueToMax();
        }
    }
}
