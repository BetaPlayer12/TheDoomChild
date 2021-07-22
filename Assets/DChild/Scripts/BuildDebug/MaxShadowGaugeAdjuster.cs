using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{

    public class MaxShadowGaugeAdjuster : MonoBehaviour
    {
        [SerializeField]
        private int increment = 25;
        [Button]
        public void IncreaseMaxShadowGauge()
        {
            int currentshadow = GameplaySystem.playerManager.player.magic.maxValue;
            if (currentshadow >= 250)
            {
                GameplaySystem.playerManager.player.magic.SetMaxValue(250);
                GameplaySystem.playerManager.player.magic.ResetValueToMax();
            }
            else
            {
                int shadow = currentshadow + increment;
                GameplaySystem.playerManager.player.magic.SetMaxValue(shadow);
                GameplaySystem.playerManager.player.magic.ResetValueToMax();
            }
               
            
        }
        [Button]
        public void DecreaseMaxShadowGauge()
        {
            int currentshadow = GameplaySystem.playerManager.player.magic.maxValue;
            if (currentshadow >= 100)
            {
                int shadow = currentshadow - increment;
                GameplaySystem.playerManager.player.magic.SetMaxValue(shadow);
                GameplaySystem.playerManager.player.magic.ResetValueToMax();
            }
            else
            {
                GameplaySystem.playerManager.player.magic.SetMaxValue(100);
                GameplaySystem.playerManager.player.magic.ResetValueToMax();
            }
            
        }
    }
}
