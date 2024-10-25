﻿using DChild.Gameplay;
using Doozy.Engine;
using UnityEngine;

namespace DChild
{
    public class DemoGuide : MonoBehaviour
    {
        public void PauseGame()
        {
            GameplaySystem.PauseGame();
        }
        public void ResumeGame()
        {
            GameplaySystem.ResumeGame();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                GameEventMessage.SendEvent("Demo Guide Close");
            }
        }
    }
}