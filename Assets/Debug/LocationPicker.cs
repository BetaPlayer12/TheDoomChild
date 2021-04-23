using DChild.Gameplay;
using Doozy.Engine;
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