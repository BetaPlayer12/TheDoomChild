using DChild.Temp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class BaseGameplayUIHandle : MonoBehaviour
    {
        public void ResetGameplayUI()
        {
            GameEventMessage.SendEvent("UI Reset");
            //ToggleBossCombatUI(false);
        }
    }
}

