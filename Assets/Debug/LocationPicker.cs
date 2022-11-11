using DChild.Gameplay;
using DChild.Temp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class LocationPicker : MonoBehaviour
    {
        void Start()
        {
            GameplaySystem.PauseGame();
            GameEventMessage.SendEvent("Open LocationPicker");
        }
    }
}