using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class MaxHealthAdjuster : MonoBehaviour
    {
        [SerializeField]
        private int increment = 25;
        [Button]
        public void IncreaseMaxHealth()
        {
            int currenthealth =GameplaySystem.playerManager.player.health.maxValue;
            if (currenthealth >= 250)
            {
                GameplaySystem.playerManager.player.health.SetMaxValue(250);
                GameplaySystem.playerManager.player.health.ResetValueToMax();
            }
            else
            {
                int health = currenthealth + increment;
                GameplaySystem.playerManager.player.health.SetMaxValue(health);
                GameplaySystem.playerManager.player.health.ResetValueToMax();
            }
                
        }
        [Button]
        public void DecreaseMaxHealth()
        {
            int currenthealth = GameplaySystem.playerManager.player.health.maxValue;
            if (currenthealth >= 100)
            {
                int health = currenthealth - increment;
                GameplaySystem.playerManager.player.health.SetMaxValue(health);
                GameplaySystem.playerManager.player.health.ResetValueToMax();
            }
            else
            {
                GameplaySystem.playerManager.player.health.SetMaxValue(100);
                GameplaySystem.playerManager.player.health.ResetValueToMax();
            }
               
        }
    }
}
