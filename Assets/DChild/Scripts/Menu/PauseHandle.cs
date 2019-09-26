using System;
using DChild.Gameplay;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class PauseHandle : MonoBehaviour
    {
        public void PauseGame()
        {
            GameplaySystem.PauseGame();
        }

        public void ResumeGame()
        {
            GameplaySystem.ResumeGame();
        }

        public void BackToMainMenu()
        {
            if(GameSystem.RequestConfirmation(OnMainMenuConfirm,"Do you want to return to main menu") == false)
            {
                GameSystem.LoadMainMenu();
            }
        }

        private void OnMainMenuConfirm(object sender, EventActionArgs eventArgs)
        {
            GameSystem.LoadMainMenu();
        }
    }
}